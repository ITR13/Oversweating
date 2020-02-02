from threading import Thread
from constants import *
import random
from time import sleep, time


class Gameloop(Thread):
	lost = False
	stopped = False

	def __init__(self, ship, *args, **kwargs):
		super(Gameloop,self).__init__(*args, **kwargs)
		self.daemon = True
		
		self.ship = ship

		self.wave_timer = 5

		self.waves = WAVES
		self.current_wave = self.next_wave()

		self.safe_stations = ship.station_count


	def run(self):
		prev_time = time()
		while not self.lost and not self.stopped:
			sleep(1)
			current_time = time()
			delta_time = current_time - prev_time
			prev_time = current_time

			self.check_next_wave(delta_time)
			self.check_faults_cleared()
			self.check_faults_lost(current_time)

		if self.stopped:
			return
			
		print("Game lost")


	def check_next_wave(self, dt):
		faults = self.current_wave["faults"]
		if self.safe_stations == 0:
			return

		self.wave_timer -= dt
		if self.wave_timer > 0:
			return

		self.wave_timer +=  self.current_wave["delay"] * (0.8 ** (self.ship.player_count - 1))
		self.current_wave = self.next_wave()
		self.ship.create_faults(faults)

		self.recalc_safe_stations()

	def next_wave(self):
		if len(self.waves) == 0:
			return self.waves[0]
		wave = self.waves[0]
		self.waves = self.waves[1:]
		return wave

	def check_faults_cleared(self):
		for station in self.ship.stations:
			station_faults = station.faults
			if station_faults is None:
				continue

			for other_station_index, faults in station_faults:
				other_station = self.ship.stations[other_station_index]
				faults = [
					component_fault
					for chunk_faults in faults
					for component_fault in chunk_faults
				]
				if not other_station.has_state(faults):
					break
			else:
				station.clear_faults()
				self.recalc_safe_stations()
		
	def check_faults_lost(self, current_time):
		for station in self.ship.stations:
			if station.end_time is None:
				continue
			if station.end_time + 1 <= current_time:
				self.ship.fail()
				self.lost = True
				break

	def recalc_safe_stations(self):
		self.safe_stations = 0
		for station in self.ship.stations:
			if station.status == RUNNING:
				self.safe_stations += 1


class Station:
	def __init__(self, ship, index, preset_index):
		self.ship = ship
		self.index = index
		self.color = COLORS[index]
		self.status = RUNNING
		self.preset_index = preset_index
		self.component_names = PRESETS[preset_index]["components"]
		self.chunks = PRESETS[preset_index]["chunks"]
		self.components = [0] * len(self.component_names)

		self.faults = None
		self.end_time = None
		self.fault_count = 0

	def activate(self, component_index, button_index):
		component = self.component_names[component_index]
		self.components[component_index] = (
			COMPONENTS[component](
				self.components[component_index],
				button_index
			)
		)

	def generate_target(self, faults):
		if faults < 0:
			return []
		if faults > len(self.chunks):
			faults = self.chunks

		targets = random.sample(self.chunks, faults)
		targets.sort()
		return [
			self.generate_chunk_target(chunk)
			for chunk in targets
		]

	def generate_chunk_target(self, chunk):
		count = random.randint(1, len(chunk))
		selected = set(random.sample(chunk, count))
		return [
			(component_index, self.generate_component_target(component_index))
			if component_index in selected
			else (component_index, self.components[component_index])
			for component_index
			in chunk
		]

	def generate_component_target(self, component_index):
		component = self.component_names[component_index]
		value = self.components[component_index]
		return COMPONENT_MODIFIERS[component](value)

	def fail(self):
		self.status = FAILED

	def set_faults(self, faults, end_time):
		self.status = WARNING
		self.faults = faults
		self.end_time = end_time
		self.fault_count += 1
		print(f"Warning on {self.index}, ending at {end_time}")

	def clear_faults(self):
		self.status = RUNNING
		self.faults = None
		self.end_time = None
		print(f"Cleared {self.index}")

	def has_state(self, component_states):
		return all(
			self.components[component_index] == value
			for component_index, value in component_states
		)

	def as_dict(self):
		faults = [
			{
				"station_id": fault[0],
				"chunks": [
					{
						"component_name": self.ship.stations[fault[0]].component_names[chunk[0][0]],
						"targets": [
							{
								"component_id": target[0],
								"target_value": target[1],
							}
							for target in chunk
						]
					}
					for chunk in fault[1]
				],
			}
			for fault in self.faults
		] if self.faults is not None else None
	
		return {
			"status": self.status,
			"color": self.color,
			"preset_index": self.preset_index,
			"components": self.components,
			"faults": faults,
			"fault_count": self.fault_count,
			"end_time": self.end_time if self.end_time is not None else -1.0,
		}


class Ship:
	def __init__(self, station_count, player_count):
		self.station_count = station_count
		self.player_count = player_count

		self.stations = [
			Station(self, i, 0)
			for i in range(station_count)
		]

		self.thread = Gameloop(self)
		self.thread.start()

	def create_faults(self, fault_list):
		empty_stations = [
			station
			for station
			in self.stations
			if station.status == RUNNING
		]
		selected_station = random.choice(empty_stations)
		other_stations =[
			station
			for station
			in self.stations
			if station != selected_station
		]

		selected_stations = random.sample(
			other_stations,
			len(fault_list)
		)
		station_faults = [
			(station.index, station.generate_target(faults))
			for faults, station
			in zip(fault_list, selected_stations)
		]
		selected_station.set_faults(station_faults, time() + 32)


	def stop(self):
		self.thread.stopped = True
		stations = []

	def fail(self):
		for station in self.stations:
			station.fail()