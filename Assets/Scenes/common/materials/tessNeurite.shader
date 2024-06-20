Shader "JJ/tessNeuriteQuads" 
{
    Properties{
        [Header(Surface options)]
        _noise("Noise", 2D) = "white" {}
        [PerMaterialData] _color1("Color1", Color) = (0.0, 0.5, 1.0, 1.0)
	    [PerMaterialData] _color2("Color2", Color) = (0.5, 1.0, 1.0, 1.0)
        [PerMaterialData] _scale("Scale", Float) = 2.0
        _minLevel("Min Level", Float) = 2
        _maxLevel("Max Level", Float) = 10
        _maxDistanceInv("Max distance", Float) = 0.01
    }
    SubShader{
        Pass{
            Name "ForwardLit"
            
            HLSLPROGRAM 

            #pragma vertex vert
            #pragma hull hull
            #pragma domain domain
            // #pragma geometry geom
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"         
            
            CBUFFER_START(UnityPerMaterial)
                TEXTURE2D(_noise);
                SAMPLER(sampler_noise);

                half4 _color1;
                half4 _color2;
                float _scale;
  
                float _minLevel;
                float _maxLevel;
                float _maxDistanceInv;
            CBUFFER_END

            struct VertAttributes
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
            struct FragAttributes
            {
                float4 positionCS: SV_POSITION;
                float3 positionWS: TEXCOORD0;
                float3 position: TEXCOORD1;
                float3 normal: TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };


            // Vertex shader
            TessAttributes vert(VertAttributes input)
            {
                UNITY_SETUP_INSTANCE_ID(input);
                float3 center = TransformObjectToWorld(input.position - input.normal);
                float3 pos = TransformObjectToWorld(input.position);
                float3 normal = TransformObjectToWorldNormal(input.normal);
                float radius = distance(center, pos);
                normal = normalize(normal);
                // float minLevel = _minLevel * max(radius*4,1);
                
                float minLevel = _minLevel;
                float maxLevel = _maxLevel;

                float level =  (1 - clamp((distance(center, _WorldSpaceCameraPos.xyz)/ radius) * _maxDistanceInv, 0, 1))* (maxLevel-minLevel) + minLevel;

                TessAttributes output;
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                output.center = center;
                output.normal = normal;
                output.levelRadius = float3(level,radius,0);

                return output;
            }    

            // Tess control
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

                // f.edge[0] = 1;
                // f.edge[1] = 1;
                // f.edge[2] = 1;
                // f.edge[3] = 1;
                // f.inside[0] = 0;
                // f.inside[1] = 0;

                return f;
            }
            [domain("quad")]
            [partitioning("integer")]
            [outputtopology("triangle_cw")]
            [outputcontrolpoints(4)]
            [patchconstantfunc("PatchConstactFunction")]
            TessAttributes hull(InputPatch<TessAttributes, 4> patch, uint id: SV_OutputControlPointID)
            {
                return patch[id];
            }
            


            // Tess Eval
            [domain("quad")]
            FragAttributes domain(
                TessFactors factors, 
                OutputPatch<TessAttributes, 4> patch,
                float2 uv: SV_DomainLocation)
            {
                UNITY_SETUP_INSTANCE_ID(patch[0]);
                // float3 c0 = lerp(patch[1].center, patch[0].center, uv.x);
                // float3 c1 = lerp(patch[3].center, patch[2].center, uv.x);
                float3 center = lerp(patch[2].center, patch[0].center, uv.y);

                float3 n0 = lerp(patch[1].normal, patch[0].normal, uv.x);          
                float3 normal = normalize(lerp(patch[2].normal, n0, uv.y));
                
                // float3 n0 = lerp(patch[1].normal, patch[0].normal, uv.x);          
                // float3 n1 = lerp(patch[2].normal, patch[3].normal, uv.x);
                // float3 normal = normalize(lerp(n1, n0, uv.y));
                
                float radius = lerp(patch[2].levelRadius.y, patch[0].levelRadius.y, uv.y);    
                
                // float3 pos = center;
                float3 pos = center + normal * radius;
                float3 position = TransformWorldToObject(pos);

                FragAttributes output;
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                output.positionCS = TransformWorldToHClip(pos);
                output.positionWS = pos;
                output.position = position;
                output.normal = normal;
                return output;
            }


            // Fragment shader
            float4 frag(FragAttributes input): SV_TARGET
            {                
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
               
                float2 uvXY = _scale * float2(input.position.x, input.position.y);
				half noiseXY = SAMPLE_TEXTURE2D_LOD(_noise, sampler_noise, uvXY, 0);

				float2 uvYZ = _scale * float2(input.position.y, input.position.z);
				half noiseYZ = SAMPLE_TEXTURE2D_LOD(_noise, sampler_noise, uvYZ, 0);

				float noiseValue = sqrt(noiseXY * noiseYZ);

				half4 final = _color1 * noiseValue + (1 -  noiseValue) * _color2;

				float3 direction = normalize(input.positionWS.xyz - _WorldSpaceCameraPos);

				half4 ratio = clamp(-dot(direction, input.normal), 0, 1);

				final = final * ratio + (1 - ratio) * _color2;
                return final;
            }


            ENDHLSL




        }
    }
}