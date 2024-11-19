/*{
	"ISFVSN":"2.0",
	"TYPE":"IMAGE",
	"LABEL":"Voronoi Tile",
	"VSN":"1.0",
	"CREDIT":"John Einselen",
	"INPUTS":[
		{
			"NAME":"inputImage",
			"TYPE":"image",
			"LABEL":"Image"
		},
		{
			"NAME":"count",
			"TYPE":"float",
			"LABEL":"Count",
			"DEFAULT":32.0,
			"MIN":0.0,
			"MAX":128.0,
			"STEP":1.0
		},
		{
			"NAME":"amount",
			"TYPE":"float",
			"LABEL":"Amount",
			"DEFAULT":2.0,
			"MIN":0.0,
			"MAX":2.0,
			"STEP":0.1
		}
	]
}*/

vec2 loopUV(vec2 uv) {
//	return vec2(mod(uv.x, 1.0), mod(uv.y, 1.0));
	return vec2(fract(uv.x), fract(uv.y));
}

void main()	{
	// Limit count to whole numbers
	float num = floor(count);
	
	// Calculate pixel offset
	float off = 1.0 / num;
	
	// Get UV map
	vec2 uvMap = isf_FragNormCoord.xy;
	
	// Pixelate UV map
	vec2 uvPix = uvMap; // Get 0.0-1.0 range map
	uvPix = floor(uvPix * num); // Convert to pixelated space
	uvPix += 0.5; // Sample from the centre of the pixelated area
	uvPix /= num; // Convert back to 0.0-1.0 range
	
	// Create Buffer
	float mask, len = 10.0;
	vec2 uvOff, noiseVal, imgVal;
	
	// Sample Centre
	uvOff = uvPix;
	noiseVal = IMG_NORM_PIXEL(inputImage, loopUV(uvOff)).xy;
	uvOff = uvOff + ((noiseVal - 0.5) * off * amount);
	len = length(uvMap - uvOff) * num;
	imgVal = uvOff;
	mask = len;
	
	// Sample Top Left
	uvOff = uvPix + vec2(-off, off);
	noiseVal = IMG_NORM_PIXEL(inputImage, loopUV(uvOff)).xy;
	uvOff = uvOff + ((noiseVal - 0.5) * off * amount);
	len = length(uvMap - uvOff) * num;
	imgVal = (len < mask)? uvOff: imgVal;
	mask = min(mask, len);
	
	// Sample Top
	uvOff = uvPix + vec2(0.0, off);
	noiseVal = IMG_NORM_PIXEL(inputImage, loopUV(uvOff)).xy;
	uvOff = uvOff + ((noiseVal - 0.5) * off * amount);
	len = length(uvMap - uvOff) * num;
	imgVal = (len < mask)? uvOff: imgVal;
	mask = min(mask, len);
	
	// Sample Top Right
	uvOff = uvPix + vec2(off, off);
	noiseVal = IMG_NORM_PIXEL(inputImage, loopUV(uvOff)).xy;
	uvOff = uvOff + ((noiseVal - 0.5) * off * amount);
	len = length(uvMap - uvOff) * num;
	imgVal = (len < mask)? uvOff: imgVal;
	mask = min(mask, len);
	
	// Sample Left
	uvOff = uvPix + vec2(-off, 0.0);
	noiseVal = IMG_NORM_PIXEL(inputImage, loopUV(uvOff)).xy;
	uvOff = uvOff + ((noiseVal - 0.5) * off * amount);
	len = length(uvMap - uvOff) * num;
	imgVal = (len < mask)? uvOff: imgVal;
	mask = min(mask, len);
	
	// Sample Right
	uvOff = uvPix + vec2(off, 0.0);
	noiseVal = IMG_NORM_PIXEL(inputImage, loopUV(uvOff)).xy;
	uvOff = uvOff + ((noiseVal - 0.5) * off * amount);
	len = length(uvMap - uvOff) * num;
	imgVal = (len < mask)? uvOff: imgVal;
	mask = min(mask, len);
	
	// Sample Bottom Left
	uvOff = uvPix + vec2(-off, -off);
	noiseVal = IMG_NORM_PIXEL(inputImage, loopUV(uvOff)).xy;
	uvOff = uvOff + ((noiseVal - 0.5) * off * amount);
	len = length(uvMap - uvOff) * num;
	imgVal = (len < mask)? uvOff: imgVal;
	mask = min(mask, len);
	
	// Sample Bottom
	uvOff = uvPix + vec2(0.0, -off);
	noiseVal = IMG_NORM_PIXEL(inputImage, loopUV(uvOff)).xy;
	uvOff = uvOff + ((noiseVal - 0.5) * off * amount);
	len = length(uvMap - uvOff) * num;
	imgVal = (len < mask)? uvOff: imgVal;
	mask = min(mask, len);
	
	// Sample Bottom Right
	uvOff = uvPix + vec2(off, -off);
	noiseVal = IMG_NORM_PIXEL(inputImage, loopUV(uvOff)).xy;
	uvOff = uvOff + ((noiseVal - 0.5) * off * amount);
	len = length(uvMap - uvOff) * num;
	imgVal = (len < mask)? uvOff: imgVal;
	mask = min(mask, len);
	
	// Adjust output values
	imgVal = loopUV(imgVal);
	mask *= 0.75;
	
	gl_FragColor = vec4(imgVal.x, imgVal.y, mask, 1.0);
//	gl_FragColor = IMG_NORM_PIXEL(inputImage, loopUV(uvPix));
}
