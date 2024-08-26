Shader "JJ/phong_tess_bump_perlin" {
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
        _alphaDepth("Alpha depth shadow", Float) = 0.1
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
            float _alphaDepth; 
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
                float3 position: INTERNALTESSPOS;
                float3 normal: TEXCOORD0;
                float3 level: TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct TessFactors{
                float edge[3]: SV_TessFactor;
                float inside: SV_InsideTessFactor;
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
                float3 pos = TransformObjectToWorld(input.position);

                TessAttributes output;
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                output.position = input.position;
                output.normal = normalize(input.normal);
                float level =  (1 - clamp(distance(pos, _WorldSpaceCameraPos.xyz) * _maxDistanceInv, 0, 1));
                level = level * level * (_maxLevel-_minLevel) + _minLevel; 
                //*(_maxLevel-_minLevel) + _minLevel;
                output.level = float3(level,0,0);                
                return output;
            }


            [domain("tri")]
            [outputcontrolpoints(3)]
            [outputtopology("triangle_cw")]
            [patchconstantfunc("PatchConstactFunction")]
            [partitioning("integer")]
            TessAttributes hull(InputPatch<TessAttributes, 3> patch, uint id: SV_OutputControlPointID)
            {
                return patch[id];
            }
            TessFactors PatchConstactFunction(InputPatch<TessAttributes, 3> patch) {
                TessFactors f;
                float l0 = patch[0].level.x;
                float l1 = patch[1].level.x;
                float l2 = patch[2].level.x;

                f.edge[0] = (l1+l2)*0.5;
                f.edge[1] = (l0+l2)*0.5;
                f.edge[2] = (l0+l1)*0.5;;
                f.inside =  (l0+l1+l2)*0.3333;
                return f;
            }

            [domain("tri")]
            FragmentAttributes domain(
                TessFactors factors, 
                OutputPatch<TessAttributes, 3> patch,
                float3 coords: SV_DomainLocation)
            {
                UNITY_SETUP_INSTANCE_ID(patch[0]);
                float3 p0 = patch[0].position;
                float3 p1 = patch[1].position;
                float3 p2 = patch[2].position;
                float3 p = p0 * coords.x + p1 * coords.y + p2 * coords.z;
                float3 n0 = patch[0].normal;
                float3 n1 = patch[1].normal;
                float3 n2 = patch[2].normal;
                float3 n = normalize(n0 * coords.x + n1 * coords.y + n2 * coords.z);


            
                float3 q0 = p - dot((p - p0), n0) * n0;
                float3 q1 = p - dot((p - p1), n1) * n1;
                float3 q2 = p - dot((p - p2), n2) * n2;
                float3 q =  q0 * coords.x + q1 * coords.y + q2 * coords.z;

                float3 position = _alphaTess * q + (1.0 - _alphaTess) * p;

                
                // float3 position = q * _alphaTess;
                
                float3 positionWS = TransformObjectToWorld(position);
                float3 normalWS = TransformObjectToWorldNormal(n);
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
                
                // float depth = 1 - ((input.positionCS.w) * 0.1);
                float3 L = GetCurrentViewPosition() - input.positionWS;
                float depth = (1.0 - clamp(length(L) * _depth, 0.0, 1.0)); 
                L = normalize(L);
                
                float3 N = normalize(input.normal);
                N = normalize(N+df*depth);

                depth = depth * (1-_alphaDepth) + _alphaDepth;
                float diffuse = pow(clamp(1 - dot(L, N), 0, 1), _pow) * (depth);
                // float diffuse = pow(clamp(1 - dot(L, N), 0, 1), _pow);
                float3 color =  _baseColor * (_alpha + diffuse * (1 - _alpha));   

                return float4(color, 1);
                
                // return float4(1- ((input.positionCS.w) *0.1), 0, 0, 1);

                // return float4(color, 1) *  ( 0.5 + 0.5 * noise(input.position, _scale2));
            }


            ENDHLSL




        }
    }
}