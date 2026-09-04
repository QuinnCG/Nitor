// Vertex
#version 460 core

layout (location = 0) in vec2 aPos;
layout (location = 1) in vec2 aUV;

layout (location = 0) out vec2 vUV;

layout(location = 0) uniform mat4 uCamera;

void main()
{
    vUV = aUV;
    gl_Position = uCamera * vec4(aPos, 0.0, 1.0);
}

// Fragment
#version 460 core

layout (location = 0) in vec2 vUV;
layout (location = 0) out vec4 fColor;

layout(location = 1) uniform float uTime;
layout(binding = 0) uniform sampler2D tex;

void main()
{
    fColor = vec4(1.0, 0.0, (sin(uTime) + 1.0) / 2.0, 1.0) * texture(tex, vUV);
}
