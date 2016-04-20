Shader "Custom/Outline" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_TexOffset ("TexOffset", Range(0.05,0.1)) = 0.5
		_Threshold ("TexOffset", Range(0.0,0.3)) = 0.05
		
//		_Glossiness ("Smoothness", Range(0,1)) = 0.5
//		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType"="Transparent" "IgnoreProjector" = "True" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite  off
		LOD 200
		
		PASS{	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float3 _Color;
			float _TexOffset;
			float _Threshold;
			
			
//			float2 _Center;
//			float2 _Radius;
//			float _BaseAlpha;

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
			
			 	

			fixed4 frag (v2f i) : SV_Target
			{		
				float3 add_color = float3(0, 0, 0);	
				fixed4 texcol = tex2D(_MainTex, i.st);
				
				if(texcol.a <= _Threshold)
				{
					return texcol;
				}

				
				for(int k = 0; k < 2 ; k++)
				{
				
					float2 st_offset = float2(_TexOffset, _TexOffset);
					float reduce = pow(0.5, k);
				
					if(tex2D(_MainTex, i.st + st_offset * k).a <= _Threshold)
					{
						add_color +=  _Color * reduce;
						continue;
					}
				
					if(tex2D(_MainTex, i.st - st_offset * k).a <= _Threshold)
					{
						add_color +=  _Color * reduce;
						continue;
					}
				
					st_offset.x *= -1;
					if(tex2D(_MainTex, i.st + st_offset * k).a <= _Threshold)
					{
						add_color +=  _Color * reduce;
						continue;
					}
										
					if(tex2D(_MainTex, i.st - st_offset * k).a <= _Threshold)
					{
						add_color +=  _Color * reduce;
						continue;
					}
				}
				
				texcol.rgb += add_color;
				
				return texcol;
//				return float4(0, 0, 0, 1);
				
			}
			
			
			ENDCG	
		}
	} 
	FallBack "Diffuse"
}
