Shader "Unlit/MainMenuWave"
{
    Properties
    {
        _ColorBack("Background Color", Color) = (0.91,0.91,0.91,1)
        _Color1("Color 1", Color) = (1,1,1,1)
        _Mask1Parms("X:Amplitude Y:Offset Z:Velocity W:Wavelength", Vector) = (0.2,-0.6,1,0.8)
        _Color2("Color 2", Color) = (1,1,1,1)
        _Mask2Parms("X:Amplitude Y:Offset Z:Velocity W:Wavelength", Vector) = (0.2,-0.3,1,1)
        _Color3("Color 3", Color) = (1,1,1,1)
        _Mask3Parms("X:Amplitude Y:Offset Z:Velocity W:Wavelength", Vector) = (0.2,0,1,1)
        _Color4("Color 4", Color) = (1,1,1,1)
        _Mask4Parms("X:Amplitude Y:Offset Z:Velocity W:Wavelength", Vector) = (0.2,0.3,1,1)
        _SmoothEdge("smoothstep clarity", range(0,0.2))=0.01
    }
    SubShader
    {
        Tags {"Queue" = "Transparent"  "IgnoreProjection" = "True" "RenderType" = "Transparent"}
        LOD 100

        Pass
        {
            ZWrite Off // 关闭深度写入
            Blend SrcAlpha OneMinusSrcAlpha // 混合的参数
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed3 _ColorBack, _Color1, _Color2, _Color3, _Color4;
            float _SmoothEdge;
            float4 _Mask1Parms, _Mask2Parms, _Mask3Parms, _Mask4Parms;

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float sine(float2 p, float4 parms)
            {
                return (p.x + sin((p.y * parms.w + _Time.y * parms.z)) * parms.x) + parms.y;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 finalCol = _ColorBack;
                float alpha = 1;
                float2 uv = i.uv*2-1;
                // float2 uv = (2.0 * i.vertex.xy -  _ScreenParams.xy) /  _ScreenParams.y;

                float mask4 = smoothstep(0,_SmoothEdge,sine(uv,_Mask4Parms));
                alpha = max(alpha,mask4);
                finalCol = lerp(finalCol,_Color4,mask4);

                float mask3 = smoothstep(0,_SmoothEdge,sine(uv,_Mask3Parms));
                alpha = max(alpha,mask3);
                finalCol = lerp(finalCol,_Color3,mask3);

                float mask2 = smoothstep(0,_SmoothEdge,sine(uv,_Mask2Parms));
                alpha = max(alpha,mask2);
                finalCol = lerp(finalCol,_Color2,mask2);

                float mask1 = smoothstep(0,_SmoothEdge,sine(uv,_Mask1Parms));
                alpha = max(alpha,mask1);
                finalCol = lerp(finalCol,_Color1,mask1);

                // pow4
                // alpha *= alpha;
                // alpha *= alpha;
                return fixed4(finalCol,alpha);
            }
            ENDCG
        }
    }
}
