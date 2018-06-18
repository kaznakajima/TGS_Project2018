Shader "Custom/Mirror_Change" {
	Properties{
		_MainTex("Texture", 2D) = "white"{}
	}
	SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 100

		CGPROGRAM
        #pragma surface surf Standard alpha:fade vertex:vert
        #pragma target 3.0

		struct Input {
			float2 uv_MainTex;
        };

	    sampler2D _MainTex;

		// 歪みの強さ
		float _distortionX;
		float _distortionY;
		float _distortionZ;

		// 透明度
		float alpha;

		// テクスチャを歪ませる
		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			// 歪ませる
			float ampX = 0.1 * sin(_Time * 500 * v.vertex.x + 150.0f);
			float ampY = 0.1 * sin(_Time * 150 * v.vertex.y + 150.0f);
			v.vertex.xyz = float3(v.vertex.x + ampX, v.vertex.y + ampY, v.vertex.z);
		}

		void surf(Input IN, inout SurfaceOutputStandard o) {
			fixed4  c = tex2D(_MainTex, IN.uv_MainTex);

			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
