#version 430 core				

uniform float time;

in vec4 in_position;
in vec2 in_velocity;

void main() 
{
	gl_PointSize = 100.0;
	vec2 newPos = in_position.xy + time * in_velocity;
	newPos += 2.0;
	newPos = mod(newPos, 4.0);
	
	newPos.x = newPos.x * step(1.0, newPos.x) + (1.0 + (1.0 - newPos.x)) * step(newPos.x, 1.0);
	newPos.x = newPos.x * step(newPos.x, 3.0) + (3.0 - (newPos.x - 3.0)) * step(3.0, newPos.x);

	newPos.y = newPos.y * step(1.0, newPos.y) + (1.0 + (1.0 - newPos.y)) * step(newPos.y, 1.0);
	newPos.y = newPos.y * step(newPos.y, 3.0) + (3.0 - (newPos.y - 3.0)) * step(3.0, newPos.y);
	
	newPos -= 2.0;
	gl_Position = vec4(newPos, 0.0, 1.0);
}