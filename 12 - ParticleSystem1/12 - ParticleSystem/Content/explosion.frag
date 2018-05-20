#version 430 core
uniform sampler2D texParticle;

in float fadeToRedFrag;

out vec4 color;

void main() 
{
	//color = vec4(gl_PointCoord, 0, 1);
	color = texture(texParticle, gl_PointCoord);
	color.a *=  fadeToRedFrag; //fade out

	float g = (clamp((fadeToRedFrag + 0.45), 0, 1) - 0.5) / 0.5;
	float b = (fadeToRedFrag - 0.55) / 0.45;
	color.rgb = 0.3 * vec3(1.0, g, b);
}