Shader "Unlit/Icon" {
	Properties{
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	    alpha("apha", Range(0, 1)) = 0
	}

	SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{
		Lighting Off
		SetTexture[_MainTex]{ combine texture }
	    }

		CGPROGRAM
#pragma surface surf Standard alpha:fade
#pragma target 3.0

		struct Input {
		       float2 uv_MainTex;
	    };

	sampler2D _MainTex;
	// 透明度
	float alpha;

	void surf(Input IN, inout SurfaceOutputStandard o) {
		fixed4  c = tex2D(_MainTex, IN.uv_MainTex);

		     o.Albedo = fixed4(c.r, c.g, c.b, alpha);
			 o.Alpha = alpha;
	     }
	ENDCG

	}
}