Shader"Custom/FaceColourShader"
{
    Properties
    {
        _ColorTop ("Color Top (+Y)", Color) = (1,0,0,1)    // Red
        _ColorBottom ("Color Bottom (-Y)", Color) = (0,1,0,1) // Green
        _ColorLeft ("Color Left (-X)", Color) = (0,0,1,1)  // Blue
        _ColorRight ("Color Right (+X)", Color) = (1,1,0,1)    // Yellow
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

struct appdata_t
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
};

struct v2f
{
    float4 pos : SV_POSITION;
    float3 normal : TEXCOORD0;
};

fixed4 _ColorTop;
fixed4 _ColorBottom;
fixed4 _ColorLeft;
fixed4 _ColorRight;

v2f vert(appdata_t v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);
                // Keep the normals in object space (no rotation effect)
    o.normal = v.normal;
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
                // Ignore Front & Back (Z faces)
    if (abs(i.normal.z) < 0.5)
    {
        if (i.normal.y > 0.5)
            return _ColorTop; // Top (+Y)
        if (i.normal.y < -0.5)
            return _ColorBottom; // Bottom (-Y)
        if (i.normal.x > 0.5)
            return _ColorRight; // Right (+X)
        if (i.normal.x < -0.5)
            return _ColorLeft; // Left (-X)
    }

    return fixed4(1, 1, 1, 1); // Default White (Front & Back)
}
            ENDCG
        }
    }
}
