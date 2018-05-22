#version 430 core

uniform sampler2D image;

in vec2 uv;

float random(in float a, in float b) { return fract((cos(dot(vec2(a,b) ,vec2(12.9898,78.233))) * 43758.5453)); }

float circle(vec2 coord, float startFadeOut, float endFadeOut)
{
	float dist = length(vec2(0.5) - coord);
	return 1 - smoothstep(startFadeOut, endFadeOut, dist);
}

void main () {
	vec2 xyOffsetScale = vec2(0.002);
	
	vec2 rOffset =  1 * xyOffsetScale;
	vec2 gOffset = -1 * xyOffsetScale;
	vec2 bOffset =  2 * xyOffsetScale;

	float colorDimFactor = 1;
	float uvLineOffset = 0;
	
	int lineSize = 6;
	if(mod(gl_FragCoord.y, lineSize * 2) < lineSize)
	{
		colorDimFactor = .2;
		float line = floor(gl_FragCoord.y / (lineSize * 2));
		uvLineOffset = random(line, line) * 0.025;
	}

	vec2 uvOffset = vec2(sin(uv.y * 5) * 0.05 + random(uv.y, uv.y) * 0.0075 + uvLineOffset, uv.x / 10);
    
	vec2 newRUv = uv - rOffset + uvOffset;
	vec2 newGUv = uv - gOffset + uvOffset;
	vec2 newBUv = uv - bOffset + uvOffset;

	float xRInBounds = step(0.0, newRUv.x) * step(newRUv.x, 1.0);
	float xGInBounds = step(0.0, newGUv.x) * step(newGUv.x, 1.0);
	float xBInBounds = step(0.0, newBUv.x) * step(newBUv.x, 1.0);

	float r = texture2D(image, newRUv).r * xRInBounds;
    float g = texture2D(image, newGUv).g * xGInBounds;
    float b = texture2D(image, newBUv).b * xBInBounds;

	vec4 col = vec4(r, g, b, 1.0) * colorDimFactor;

	col *= circle(uv, 0.1, 0.75);
		
	gl_FragColor = col;
}
