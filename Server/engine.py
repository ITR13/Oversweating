from threading import Thread
from constants import COLORS, RUNNING, WARNING, FAILED


class Station:
	def __init__(self, index):
		self.index = index
		self.color = COLORS[index]
		self.status = RUNNING
		
	def fail(self):
		self.status = FAILED
	
	def as_dict(self):
		return {
			"status": self.status,
			"color": self.color,
		}


class Ship:
	def __init__(self, station_count, player_count):
		self.station_count = station_count
		self.player_count = player_count
		
		self.stations = [
			Station(i)
			for i in range(station_count)
		]
