Shader "Custom/GreyScaleShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // �e�N�X�`������F���T���v�����O
                fixed4 col = tex2D(_MainTex, i.uv);

            // RGB�̕��ϒl���Ƃ��ăO���[�X�P�[����
            float gray = dot(col.rgb, float3(0.3, 0.59, 0.11));

            // �O���[�X�P�[���̐F���o��
            return fixed4(gray, gray, gray, col.a);
            }
        ENDCG
        }
    }
        FallBack "Diffuse"
}
