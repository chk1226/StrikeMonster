Shader "Custom/Outline" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_TexOffset ("TexOffset", Range(0.001, 0.005)) = 0.0025
		_Threshold ("Threshold", Range(0.0, 0.3)) = 0.05
		
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
			
			 	
			 	
			float3 AddColor(float2 st, int index)
			{
				float2 st_offset = float2(_TexOffset, _TexOffset);
				float reduce = 0.5/index;
				
				if(tex2D(_MainTex, st + st_offset * index).a <= _Threshold || tex2D(_MainTex, st - st_offset * index).a <= _Threshold)
				{
					return  _Color.rgb * reduce;
				}
			
				st_offset.x *= -1;
				if(tex2D(_MainTex, st + st_offset * index).a <= _Threshold || tex2D(_MainTex, st - st_offset * index).a <= _Threshold)
				{
					return  _Color.rgb * reduce;
				}			
				
				st_offset.x = _TexOffset;
				st_offset.y = 0;
				if(tex2D(_MainTex, st + st_offset * index).a <= _Threshold || tex2D(_MainTex, st - st_offset * index).a <= _Threshold)
				{
					return  _Color.rgb * reduce;
				}
				
				st_offset.x = 0;
				st_offset.y = _TexOffset;
				if(tex2D(_MainTex, st + st_offset * index).a <= _Threshold || tex2D(_MainTex, st - st_offset * index).a <= _Threshold)
				{
					return  _Color.rgb * reduce;
				}
				
				return float3(0, 0, 0);
			}

			fixed4 frag (v2f i) : SV_Target
			{		
				float3 add_color = float3(0, 0, 0);	
				fixed4 texcol = tex2D(_MainTex, i.st);
				
				if(texcol.a <= _Threshold)
				{
					return texcol;
				}

				add_color += AddColor(i.st, 1);
				add_color += AddColor(i.st, 2);
				add_color += AddColor(i.st, 3);
				add_color += AddColor(i.st, 4);
				add_color += AddColor(i.st, 5);
				add_color += AddColor(i.st, 6);
				add_color += AddColor(i.st, 7);
				add_color += AddColor(i.st, 8);			
				texcol.rgb += add_color;
				
				return texcol;
				
			}
			
			
			ENDCG	
		}
	} 
	FallBack "Diffuse"
}
