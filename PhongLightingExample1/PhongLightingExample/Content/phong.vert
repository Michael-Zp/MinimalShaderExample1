#version 430 core				

uniform mat4 camera;

in vec3 position;
in vec3 normal;

out vec3 pos;
out vec3 worldPos;
out vec3 n;

void main() 
{
	pos = position;
	n = normalize(normal);

	gl_Position = camera * vec4(position, 1.0);
	worldPos = gl_Position.xyz;
}