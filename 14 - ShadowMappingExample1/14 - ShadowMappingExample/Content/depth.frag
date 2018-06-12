#version 430 core

in blockData
{
	vec3 normal;
	float depth;
} i;

out float color;

void main() 
{
	color = i.depth;
}