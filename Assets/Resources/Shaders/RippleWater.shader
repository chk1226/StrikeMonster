Shader "Custom/RippleWater" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Speed ("Speed", float) = 4
		_Frequency ("Frequency", float) = 0.5
		_NormalDebug ("NormalDebug", Range(0,1)) = 0
		_Amplitude("Amplitude", float) = 0
		
//		_DebugWorldNormal("DebugWorldNormal", Vector) = (0, 0, 0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BlinnPhong vertex:vert

		sampler2D _MainTex;
		float _Amplitude, _Speed, _Frequency;
		fixed4 _Color;
		
		int _NormalDebug;
//		float3 _DebugWorldNormal;
		
		float _ContactX, _ContactZ;
		
		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 worldNormal;
			float3 _normal;
			INTERNAL_DATA
		};


		float3 expand(float3 v)
		{
			return (v-0.5)*2;
		}


		void vert (inout appdata_full v, out Input data)
		{
		    
			half r = sqrt( pow(v.vertex.x + _ContactX, 2) + pow( v.vertex.z + _ContactZ, 2));
			half k;
			if(r == 0)
			{
				k = 1;
			}
			else
			{
				k = r;
			}
			
			float value = _Amplitude * sin(_Time[1] * _Speed + r * _Frequency);
			
			v.vertex.y += value;
			
			// normal calculate referce by
			// http://http.developer.nvidia.com/GPUGems/gpugems_ch01.html
			data._normal.y = 1;
			data._normal.x = -1 * _Frequency * v.vertex.x * _Amplitude * cos(_Time[1] * _Speed + r * _Frequency) ;
			data._normal.z = -1 * _Frequency * v.vertex.z * _Amplitude * cos(_Time[1] * _Speed + r * _Frequency) ;

		}

		void surf (Input IN, inout SurfaceOutput o) {
			
			
			fixed4 c;
			float3 customNormal = UnityObjectToWorldNormal(IN._normal.xyz);
			if(_NormalDebug == 1)
			{
				c.xyz = customNormal.xyz;

			}
			else
			{
				c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			}
			
			// confused!!! why o.Normal.z is v-up
			o.Normal.x = customNormal.x;
			o.Normal.y = customNormal.z;
			o.Normal.z = customNormal.y;

			
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		
		ENDCG
	} 
	FallBack "Diffuse"
}
