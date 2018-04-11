#version 430 core				

uniform mat4 camera;

uniform vec3 light1DirectionWorld;
uniform vec4 light1Color;
uniform vec4 ambientLightColor;
uniform vec4 materialColor;
uniform vec4 cameraPosition;

in vec3 position;
in vec3 normal;

out vec3 pos;
out vec3 n;
out vec4 vertexColor;

void main() 
{
	pos = position;
	n = normalize(normal);

	gl_Position = camera * vec4(position, 1.0);

	
	vec4 ambient = materialColor * ambientLightColor;
	vec4 diffuse = materialColor * light1Color * max(0, dot(-light1DirectionWorld, n));

	vec3 posToEye = normalize((cameraPosition - gl_Position).xyz);
	vec3 refVec = reflect(-light1DirectionWorld, n);
	vec4 specular = materialColor * light1Color * pow(max(0, dot(refVec, posToEye)), 16);

	float applySpecular = step(0.0, dot(n, -light1DirectionWorld)) * step(0.0, dot(refVec, posToEye));

	specular *= applySpecular;
	
	vertexColor = ambient + diffuse + specular;
}