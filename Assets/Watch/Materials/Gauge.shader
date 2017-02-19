Shader "Unlit/NewUnlitShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "red" {}
		_Percent("Percent", Range(0, 1)) = 0.5
	}
	
	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }

		Lighting Off
		Fog { Mode Off }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			#define M_PI 3.1415926535897932384626433832795

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Percent;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, float2(_Percent, 0));

				float2 recentered = float2((i.uv.x * 2) - 1, (i.uv.y * 2) - 1);
				
				float radius = length(recentered);
				float angle = atan2(recentered.x, -recentered.y);

				if ((radius > 0.9) || (radius < 0.8) || ((angle + M_PI) / (2 * M_PI) < (1 - _Percent)))
					col.a = 0;
				
				
				return col;
			}
			ENDCG
		}
	}
}
