from threading import Thread
from constants import *
import random
from time import sleep, time


class Gameloop(Thread):
	daemon = True
	lost = False
	stopped = False

	def __init__(self, ship, *args, **kwargs):
		super(Gameloop,self).__init__(*args, **kwargs)
		self.ship = ship

		self.wave_timer = 5

		self.waves = WAVES
		self.current_wave = self.next_wave()

		self.safe_stations = ship.station_count


	def run(self):
		prev_time = time()
		while not self.lost and not self.stopped:
			sleep(0)
			current_time = time()
			delta_time = current_time - prev_time
			prev_time = current_time

			self.check_next_wave(delta_time)
			# self.check_faults_cleared()
			# self.check_faults_lost()
			
		if self.stopped:
			return


	def check_next_wave(self, dt):
		faults = self.current_wave["faults"]
		if self.safe_stations > 0:
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

	def recalc_safe_stations(self):
		self.safe_stations = 0
		for station in self.ship.stations:
			if station.status == RUNNING:
				self.safe_stations += 1



class Station:
	def __init__(self, index, preset_index):
		self.index = index
		self.color = COLORS[index]
		self.status = RUNNING
		self.preset_index = preset_index
		self.component_names = PRESETS[preset_index]["components"]
		self.chunks = PRESETS[preset_index]["chunks"]
		self.components = [0] * len(self.component_names)

		self.faults = None

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

	def set_faults(self, faults):
		self.status = WARNING
		self.faults = faults

	def as_dict(self):
		return {
			"status": self.status,
			"color": self.color,
			"preset_index": self.preset_index,
			"components": self.components,
			"faults": self.faults,
		}


class Ship:
	def __init__(self, station_count, player_count):
		self.station_count = station_count
		self.player_count = player_count

		self.stations = [
			Station(i, 0)
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
		faults = [
			(station.index, station.generate_target(faults))
			for faults, station
			in zip(fault_list, selected_stations)
		]
		selected_station.set_faults(faults)
		print(f"Warning on {selected_station.index}")
		

	def stop(self):
		self.thread.stopped = True
		stations = []

	def fail(self):
		for station in self.stations:
			station.fail()