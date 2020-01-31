from flask import Flask, request, jsonify, url_for
from utils import parse_form
from engine import Ship
from constants import PORT, STOPPED, DISABLED

app = Flask(__name__)


ship = None


@app.route('/setup', methods=['GET', 'POST'])
def setup_game():
	global ship

	success, data = parse_form({'station_count':int, 'player_count':int})
	if not success:
		return data
	
	ship = Ship(data["station_count"], data["player_count"])
	return "Success"

@app.route('/stop')
def stop_game():
	global ship
	ship = None


@app.route('/info/<int:station_id>')
def station_info(station_id):
	if ship is None:
		return jsonify({"status": STOPPED})
	if station_id < 0 or station_id >= len(ship.stations):
		return jsonify({"status": DISABLED})
	
	return jsonify(ship.stations[station_id].as_dict())


if __name__ == '__main__':
	app.run(
		host='0.0.0.0', 
		port=PORT, 
		threaded=False, 
		debug=False
	)