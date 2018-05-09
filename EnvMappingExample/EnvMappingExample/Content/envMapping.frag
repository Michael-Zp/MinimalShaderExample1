#version 430 core

uniform sampler2D envMap;
uniform sampler2D materialTex;

uniform vec3 cameraPosition;

in vec3 pos;
in vec3 n;
in float spec;
in float transl;
in float refractIdx;

out vec4 color;
const float PI = 3.14159265359;

vec2 projectLongLat(vec3 direction) {
  float theta = atan(direction.x, -direction.z) + PI;
  float phi = acos(-direction.y);
  return vec2(theta / (2*PI), phi / PI);
}

void main() 
{
	vec3 normal = normalize(n);

	vec3 dir = normalize(pos - cameraPosition); //for sky dome camera should stay fixed in the center
	vec3 reflectDir = reflect(dir, normal);

	vec3 refractDir = refract(dir, normal, 1.0f / refractIdx);

	vec4 matColor = texture(materialTex, projectLongLat(normal));
	vec4 reflectColor = texture(envMap, projectLongLat(reflectDir));
	vec4 refractColor = texture(envMap, projectLongLat(refractDir));

	color = mix(mix(matColor, reflectColor, spec), refractColor, transl);
}