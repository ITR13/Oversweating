from PIL import Image

colors = ["Pink", "Yellow", "Red", "Blue", "Purple"]
color_count = len(colors)

button = (
	"Button",
	[
		f"Button_{state}.png"
		for color in colors
		for state in ["Off", color]
	]
)

compass = (
	"Compass",
	[
		f"Compass_{direction}_{color}.png"
		for color in colors
		for direction in ["North", "East", "South", "West"]
	]
)

lever = (
	"Lever",
	[
		f"Lever{pos}_{color}.png"
		for color in colors
		for pos in ["", "_Up", "_Down"]
	]
)

switch = (
	"Switch",
	[
		f"Switch_{state}_{color}.png"
		for color in colors
		for state
		in ["Off", "On"]
	]
)

station = (
	"StationIcon",
	[
		f"Station_{color}.png"
		for color in colors
	]
)

warning = (
	"Warning",
	[
		f"Warning_{state}.png"
		for color in colors
		for state in ["Off", color]
	]
)

for name, paths in [button, compass, lever, switch, station, warning]:
	frames = [
		Image.open(path)
		for path in paths
	]
	tile_width = frames[0].size[0]
	tile_height = frames[0].size[1]
	
	rows = (len(frames) // color_count)
	
	width = tile_width * color_count
	height = tile_height * rows
	
	spritesheet = Image.new("RGBA",(width, height))
	for n, frame in enumerate(frames):
		x = (n // rows) * tile_width
		y = (n % rows) * tile_height
		
		box = (x, y, x+tile_width, y+tile_height)
		spritesheet.paste(frame, box)
	
	spritesheet.save(f"./../Resources/{name}Sheet.png", "PNG")


