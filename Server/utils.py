from flask import request

def parse_form(keys):
	form = request.form if request.method == 'POST' else request.args
	
	missing = [i for i in keys.keys() if i not in form]
	if len(missing)>0:
		return (False, 'Missing the following parameters: '+' '.join(missing))
	
	data = {}
	for key in keys.keys():
		value = form.get(key)
		try:
			value = keys[key](value)
		except Exception:
			return (False, f'Failed to parse {key} with value {value}')
		data[key] = value
	
	return (True, data)
