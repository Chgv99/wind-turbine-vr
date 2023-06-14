Shader "Hidden/AtlasCreator_BuilderCombine"
{
	Properties
	{
		_MainTex("Base (RGBA)", 2D) = "" {}
		_AddTex("Add (RGBA)", 2D) = "" {}
	}

	CGINCLUDE

	#include "UnityCG.cginc"

	struct v2f
	{
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
	};

	sampler2D _MainTex;
	sampler2D _AddTex;
	uniform float4 _AddTexRect;

	v2f vert(appdata_img v)
	{
		v2f o = (v2f)0;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 r = tex2D(_MainTex, i.uv);
		i.uv.y = 1 - i.uv.y;

		if (i.uv.x < _AddTexRect.x || i.uv.y < _AddTexRect.y || i.uv.x > _AddTexRect.x + _AddTexRect.z
			|| i.uv.y > _AddTexRect.y + _AddTexRect.w)
			return r;

		fixed4 r_add = tex2D(_AddTex, (i.uv - _AddTexRect.xy) / (_AddTexRect.zw + 0.000000001));

		r.rgba = lerp(r.rgba, r_add.rgba, r_add.a);
		return r;
	}

	ENDCG

	Subshader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}

	Fallback off
}
