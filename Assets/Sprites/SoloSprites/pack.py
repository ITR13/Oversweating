from PIL import Image

color_count = 5

button = (
	"Button",
	[
		f"Button_{color}.png"
		for color
		in ["Gray", "Red"] * color_count
	]
)

compass = (
	"Compass",
	[
		f"Compass_{direction}.png"
		for direction
		in ["North", "East", "South", "West"] * color_count
	]
)

lever = (
	"Lever",
	[
		f"Lever{pos}.png"
		for pos
		in ["", "_Up", "_Down"] * color_count
	]
)

switch = (
	"Switch",
	[
		f"Switch_{state}.png"
		for state
		in ["Off", "On"] * color_count
	]
)

for name, paths in [button, compass, lever, switch]:
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


