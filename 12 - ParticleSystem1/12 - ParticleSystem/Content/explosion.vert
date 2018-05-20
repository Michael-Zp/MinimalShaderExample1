#version 430 core				

uniform mat4 camera;
uniform float pointSize = 1;

in vec4 position;
in float fadeToRed;

out float fadeToRedFrag;

void main() 
{
	fadeToRedFrag = fadeToRed;
	vec4 pos = camera * position;
	gl_PointSize = (1 - pos.z / pos.w) * 1000 * pointSize;
	gl_Position = pos;
}