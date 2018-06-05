Shader "Unlit/Player" {
	Properties{
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	    _SubTex("Sub (RGB) Trans (A)", 2D) = "white" {}
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
	}
}