from flask import Flask, request, jsonify, url_for
from utils import parse_form
from engine import Ship
from constants import PORT, STOPPED, DISABLED

app = Flask(__name__)


ship = None


@app.route('/status')
def global_status():
	if ship is None or len(ship.stations) == 0:
		return jsonify({"status": STOPPED, "station_count": 0, "player_count": 0, "colors": []})
	
	station = ship.stations[0]
	return jsonify({
		"status": station.status, 
		"station_count": ship.station_count,
		"player_count": ship.player_count,
		"colors": [station.color for station in ship.stations],
	})


@app.route('/setup', methods=['GET', 'POST'])
def setup_game():
	global ship
	if ship is not None:
		ship.stop()

	success, data = parse_form({'station_count':int, 'player_count':int})
	if not success:
		return data
	
	ship = Ship(data["station_count"], data["player_count"])
	return "Success"


@app.route('/stop')
def stop_game():
	global ship
	if ship is None:
		return "Already stopped"
	ship.stop()
	ship = None
	return "Success"


@app.route('/info/<int:station_id>')
def station_info(station_id):
	if ship is None:
		return jsonify({"status": STOPPED})
	if station_id < 0 or station_id >= len(ship.stations):
		return jsonify({"status": DISABLED})
	
	return jsonify(ship.stations[station_id].as_dict())

@app.route('/activate/<int:station_id>', methods=['GET', 'POST'])
def activate_component(station_id):
	if ship is None:
		return "Ship is stopped"
	if station_id < 0 or station_id >= len(ship.stations):
		return "Station doesn't exist"
		
	station = ship.stations[station_id]
	success, data = parse_form({'component_index': int, 'button_index': int})
	if not success:
		return data

	component_index = data["component_index"]
	button_index = data["button_index"]
	
	if component_index < 0 or component_index >= len(station.components):
		return "Component doesn't exist"
	
	station.activate(component_index, button_index)
	return "Success"


@app.route('/ready/<int:station_id>')
def ready_system(station_id):
	if ship is None:
		return "Ship is stopped"
	if station_id < 0 or station_id >= len(ship.stations):
		return "Station doesn't exist"
		
	station = ship.stations[station_id]
	if station.ready():
		return "Success"
	return "Station not waiting"


@app.route('/debug/<int:station_id>/generate_targets/<int:faults>')
def debug_generate_targets(station_id, faults):
	if ship is None:
		return {}
	if station_id < 0 or station_id >= len(ship.stations):
		return {}
	
	station = ship.stations[station_id]
	return jsonify(station.generate_target(faults))


if __name__ == '__main__':
	import logging
	log = logging.getLogger('werkzeug')
	log.setLevel(logging.WARNING)
	from flask.logging import default_handler
	app.logger.removeHandler(default_handler)
	app.run(
		host='0.0.0.0', 
		port=PORT, 
		threaded=False, 
		debug=False
	)