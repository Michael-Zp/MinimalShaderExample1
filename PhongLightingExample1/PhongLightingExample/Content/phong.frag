#version 430 core

uniform vec3 light1DirectionWorld;
uniform vec4 light1Color;
uniform vec4 ambientLightColor;
uniform vec4 materialColor;
uniform vec4 cameraPosition;

uniform vec3 light2Position;
uniform vec4 light2Color;

uniform vec3 light3Position;
uniform vec3 light3Direction;
uniform float light3Angle;
uniform vec4 light3Color;

in vec3 pos;
in vec3 worldPos;
in vec3 n;

out vec4 color;

void main() 
{
	vec3 normal = normalize(n);
	 
	vec3 lightToPos;
	
#define DIRECTIONAL 0;
#define POINT 0;
#define SPOT 1;

#if DIRECTIONAL
	lightToPos = light1DirectionWorld;
#elif POINT
	lightToPos = -light2Position + pos;
#elif SPOT
	//Spot
	lightToPos = -light3Position + pos;
	

#endif

	lightToPos = normalize(lightToPos);

#if SPOT
	if(acos(dot(lightToPos, normalize(light3Direction))) > light3Angle)
	{
		color = materialColor * ambientLightColor;
		return;
	}
#endif

	

	vec4 ambient = materialColor * ambientLightColor;
	vec4 diffuse = materialColor * light1Color * max(0, dot(-lightToPos, normal));
	
	vec3 posToEye = normalize((cameraPosition.xyz - worldPos).xyz);

	vec3 refVec = reflect(-lightToPos, normal);
	vec4 specular = materialColor * light1Color * pow(dot(posToEye, refVec), 8);

	float applySpecular = step(0.0, dot(normal, -lightToPos)) * step(0.0, dot(refVec, posToEye));

	specular *= applySpecular;
	
	vec4 vertexColor = ambient + diffuse + specular;

	color = vec4(vertexColor.rgb, 1);
}