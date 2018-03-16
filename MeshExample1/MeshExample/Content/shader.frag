#version 430 core
uniform sampler2D texDiffuse;

in vec3 n;
out vec4 color;
in vec2 uvPosition;

void main() 
{
	//use normal as color
	color = vec4(n, 1.0);
	color = texture(texDiffuse, uvPosition);
}