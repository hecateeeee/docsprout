﻿Shader "Custom/doods" {
	Properties {
		_Color ("Happy Color", Color) = (1,1,1,1)
        _SickColor ("Sick Color", Color) = (1,1,1,1)
        _Happiness ("Happiness", Range(0, 1)) = 1
        _LightMultiplier ("Ramp Multiplier", Float) = 1
        
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Cel fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};
        
        float _LightMultiplier;
        fixed4 _SickColor;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
            UNITY_DEFINE_INSTANCED_PROP(fixed, _Happiness)
		UNITY_INSTANCING_BUFFER_END(Props)

        half4 LightingCel (SurfaceOutput s, half3 lightDir, half atten) {
            half NdotL = dot (s.Normal, lightDir);
            NdotL = min(1, max(0, NdotL)*20)*_LightMultiplier;
            half4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten);
            c.a = s.Alpha;
            return c;
        }

		void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
            fixed h = UNITY_ACCESS_INSTANCED_PROP(Props, _Happiness);
			o.Albedo = lerp(_SickColor.rgb, c.rgb, h);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
