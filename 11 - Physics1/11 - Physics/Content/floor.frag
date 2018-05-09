#version 430 core

uniform vec4 floorColor;

out vec4 color;

void main() 
{
	color = vec4(floorColor);
}