import random

PORT = 5000

COLORS = ["pink", "yellow", "red", "blue", "purple"]

STOPPED = "stopped"
DISABLED = "disabled"
RUNNING = "running"
WARNING = "warning"
FAILED = "failed"
WAITING = "waiting"
READY = "ready"


LEVER_MAX = 3

COMPONENTS = {
	"button": lambda c, b: 1-c,
	"lever": lambda c, b: b,
	"switch": lambda c, b: 1-c,
	"compass": lambda c, b: (c+1)%4,
}

COMPONENT_MODIFIERS = {
	"button": lambda c: 1-c,
	"lever": lambda c: (c + random.randint(1, LEVER_MAX - 1)) % LEVER_MAX,
	"switch": lambda c: 1-c,
	"compass": lambda c: (c+random.randint(1, 3))%4,
}

PRESETS = [
	{
		"components": ["button"] * 3+ ["switch"] + ["lever"] * 2 + ["compass"],
		"chunks": [ (0, 1, 2), (3,), (4, 5), (6,) ],
	},
	{
		"components": ["compass"] * 2 + ["button"] * 4 + ["switch"],
		"chunks": [ (0, 1), (2, 3, 4, 5), (6,) ],
	},
	{
		"components": ["lever"] + ["switch"] * 4 + ["button"] * 2 + ["compass"],
		"chunks": [ (0, ), (1, 2, 3, 4), (5, 6), (7, ) ],
	},
	{
		"components": ["compass"] * 2 + ["button"] * 4 + ["lever"],
		"chunks": [ (0, 1), (2, 3, 4, 5), (6,) ],
	},
	{
		"components": ["lever"] * 2 + ["compass"] + ["button"] * 2 + ["switch"] * 2,
		"chunks": [ (0, 1), (2, ), (3, 4), (5, 6,) ],
	},
]

for _preset in PRESETS:
	for _chunk in _preset["chunks"]:
		_components = _preset["components"]
		_current_type = None
		for _component_index in _chunk:
			if _component_index < 0 or _component_index >= len(_components):
				print(f"Component needs to be 0 <= {_component_index} < {len(_components)} (in {_preset})")
			elif _current_type is None:
				_current_type = _components[_component_index]
			elif _current_type != _components[_component_index]:
				print(f"{_component_index} is not {_current_type}, but {_components[_component_index]} (in {_preset})")



WAVES = [
	{ "delay": 15, "faults": [1] },
	{ "delay": 15, "faults": [1] },
	{ "delay": 15, "faults": [2] },
	{ "delay": 15, "faults": [1] },
	{ "delay": 20, "faults": [3] },

	{ "delay": 13, "faults": [1] },
	{ "delay": 13, "faults": [1] },
	{ "delay": 13, "faults": [1, 2] },
	{ "delay":  5, "faults": [2] },
	{ "delay":  5, "faults": [2] },
	{ "delay": 13, "faults": [1] },

	{ "delay": 10, "faults": [2] },
	{ "delay": 10, "faults": [1, 1] },
	{ "delay": 10, "faults": [3] },
	{ "delay": 10, "faults": [2] },
	{ "delay": 15, "faults": [4] },

	{ "delay":  8, "faults": [2] },
	{ "delay":  8, "faults": [1, 1, 1] },
	{ "delay":  8, "faults": [2] },
	{ "delay": 13, "faults": [2, 2] },
	{ "delay": 15, "faults": [2, 2, 1] },

	{ "delay":  6, "faults": [1, 1] },
	{ "delay":  6, "faults": [1, 1] },
	{ "delay":  6, "faults": [2, 1] },
	{ "delay":  6, "faults": [2, 1] },
	{ "delay":  6, "faults": [2, 2] },
	{ "delay":  6, "faults": [2, 2] },
	{ "delay":  6, "faults": [3, 2] },
	{ "delay": 15, "faults": [3, 2] },

	{ "delay":  5, "faults": [1] },
	{ "delay":  5, "faults": [3, 2, 1] },
	{ "delay":  5, "faults": [2, 2] },
	{ "delay":  5, "faults": [2, 2, 2] },
	{ "delay":  5, "faults": [3, 2, 2] },

	{ "delay":  5, "faults": [3, 2, 2] },
	{ "delay":  5, "faults": [3, 3, 1] },
	{ "delay":  5, "faults": [3, 3, 2] },
	{ "delay":  5, "faults": [3, 3, 2] },
	{ "delay":  5, "faults": [4, 4, 2] },
	{ "delay":  5, "faults": [4, 4, 3] },
	{ "delay":  5, "faults": [4, 4, 4] },

	{ "delay":  3, "faults": [4, 4, 4] },
	{ "delay":  3, "faults": [4, 4, 4] },
	{ "delay":  3, "faults": [4, 4, 4] },
]


