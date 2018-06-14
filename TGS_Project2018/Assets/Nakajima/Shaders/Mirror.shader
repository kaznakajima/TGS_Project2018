Shader "Custom/Mirror" {
	Properties{
		_MainTex("Texture", 2D) = "white"{}
	    alpha("alpha", Range(0, 1)) = 0
	}
	SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard alpha:fade
        #pragma target 3.0

		struct Input {
			float2 uv_MainTex;
        };

	    sampler2D _MainTex;
		float alpha;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

			o.Albedo = c.rgb;
			o.Alpha = alpha;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
