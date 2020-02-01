from threading import Thread
from constants import *
import random

class Gameloop(Thread):
	daemon = True
	lost = False

	def __init__(self, ship, *args, **kwargs):
		super(Gameloop,self).__init__(*args, **kwargs)
		self.ship = ship

	def run(self):
		while not self.lost:
			pass
		

class Station:
	def __init__(self, index, preset_index):
		self.index = index
		self.color = COLORS[index]
		self.status = RUNNING
		self.preset_index = preset_index
		self.component_names = PRESETS[preset_index]["components"]
		self.chunks = PRESETS[preset_index]["chunks"]
		self.components = [0] * len(self.component_names)
		
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
	
	def as_dict(self):
		return {
			"status": self.status,
			"color": self.color,
			"preset_index": self.preset_index,
			"components": self.components,
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

	def stop(self):
		self.thread.stop()
		stations = []
	
	def fail(self):
		for station in self.stations:
			station.fail()