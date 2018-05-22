#version 430 core

uniform mat4 camera;

in Data
{
	vec3 position;
	vec3 normal;
	flat uint material;
} inData;

layout(location=0) out vec4 normal;
layout(location=1) out vec4 depth;
layout(location=2) out vec4 materialColor;

void main() 
{
	const vec4 materials[] = { vec4(1), vec4(1, 0, 0, 1), vec4(0, 1, 0, 1), vec4(0, 1, 1, 1) };
	
	vec3 camPos = vec3(camera[3].xyz);

	normal = vec4(normalize(inData.normal), 1.0);
	depth = vec4(vec3(distance(camPos, inData.position) / 10), 10);
	materialColor = materials[inData.material];
}