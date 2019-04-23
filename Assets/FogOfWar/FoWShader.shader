Shader "Custom/FoWShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Albedo Texture", 2D) = "white" {}
		_Transparency("Transparency", Range(0.0, 1.0)) = 0.25
		_CutoutThresh("Cutout Threshold", Range(0.0, 1.0)) = 0.2
		_Distance("Distance", Float) = 1
		_Amplitude("Amplitude", Float) = 1
		_Speed("Speed", Float) = 1
		_Amount("Amount", Range(0.0, 1.0)) = 1
	}

	SubShader
	{
		Tags {"Queue"="Transparent" "RenderType"="Transparent" "LightMode"="ForwardBase"}
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite off
		LOD 200
			
		CGPROGRAM
		#pragma surface surf NoLighting noambient alpha:fade

		sampler2D _MainTex;
		fixed4 _Color;

		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, float aten) {
			fixed4 color;
			color.rgb = s.Albedo;
			color.a = s.Alpha;
			return color;
		}

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
			half4 baseColor = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = _Color.rgb * baseColor.b;
			o.Alpha = _Color.a - baseColor.g;
        }
        ENDCG
    }
    FallBack "Transparent/Diffuse"
}
