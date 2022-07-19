extends TouchScreenButton

var radius = Vector2(12,12)
var bound = 32
var ongoing_drag = -1
var accel = 100
var threshold = 30
func _ready():
	position -= radius
	
func get_button_pos():
	return position + radius

func _physics_process(delta):
	if(ongoing_drag == -1):
		var pos_difference = (Vector2.ZERO - radius) - position
		position += pos_difference * accel * delta
		
		
func _input(event):
	if(event is InputEventScreenDrag or (event is InputEventScreenTouch and event.is_pressed())):
		var eventdfc = (event.position - get_parent().global_position).length()
		
		if(eventdfc <= bound * global_scale.x or event.get_index() == ongoing_drag):
			set_global_position(event.position - radius)
	
			if(get_button_pos().length() > bound):
				set_position(get_button_pos().normalized() * bound - radius)
			ongoing_drag = event.get_index()
	if(event is InputEventScreenTouch and !event.is_pressed() and event.get_index() == ongoing_drag):
		ongoing_drag = -1

func get_val() -> Vector2:
	if(get_button_pos().length() > threshold):
		return get_button_pos().normalized()
	else:
		return Vector2.ZERO
