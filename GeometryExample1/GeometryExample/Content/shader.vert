#version 430 core				

uniform float time;

in vec4 in_position;
in vec2 in_velocity;

void main() 
{
	gl_PointSize = 10.0;
	vec2 newPos = in_position.xy + time * in_velocity;

	//newPos = abs(mod(newPos + vec2(3), 4) - vec2(2)) - vec2(1);

	newPos = mod(newPos + vec2(1), 2) - vec2(1);

	gl_Position = vec4(newPos, 0.0, 1.0);
}