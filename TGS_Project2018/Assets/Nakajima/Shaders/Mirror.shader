Shader "Custom/Mirror" {
	Properties{
		_MainTex("Texture", 2D) = "white"{}
	}
	SubShader{
		Tags{"Queue" = "Transparent"}
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard alpha:fade
        #pragma target 3.0

		struct Input {
			float3 worldNormal;
			float3 viewDir;
			float2 uv_MainTex;
        };

	    sampler2D _MainTex;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
