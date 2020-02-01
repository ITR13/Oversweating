import random

PORT = 5000

COLORS = ["pink", "blue", "orange", "green", "yellow"]

STOPPED = "stopped"
DISABLED = "disabled"
RUNNING = "running"
WARNING = "warning"
FAILED = "failed"

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
		"components": ["button", "button", "button", "switch", "lever", "lever", "compass"],
		"chunks": [ (0, 1, 2), (3,), (4, 5), (6,) ],
	},
	{
		"components": ["button", "button", "compass", "compass", "compass"],
		"chunks": [ (0, 1, 2), (3, 4, 5), ],
	},
]



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


