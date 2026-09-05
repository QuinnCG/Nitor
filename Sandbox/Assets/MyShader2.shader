// Vertex
#version 460 core
#extension GL_ARB_bindless_texture : require

#include SpriteInstance

#inject (std430, binding = 0) SpriteInstance sprites

layout (location = 0) in vec2 aPos;
layout (location = 1) in vec2 aUV;

layout (location = 0) out vec2 vUV;
layout (location = 1) out vec4 vColor;
layout (location = 2) flat out uvec2 vTextureHandle;

layout(location = 0) uniform mat4 uCamera;

void main()
{
    SpriteInstance sprite = sprites[gl_InstanceID];

    vUV = mix(sprite.uvMinMax.xy, sprite.uvMinMax.zw, aUV);
    vColor = sprite.color;
    vTextureHandle = sprite.textureHandle;

    float c = cos(sprite.rotation);
    float s = sin(sprite.rotation);

    vec2 pos = aPos;
    pos *= sprite.size;
    pos = vec2((pos.x * c) - (pos.y * s), (pos.x * s) + (pos.y * c));
    pos += sprite.position;

    gl_Position = uCamera * vec4(pos, 0.0, 1.0);
}

// Fragment
#version 460 core
#extension GL_ARB_bindless_texture : require

layout (location = 0) in vec2 vUV;
layout (location = 1) in vec4 vColor;
layout (location = 2) flat in uvec2 vTextureHandle;

layout (location = 0) out vec4 fColor;

layout(location = 1) uniform float uTime;

void main()
{
    sampler2D tex = sampler2D(vTextureHandle);
    fColor = texture(tex, vUV) * vColor * vec4(sin(uTime), 1.0, 1.0, 1.0);
}
