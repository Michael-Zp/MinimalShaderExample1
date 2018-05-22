#version 430 core

const float PI = 3.14159265359;

uniform sampler2D image;
uniform float effectScale = 0.3;

in vec2 uv;

//sobel operator for x
mat3 kernel = mat3( 
	0.0, -1.0, 0.0, 
	-1.0, 5.0, -1.0, 
	0.0, -1.0, 0.0 
	);

mat3 ident = mat3(
	0, 0, 0,
	0, 1, 0,
	0, 0, 0
);

float grayScale(vec3 color)
{
	return 0.2126 * color.r + 0.7152 * color.g + 0.0722 * color.b;
}

float convolve(mat3 a, mat3 b)
{
	return dot(a[0], b[0]) + dot(a[1], b[1]) + dot(a[2], b[2]);
}

void main()
{
	mat3 I;
	for (int i = 0; i < 3; ++i) 
	{
		for (int j = 0; j < 3; ++j) 
		{
			vec3 aSample  = texelFetch(image, ivec2(gl_FragCoord) + ivec2(i - 1, j - 1), 0).rgb;
			I[i][j] = grayScale(aSample);
		}
	}
	
	float gx = convolve(kernel, I);
	gl_FragColor = vec4(vec3(gx), 1.0);
}
