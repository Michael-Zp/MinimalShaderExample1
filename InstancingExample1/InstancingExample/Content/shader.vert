#version 430 core				

uniform float iGlobalTime;

in vec3 instancePosition;
in vec3 velocity;

in vec3 position;
in vec3 normal;

out vec3 var_color;

void main() 
{
	var_color = normal;

	vec3 pos = position + instancePosition + (iGlobalTime * velocity);
	gl_Position = vec4(pos, 1.0);
}