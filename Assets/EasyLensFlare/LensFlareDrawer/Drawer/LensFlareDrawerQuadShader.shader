Shader "Hidden/LensFlareDrawerQuadShader"
{
    Properties
    {
		_Color("Main Color", Color) = (0,0,0,0)
        _MainTex ("Main Texture", 2D) = "white" {}
    }

	CGINCLUDE
	#include "UnityCG.cginc"
	struct v2f
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float3 color : COLOR;
	};

	v2f vert(appdata_full v)
	{
		v2f o = (v2f)0;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord;
		o.color = v.color;
		return o;
	}

	sampler2D _MainTex;
	half4 _Color;

	half4 frag(v2f i) : COLOR
	{
		half4 result = tex2D(_MainTex, i.uv) * i.color.r * _Color;
		if (result.a < 0.000001)
			discard;
		return result;
	}
	ENDCG

    SubShader
    {
        CULL Off
        ZWrite Off
        tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }
        Blend One One

		Pass
		{
			ZTest LEqual

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}

		Pass
		{
			ZTest greater

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
    }
}
