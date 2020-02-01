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
]


