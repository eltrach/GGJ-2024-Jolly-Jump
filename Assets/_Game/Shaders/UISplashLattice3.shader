Shader "Hidden/SplashLattice3"
{
   Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _ColorMask ("Color Mask", Float) = 15
        _M ("M", Float) = 3
        _R ("R", Float) = 0.5
        _Color2 ("Color2", Color) = (1,1,1,1)

    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
        Name "UISplash"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #define PI 3.14159265359

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _M;
            float _R;
            fixed4 _Color2;

            float rand(float2 st)
            {
                return frac(sin(dot(st, float2(12.9898, 78.233))) * 43758.5453);
            }

            float2 rotate(float2 st, float angle)
            {
                float2x2 mat = float2x2(cos(angle), -sin(angle),
                                        sin(angle), cos(angle));
                st -= 0.5;
                st = mul(mat, st);
                st += 0.5;
                return st;
            }

            float box(float2 st, float t)
            {
                st = rotate(st, t * 2.05 * PI / 4);
                float size = t * 1.42;
                st = step(size, st) * step(size, 1.0 - st);
                return st.x * st.y;
            }

            float lattice(float2 st, float n,float time)
            {
                float2 ist = floor(st * n);
                float freq = 1.5 * length(0.5 - (ist + 0.5) / n);
                float t = sin(rand(ist) * 0.8 - time + freq) * 0.5;
                return box(frac(st * n), t);
            }

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.color = v.color;
                return OUT;
            }

            float4 frag(v2f IN) : SV_Target
            {
                float t = (IN.color.a*_M + _R);
                IN.texcoord = rotate(IN.texcoord, PI / 4);

                float l1 = lattice(IN.texcoord, 7.5,t);
                float l2 = lattice(IN.texcoord, 15,t);

                half4 color ;

                if(l1 == 0 && l2 == 0){
                    half4 iMcolor = (tex2D(_MainTex, IN.texcoord*4))*IN.color;
                    iMcolor.a = 1;
                    color =  iMcolor;
                }else if(l1 == 1 && l2 == 1){
                    return float4( 0, 0, 0, 0);
                }
                else if(l1 == 1){
                    return _Color2;
                }else{
                    return ( 1, 1, 1, 1);
                }
                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                return color;
            }
        ENDCG
        }
    }
}
