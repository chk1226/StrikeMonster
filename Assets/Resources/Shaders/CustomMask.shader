Shader "Custom/CustomMask" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Center("Center", Vector) = (0, 0, 0, 0)				// [0,1]
		_Radius("Radius", Vector) = (0.5, 0.5, 0, 0)			// [0,1]
		_BaseAlpha("BaseAlpha", Range(0, 1)) = 0.5
		_AddColor("AddColor", Color) = (0.0, 0.0, 0.0, 0.5)
	}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType"="Transparent" "IgnoreProjector" = "True" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite  off
//		LOD 200
		
		PASS{	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float2 _Center;
			float2 _Radius;
			float _BaseAlpha;
			float3 _AddColor;

			struct v2f {
				float4 pos : SV_POSITION;
				float2 st : TEXCOORD0;
			};

			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
//				o.st = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.st = v.texcoord;
				return o;
			}
			
			float InRangeValue(float2 pos)
			{
				return pow(pos.x - _Center.x, 2)/pow(_Radius.x, 2)  + pow(pos.y - _Center.y, 2)/pow(_Radius.y, 2);
			}
			
			float SmoothAlpha(float value)
			{
				return smoothstep(0, 1, value) * _BaseAlpha;
			}
			

			fixed4 frag (v2f i) : SV_Target
			{		
				float3 p_color = float3(0, 0, 0);	
				
				float inRangeValue = InRangeValue(i.st);
				if( inRangeValue <= 1 ) 
				{					
					return fixed4(p_color + _AddColor , SmoothAlpha(inRangeValue));	
				}		
				else
				{
					return fixed4 (p_color, _BaseAlpha);
				}
			}
			
			
			ENDCG	
		}
	} 
	FallBack "Diffuse"
}
