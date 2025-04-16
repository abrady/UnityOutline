Shader "Custom/SingleObjectEdgeDetection"
{
    Properties
    {
        // This texture should be the render texture you set up from your isolated object.
        _MainTex ("Main Texture (Object Render)", 2D) = "white" {}
        // You can adjust this threshold to control edge sensitivity.
        _Threshold ("Edge Threshold", Range(0,1)) = 0.1
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Overlay" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
                        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _Threshold;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

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
                return o;
            }

            // This fragment shader applies a Sobel filter to the texture.
            fixed4 frag(v2f i) : SV_Target
            {
                // Sample a 3x3 grid around the current pixel.
                float3 sample[9];
                int index = 0;
                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        float2 offset = float2(x, y) * _MainTex_TexelSize.xy;
                        sample[index++] = tex2D(_MainTex, i.uv + offset).rgb;
                    }
                }
                
                // Sobel operator kernels for Gx and Gy.
                // Gx kernel: [ -1, 0, 1, -2, 0, 2, -1, 0, 1 ]
                // Gy kernel: [ -1, -2, -1,  0,  0,  0,  1,  2,  1 ]
                float3 gx = sample[2] + 2 * sample[5] + sample[8] - (sample[0] + 2 * sample[3] + sample[6]);
                float3 gy = sample[0] + 2 * sample[1] + sample[2] - (sample[6] + 2 * sample[7] + sample[8]);

                // Compute the gradient magnitude.
                float3 gradient = sqrt(gx * gx + gy * gy);
                float edgeIntensity = length(gradient);

                // If the edge intensity exceeds the threshold, output a solid color (for example, black).
                if(edgeIntensity > _Threshold)
                {
                    return fixed4(1, 0, 0, 1);
                }
                // Otherwise, output transparent (or another color if desired).
                return fixed4(0, 0, 0, 0);
                // return fixed4(0,1,0,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
