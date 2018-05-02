#version 430 core

uniform mat4 camera;

in vec3 position;
in vec3 normal;
in float specularity;
in float translucency;
in float refractionIndex;

out vec3 pos;
out vec3 n;
out float spec;
out float transl;
out float refractIdx;

void main() 
{
	pos = position;
	n = normal;
	spec = specularity;
	transl = translucency;
	refractIdx = refractionIndex;

	gl_Position = camera * vec4(position, 1.0);
}