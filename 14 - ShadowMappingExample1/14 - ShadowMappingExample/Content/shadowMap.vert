#version 430 core	

uniform mat4 lightCamera;
uniform mat4 camera;

in vec4 position;
in vec3 normal;

out blockData
{
	vec3 normal;
	float distanceToLight;
} o;

void main() 
{
	o.normal = normal;
	gl_Position = camera * position;
	
	vec3 camPos = vec3(camera[0][3], camera[1][3], camera[2][3]) / camera[3][3];
	o.distanceToLight = distance((lightCamera * position).xyz, camPos);
}