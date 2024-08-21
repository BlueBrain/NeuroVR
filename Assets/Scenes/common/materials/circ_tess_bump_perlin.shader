Shader "JJ/circ_tess_bump_perlin" {
    Properties{
        [Header(Surface options)]
        [MainColor] _baseColor("Base", Color) = (1,0,0,1)
        _alpha("Alpha", Float) = 0.1 
        _scale0("Scale 0", Float) = 1.0
        _height0("Height 0", Float) = 1.0
        _scale1("Scale 1", Float) = 2.0
        _height1("Height 1", Float) = 1.0
        _minLevel("Min level", Float) = 2.0
        _maxLevel("Max level", Float) = 10.0
        _maxDistanceInv("Max distance", Float) = 0.001
        _depth("Depth shadow", Float) = 0.05
        _alphaTess("Alpha tess", Float) = 1.0        
        _pow("Pow", Float) = 2.0
        _speed("Speed", Float) = 0.0






    }

    SubShader{
        Tags{"RenderPipeline" = "UniversalPipeline"}

        Pass{
            Name "ForwardLit"
            Tags{"LightMode" = "UniversalForward"}

            HLSLPROGRAM 

            #pragma vertex vert
            #pragma hull hull
            #pragma domain domain
            #pragma fragment frag
            

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"         

            float4 _baseColor;
            float _alpha;               
            float _scale0;    
            float _height0;             
            float _scale1;    
            float _height1;   
            float _minLevel;
            float _maxLevel;
            float _maxDistanceInv;         
            float _depth;
            float _alphaTess; 
            float _pow;   
            float _speed;  
                     
  



            struct VertexAttributes
            {
                float3 position: POSITION;
                float3 normal: NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct TessAttributes
            {
                float3 center: INTERNALTESSPOS;
                float3 normal: TEXCOORD0;
                float3 levelRadius: TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct TessFactors{
                float edge[4]: SV_TessFactor;
                float inside[2]: SV_InsideTessFactor;
            };
            struct FragmentAttributes
            {
                float4 positionCS: SV_POSITION;
                float3 positionWS: TEXCOORD0;
                float3 position: TEXCOORD1;
                float3 normal: TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TessAttributes vert(VertexAttributes input)
            {
                UNITY_SETUP_INSTANCE_ID(input);
                float3 center = input.position - input.normal;
                float radius = length(input.normal);
                

                float3 pos = TransformObjectToWorld(center);
                float level =  (1 - clamp(distance(pos, _WorldSpaceCameraPos.xyz) * _maxDistanceInv, 0, 1));
                level = level * level * (_maxLevel-_minLevel) + _minLevel; 
                
                TessAttributes output;
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                output.center = center;
                output.normal = normalize(input.normal);
                output.levelRadius = float3(level,radius,0);                
                return output;
            }


            [domain("quad")]
            [outputcontrolpoints(4)]
            [outputtopology("triangle_cw")]
            [patchconstantfunc("PatchConstactFunction")]
            [partitioning("integer")]
            TessAttributes hull(InputPatch<TessAttributes, 4> patch, uint id: SV_OutputControlPointID)
            {
                return patch[id];
            }
            TessFactors PatchConstactFunction(InputPatch<TessAttributes, 4> patch) {
                TessFactors f;
                float l0 = patch[0].levelRadius.x;
                float l1 = patch[2].levelRadius.x;
                float l = (l0+l1)*0.5;

                f.edge[0] = l;
                f.edge[1] = l1;
                f.edge[2] = l;
                f.edge[3] = l0;
                f.inside[0] = l;
                f.inside[1] = l;
                return f;
            }

            [domain("quad")]
            FragmentAttributes domain(
                TessFactors factors, 
                OutputPatch<TessAttributes, 4> patch,
                float2 uv: SV_DomainLocation)
            {
                UNITY_SETUP_INSTANCE_ID(patch[0]);
                float3 center = lerp(patch[2].center, patch[0].center, uv.y);

                float3 n0 = lerp(patch[1].normal, patch[0].normal, uv.x);          
                float3 normal = normalize(lerp(patch[2].normal, n0, uv.y));
                float radius = lerp(patch[2].levelRadius.y, patch[0].levelRadius.y, uv.y);    

                
                float3 position =  center + normal * radius;
                
                
                float3 positionWS = TransformObjectToWorld(position);
                float3 normalWS = TransformObjectToWorldNormal(normal);
                FragmentAttributes output;
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                output.positionCS = TransformWorldToHClip(positionWS);
                output.positionWS = positionWS;
                output.position = position;
                output.normal = normalWS;
                return output;
            }


            float hash(float3 p)  
            {
                p  = frac( p*0.3183099+.1 );
                p *= 17.0;
                return frac( p.x*p.y*p.z*(p.x+p.y+p.z) );
            }

            float noise( float3 x, float scale)
            {
                x *= scale;
                int3 i = int3(floor(x));
                float3 f = frac(x);
                f = f*f*f*(10-15*f+6*f*f);                
                return lerp(lerp(lerp( hash(i+int3(0,0,0)), 
                                    hash(i+int3(1,0,0)),f.x),
                            lerp( hash(i+int3(0,1,0)), 
                                    hash(i+int3(1,1,0)),f.x),f.y),
                        lerp(lerp( hash(i+int3(0,0,1)), 
                                    hash(i+int3(1,0,1)),f.x),
                            lerp( hash(i+int3(0,1,1)), 
                                    hash(i+int3(1,1,1)),f.x),f.y),f.z);
            }

            float layersNoise(float3 x)
            {
                // return noise(x, _scale0) * _height0 + noise(x, _scale1) * _height1 + noise(x, _scale2) * _height2;
                float t = _Time* _speed;
                return noise(x + float3(t, t, t), _scale0) * _height0 + noise(x , _scale1) * _height1;
            
            }

            float4 frag(FragmentAttributes input): SV_TARGET
            {                
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float e = 0.00001;
                float f0 = layersNoise(input.position);
                float fx = layersNoise(input.position + float3(e, 0, 0));
                float fy = layersNoise(input.position + float3(0, e, 0));
                float fz = layersNoise(input.position + float3(0, 0, e));
                float3 df = float3(fx-f0, fy-f0, fz-f0) / e ;                
                
                float3 L = GetCurrentViewPosition() - input.positionWS;
                float depth = (1.0 - clamp(length(L) * _depth, 0.0, 1.0)); 
                L = normalize(L);
                float3 N = normalize(input.normal);
                N = normalize(N+df*depth);

                float diffuse = pow(clamp(1 - dot(L, N), 0, 1), _pow) * depth;
                float3 color =  _baseColor * (_alpha + diffuse * (1 - _alpha));   

                return float4(color, 1);
                
                // return float4(1- ((input.positionCS.w) *0.1), 0, 0, 1);

                // return float4(color, 1) *  ( 0.5 + 0.5 * noise(input.position, _scale2));
            }


            ENDHLSL




        }
    }
}