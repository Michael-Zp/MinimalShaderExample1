#version 430 core				
in vec3 position;
in vec3 normal;
in vec2 uv;

out vec3 n;
out vec2 uvPosition;

void main() 
{
	n = normal;
	uvPosition = uv;
	gl_Position = vec4(position, 1.0);
}