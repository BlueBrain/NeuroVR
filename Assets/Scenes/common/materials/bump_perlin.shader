Shader "JJ/bump_perlin" {
    Properties{
        [Header(Surface options)]
        [MainColor] _baseColor("Base", Color) = (1,0,0,1)
        _alpha("Alpha", Float) = 0.1 
        _scale("Scale", Float) = 1.0
        _height("Height", Float) = 1.0
        _speed("Speed", Float) = 0.0


    }

    SubShader{
        Tags{"RenderPipeline" = "UniversalPipeline"}

        Pass{
            Name "ForwardLit"
            Tags{"LightMode" = "UniversalForward"}

            HLSLPROGRAM 

            #define _SPECULAR_COLOR

            #pragma vertex vert
            #pragma fragment frag
            

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            float4 _baseColor;
            float _alpha;               
            float _scale;    
            float _height;  
            float _speed; 
    


            struct VertexAttributes
            {
                float3 position: POSITION;
                float3 normal: NORMAL;
            };

            struct FragmentAttributes
            {
                float4 positionCS: SV_POSITION;
                float3 position: TEXCOORD0;
                float3 positionWS: TEXCOORD1;
                float3 normal: TEXCOORD2;
            };

            FragmentAttributes vert(VertexAttributes input)
            {
                FragmentAttributes output;
                output.positionCS = TransformObjectToHClip(input.position);
                output.position = input.position;
                output.positionWS = TransformObjectToWorld(input.position);
                output.normal = TransformObjectToWorldNormal(input.normal);
                return output;
            }

            float hash(float3 p)  // replace this by something better
            {
                p  = frac( p*0.3183099+.1 );
                p *= 17.0;
                return frac( p.x*p.y*p.z*(p.x+p.y+p.z) );
            }

            float noise( float3 x )
            {
                x *= _scale;
                int3 i = int3(floor(x));
                float3 f = frac(x);
                f = f*f*f*(10-15*f+6*f*f);
                // f = f*f*(3.0-2.0*f);
                
                return lerp(lerp(lerp( hash(i+int3(0,0,0)), 
                                    hash(i+int3(1,0,0)),f.x),
                            lerp( hash(i+int3(0,1,0)), 
                                    hash(i+int3(1,1,0)),f.x),f.y),
                        lerp(lerp( hash(i+int3(0,0,1)), 
                                    hash(i+int3(1,0,1)),f.x),
                            lerp( hash(i+int3(0,1,1)), 
                                    hash(i+int3(1,1,1)),f.x),f.y),f.z) * _height;
            }



            float4 frag(FragmentAttributes input): SV_TARGET
            {                
                float t = _Time* _speed;
                float3 p = input.position + float3(t, t, t);

                float e = 0.00001;
                float f0 = noise(p);
                float fx = noise(p + float3(e, 0, 0));
                float fy = noise(p + float3(0, e, 0));
                float fz = noise(p + float3(0, 0, e));
                float3 df = float3(fx-f0, fy-f0, fz-f0) / e;                
                
                
                // float3 L = _MainLightPosition.xyz - input.positionWS;
                
                float3 L = normalize(GetCurrentViewPosition() - input.positionWS);
                float3 N = normalize(input.normal);
                N = normalize(N+df);

                // float3 V = normalize(input.positionWS - GetCurrentViewPosition());
                // V = normalize(reflect(V, N));


                float diffuse = clamp(dot(L, N), 0, 1);
                // float3 specular =  float3(1,1,1) * pow(clamp(dot(L,V), 0, 1), _smoothness);
                float3 color =  _baseColor * (_alpha + diffuse * (1 - _alpha));   
                // float3 color = float3(f0, f0, f0);   


                return float4(color, 1);
            }


            ENDHLSL




        }
    }
}