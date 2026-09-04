// Vertex
#version 460 core

layout (location = 0) in vec2 aPos;

uniform mat4 uCamera;

void main()
{
    gl_Position = uCamera * vec4(aPos, 0.0, 1.0);
}

// Fragment
#version 460 core

layout (location = 0) out vec4 fColor;

uniform float uTime;

void main()
{
    fColor = vec4(1.0, 0.0, (sin(uTime) + 1.0) / 2.0, 1.0);
}
