#version 430 core

uniform sampler2D normalTex;
uniform sampler2D depthTex;
uniform sampler2D materialColorTex;

in vec2 uv;

out vec4 color;

void main() 
{
	vec3 normal = texture2D(normalTex, uv).rgb;
	vec3 depth = texture2D(depthTex, uv).xyz * texture2D(depthTex, uv).w;
	vec3 materialColor = texture2D(materialColorTex, uv).rgb;

	
	vec3 light = normalize(vec3(1, 0.5, 1));
	vec3 albedo = materialColor;

	vec3 ambient = 0.3 * albedo;
	vec3 diffuse = max(0, dot(normal, light)) * albedo;
	

	color = vec4(ambient + diffuse, 1);

	/*
	if(uv.x < 0.33)
	{
		color = vec4(normal, 1.0);
	}
	else if(uv.x > 0.33 && uv.x < 0.66)
	{
		color = vec4(depth, 1.0);
	}
	else
	{
		color = vec4(materialColor, 1.0);
	}
	*/
}