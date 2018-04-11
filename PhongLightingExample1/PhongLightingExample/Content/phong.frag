#version 430 core

in vec3 pos;
in vec3 n;
in vec4 vertexColor;

out vec4 color;

void main() 
{
	vec3 normal = normalize(n);

	color =  vec4(vertexColor.rgb, 1);
}