Shader "Unlit/CharacterShaderCustom"
{
    Properties
    {
        [NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        _ScrollSpeed("ScrollSpeed", Float) = 0.5
        _DisolveColor("DisolveColor", Color) = (0.9921569, 0.3878876, 0.2117647, 1)
        _ColorIntensity("ColorIntensity", Float) = 2
        _DisolveHeight("DisolveHeight", Range(0, 1)) = 0
        _DistortionStrength("DistortionStrength", Float) = 2
        _Intensity("BloomIntensity", Float) = 0
        _Color("BloomColor", Color) = (0, 0, 0, 0)
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
        
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }
    SubShader
    {
        
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
            // DisableBatching: <None>
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalSpriteUnlitSubTarget"
        }
        Pass
        {
            
            Name "Sprite Unlit"
            Tags
            {
                "LightMode" = "Universal2D"
            }
        ColorMask[_ColorMask]
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask 255
            WriteMask 255
        }
        // Render State
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITEUNLIT
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 color : INTERP1;
             float3 positionWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.color.xyzw = input.color;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.color = input.color.xyzw;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float _ScrollSpeed;
        float _DisolveHeight;
        float4 _DisolveColor;
        float _ColorIntensity;
        float _DistortionStrength;
        float _Intensity;
        float4 _Color;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Negate_float(float In, out float Out)
        {
            Out = -1 * In;
        }
        
        float Unity_SimpleNoise_ValueNoise_Deterministic_float (float2 uv)
        {
            float2 i = floor(uv);
            float2 f = frac(uv);
            f = f * f * (3.0 - 2.0 * f);
            uv = abs(frac(uv) - 0.5);
            float2 c0 = i + float2(0.0, 0.0);
            float2 c1 = i + float2(1.0, 0.0);
            float2 c2 = i + float2(0.0, 1.0);
            float2 c3 = i + float2(1.0, 1.0);
            float r0; Hash_Tchou_2_1_float(c0, r0);
            float r1; Hash_Tchou_2_1_float(c1, r1);
            float r2; Hash_Tchou_2_1_float(c2, r2);
            float r3; Hash_Tchou_2_1_float(c3, r3);
            float bottomOfGrid = lerp(r0, r1, f.x);
            float topOfGrid = lerp(r2, r3, f.x);
            float t = lerp(bottomOfGrid, topOfGrid, f.y);
            return t;
        }
        
        void Unity_SimpleNoise_Deterministic_float(float2 UV, float Scale, out float Out)
        {
            float freq, amp;
            Out = 0.0f;
            freq = pow(2.0, float(0));
            amp = pow(0.5, float(3-0));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(1));
            amp = pow(0.5, float(3-1));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(2));
            amp = pow(0.5, float(3-2));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        }
        
        void Unity_Minimum_float(float A, float B, out float Out)
        {
            Out = min(A, B);
        };
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Maximum_float4(float4 A, float4 B, out float4 Out)
        {
            Out = max(A, B);
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_a6ab98372a55457aa13e0da688ef1dae_Out_0_Float = _Intensity;
            float4 _Property_5401c86ee60a4d57a8b690526731076c_Out_0_Vector4 = _Color;
            float4 _Multiply_cf9d39ea36cf465790d5239828d629c2_Out_2_Vector4;
            Unity_Multiply_float4_float4((_Property_a6ab98372a55457aa13e0da688ef1dae_Out_0_Float.xxxx), _Property_5401c86ee60a4d57a8b690526731076c_Out_0_Vector4, _Multiply_cf9d39ea36cf465790d5239828d629c2_Out_2_Vector4);
            float _Property_2cc70c8b605d4503afd61a7d0bc93d98_Out_0_Float = _ColorIntensity;
            float4 _Property_24cd9d0cd5144e71a99af8c9e6b09fab_Out_0_Vector4 = _DisolveColor;
            float _Property_c5de34ce4dd34257aa46153b38e65cf1_Out_0_Float = _DisolveHeight;
            float _Remap_43ce58d5fc9e4c7cb6dba9f00db18f6d_Out_3_Float;
            Unity_Remap_float(_Property_c5de34ce4dd34257aa46153b38e65cf1_Out_0_Float, float2 (0, 1), float2 (2, -2), _Remap_43ce58d5fc9e4c7cb6dba9f00db18f6d_Out_3_Float);
            float4 _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RGBA_4_Vector4;
            float3 _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RGB_5_Vector3;
            float2 _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RG_6_Vector2;
            Unity_Combine_float(0, _Remap_43ce58d5fc9e4c7cb6dba9f00db18f6d_Out_3_Float, 0, 0, _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RGBA_4_Vector4, _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RGB_5_Vector3, _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RG_6_Vector2);
            float2 _TilingAndOffset_2726eefb16f34bce8cb5e073f113e152_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RG_6_Vector2, _TilingAndOffset_2726eefb16f34bce8cb5e073f113e152_Out_3_Vector2);
            float _Split_40376b728a2c4f88881a5da8c76b59bd_R_1_Float = _TilingAndOffset_2726eefb16f34bce8cb5e073f113e152_Out_3_Vector2[0];
            float _Split_40376b728a2c4f88881a5da8c76b59bd_G_2_Float = _TilingAndOffset_2726eefb16f34bce8cb5e073f113e152_Out_3_Vector2[1];
            float _Split_40376b728a2c4f88881a5da8c76b59bd_B_3_Float = 0;
            float _Split_40376b728a2c4f88881a5da8c76b59bd_A_4_Float = 0;
            float _Property_779d4f7aa46041068d9eb85898afa8cc_Out_0_Float = _ScrollSpeed;
            float _Multiply_2491c09d99db4705b275c993b790bf3f_Out_2_Float;
            Unity_Multiply_float_float(_Property_779d4f7aa46041068d9eb85898afa8cc_Out_0_Float, IN.TimeParameters.x, _Multiply_2491c09d99db4705b275c993b790bf3f_Out_2_Float);
            float _Negate_142d92d602014019a061bf0817486570_Out_1_Float;
            Unity_Negate_float(_Multiply_2491c09d99db4705b275c993b790bf3f_Out_2_Float, _Negate_142d92d602014019a061bf0817486570_Out_1_Float);
            float4 _Combine_fa6c3af54e834aa1908446ab4e32a900_RGBA_4_Vector4;
            float3 _Combine_fa6c3af54e834aa1908446ab4e32a900_RGB_5_Vector3;
            float2 _Combine_fa6c3af54e834aa1908446ab4e32a900_RG_6_Vector2;
            Unity_Combine_float(0, _Negate_142d92d602014019a061bf0817486570_Out_1_Float, 0, 0, _Combine_fa6c3af54e834aa1908446ab4e32a900_RGBA_4_Vector4, _Combine_fa6c3af54e834aa1908446ab4e32a900_RGB_5_Vector3, _Combine_fa6c3af54e834aa1908446ab4e32a900_RG_6_Vector2);
            float2 _TilingAndOffset_1a302799df4748eb9f9b8574ddd13f43_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_fa6c3af54e834aa1908446ab4e32a900_RG_6_Vector2, _TilingAndOffset_1a302799df4748eb9f9b8574ddd13f43_Out_3_Vector2);
            float _SimpleNoise_c8d43ffb87824260a5afd97caa5a85fe_Out_2_Float;
            Unity_SimpleNoise_Deterministic_float(_TilingAndOffset_1a302799df4748eb9f9b8574ddd13f43_Out_3_Vector2, 18, _SimpleNoise_c8d43ffb87824260a5afd97caa5a85fe_Out_2_Float);
            float2 _TilingAndOffset_a65fc9e3b24d46e48831bc8a9581e477_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_fa6c3af54e834aa1908446ab4e32a900_RG_6_Vector2, _TilingAndOffset_a65fc9e3b24d46e48831bc8a9581e477_Out_3_Vector2);
            float _SimpleNoise_f517e2c01fae4e669449d9f1ec034063_Out_2_Float;
            Unity_SimpleNoise_Deterministic_float(_TilingAndOffset_a65fc9e3b24d46e48831bc8a9581e477_Out_3_Vector2, 50, _SimpleNoise_f517e2c01fae4e669449d9f1ec034063_Out_2_Float);
            float _Minimum_d714df9c1ea343599c548bbfcfdb2070_Out_2_Float;
            Unity_Minimum_float(_SimpleNoise_c8d43ffb87824260a5afd97caa5a85fe_Out_2_Float, _SimpleNoise_f517e2c01fae4e669449d9f1ec034063_Out_2_Float, _Minimum_d714df9c1ea343599c548bbfcfdb2070_Out_2_Float);
            float _Add_81b74321eeb74761a400e98cff6f55bb_Out_2_Float;
            Unity_Add_float(_Split_40376b728a2c4f88881a5da8c76b59bd_G_2_Float, _Minimum_d714df9c1ea343599c548bbfcfdb2070_Out_2_Float, _Add_81b74321eeb74761a400e98cff6f55bb_Out_2_Float);
            float _Clamp_ff5668e2d3e2443ca20e0505031de19e_Out_3_Float;
            Unity_Clamp_float(_Add_81b74321eeb74761a400e98cff6f55bb_Out_2_Float, 0, 1, _Clamp_ff5668e2d3e2443ca20e0505031de19e_Out_3_Float);
            float _OneMinus_ff5066a075204eaa8eb2193380bd74c5_Out_1_Float;
            Unity_OneMinus_float(_Clamp_ff5668e2d3e2443ca20e0505031de19e_Out_3_Float, _OneMinus_ff5066a075204eaa8eb2193380bd74c5_Out_1_Float);
            float4 _Multiply_bb8a85fcbcc3425bab40946add5e2995_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Property_24cd9d0cd5144e71a99af8c9e6b09fab_Out_0_Vector4, (_OneMinus_ff5066a075204eaa8eb2193380bd74c5_Out_1_Float.xxxx), _Multiply_bb8a85fcbcc3425bab40946add5e2995_Out_2_Vector4);
            float4 _Multiply_a1b6ca4a0fef45d285395426f1ede815_Out_2_Vector4;
            Unity_Multiply_float4_float4((_Property_2cc70c8b605d4503afd61a7d0bc93d98_Out_0_Float.xxxx), _Multiply_bb8a85fcbcc3425bab40946add5e2995_Out_2_Vector4, _Multiply_a1b6ca4a0fef45d285395426f1ede815_Out_2_Vector4);
            float4 _Maximum_fc7fb0fb31de44bf861c7b106de07979_Out_2_Vector4;
            Unity_Maximum_float4(float4(0, 0, 0, 0), _Multiply_a1b6ca4a0fef45d285395426f1ede815_Out_2_Vector4, _Maximum_fc7fb0fb31de44bf861c7b106de07979_Out_2_Vector4);
            UnityTexture2D _Property_67337f168dfa479d9a883c4fafe2a198_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float _Property_5a7a2fb9586746d1be9ee501985debd2_Out_0_Float = _DistortionStrength;
            float _Multiply_1b904e8515e4436784b052d5290249e9_Out_2_Float;
            Unity_Multiply_float_float(_Property_5a7a2fb9586746d1be9ee501985debd2_Out_0_Float, _Property_c5de34ce4dd34257aa46153b38e65cf1_Out_0_Float, _Multiply_1b904e8515e4436784b052d5290249e9_Out_2_Float);
            float _Negate_2cdd1f9236ca4f99b473b6a143fab7c4_Out_1_Float;
            Unity_Negate_float(_Property_c5de34ce4dd34257aa46153b38e65cf1_Out_0_Float, _Negate_2cdd1f9236ca4f99b473b6a143fab7c4_Out_1_Float);
            float4 _Combine_1ffde5e2bc8f46568c1f46429934ed69_RGBA_4_Vector4;
            float3 _Combine_1ffde5e2bc8f46568c1f46429934ed69_RGB_5_Vector3;
            float2 _Combine_1ffde5e2bc8f46568c1f46429934ed69_RG_6_Vector2;
            Unity_Combine_float(0, _Negate_2cdd1f9236ca4f99b473b6a143fab7c4_Out_1_Float, 0, 0, _Combine_1ffde5e2bc8f46568c1f46429934ed69_RGBA_4_Vector4, _Combine_1ffde5e2bc8f46568c1f46429934ed69_RGB_5_Vector3, _Combine_1ffde5e2bc8f46568c1f46429934ed69_RG_6_Vector2);
            float2 _TilingAndOffset_9c375bb7cbc84550854725f0dcd297f4_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (0.5, 0.5), _Combine_1ffde5e2bc8f46568c1f46429934ed69_RG_6_Vector2, _TilingAndOffset_9c375bb7cbc84550854725f0dcd297f4_Out_3_Vector2);
            float _Split_bdd629fda6994e0bbcb19b8d138454ad_R_1_Float = _TilingAndOffset_9c375bb7cbc84550854725f0dcd297f4_Out_3_Vector2[0];
            float _Split_bdd629fda6994e0bbcb19b8d138454ad_G_2_Float = _TilingAndOffset_9c375bb7cbc84550854725f0dcd297f4_Out_3_Vector2[1];
            float _Split_bdd629fda6994e0bbcb19b8d138454ad_B_3_Float = 0;
            float _Split_bdd629fda6994e0bbcb19b8d138454ad_A_4_Float = 0;
            float _Multiply_67baf8ac851a416f82a2fab271182c71_Out_2_Float;
            Unity_Multiply_float_float(_Split_bdd629fda6994e0bbcb19b8d138454ad_G_2_Float, _Minimum_d714df9c1ea343599c548bbfcfdb2070_Out_2_Float, _Multiply_67baf8ac851a416f82a2fab271182c71_Out_2_Float);
            float _Multiply_4b5faed5858b4ea0a6d9c334709834dc_Out_2_Float;
            Unity_Multiply_float_float(_Multiply_1b904e8515e4436784b052d5290249e9_Out_2_Float, _Multiply_67baf8ac851a416f82a2fab271182c71_Out_2_Float, _Multiply_4b5faed5858b4ea0a6d9c334709834dc_Out_2_Float);
            float _Clamp_dc90f4823ab64ea79075060fda77a52b_Out_3_Float;
            Unity_Clamp_float(_Multiply_4b5faed5858b4ea0a6d9c334709834dc_Out_2_Float, -1, 0, _Clamp_dc90f4823ab64ea79075060fda77a52b_Out_3_Float);
            float4 _Combine_a42b8c2d2b00478386645f2d832d6bb6_RGBA_4_Vector4;
            float3 _Combine_a42b8c2d2b00478386645f2d832d6bb6_RGB_5_Vector3;
            float2 _Combine_a42b8c2d2b00478386645f2d832d6bb6_RG_6_Vector2;
            Unity_Combine_float(0, _Clamp_dc90f4823ab64ea79075060fda77a52b_Out_3_Float, 0, 0, _Combine_a42b8c2d2b00478386645f2d832d6bb6_RGBA_4_Vector4, _Combine_a42b8c2d2b00478386645f2d832d6bb6_RGB_5_Vector3, _Combine_a42b8c2d2b00478386645f2d832d6bb6_RG_6_Vector2);
            float2 _TilingAndOffset_b00c802fe5e84288a8beeadd19fbd581_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_a42b8c2d2b00478386645f2d832d6bb6_RG_6_Vector2, _TilingAndOffset_b00c802fe5e84288a8beeadd19fbd581_Out_3_Vector2);
            float4 _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_67337f168dfa479d9a883c4fafe2a198_Out_0_Texture2D.tex, _Property_67337f168dfa479d9a883c4fafe2a198_Out_0_Texture2D.samplerstate, _Property_67337f168dfa479d9a883c4fafe2a198_Out_0_Texture2D.GetTransformedUV(_TilingAndOffset_b00c802fe5e84288a8beeadd19fbd581_Out_3_Vector2) );
            float _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_R_4_Float = _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4.r;
            float _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_G_5_Float = _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4.g;
            float _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_B_6_Float = _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4.b;
            float _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_A_7_Float = _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4.a;
            float4 _Add_b218ccbc703e434493e6694dea00635d_Out_2_Vector4;
            Unity_Add_float4(_Maximum_fc7fb0fb31de44bf861c7b106de07979_Out_2_Vector4, _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4, _Add_b218ccbc703e434493e6694dea00635d_Out_2_Vector4);
            float4 _Add_2ab29bb60b7043d28f6bd161e52fcbd8_Out_2_Vector4;
            Unity_Add_float4(_Multiply_cf9d39ea36cf465790d5239828d629c2_Out_2_Vector4, _Add_b218ccbc703e434493e6694dea00635d_Out_2_Vector4, _Add_2ab29bb60b7043d28f6bd161e52fcbd8_Out_2_Vector4);
            float _Clamp_f3bb0624e5aa4a02923604f9c5839c27_Out_3_Float;
            Unity_Clamp_float(_Clamp_ff5668e2d3e2443ca20e0505031de19e_Out_3_Float, 0, 1, _Clamp_f3bb0624e5aa4a02923604f9c5839c27_Out_3_Float);
            float _Multiply_86658fa4e2ae43d7a5dc89ccf2a1e0f6_Out_2_Float;
            Unity_Multiply_float_float(_SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_A_7_Float, _Clamp_f3bb0624e5aa4a02923604f9c5839c27_Out_3_Float, _Multiply_86658fa4e2ae43d7a5dc89ccf2a1e0f6_Out_2_Float);
            surface.BaseColor = (_Add_2ab29bb60b7043d28f6bd161e52fcbd8_Out_2_Vector4.xyz);
            surface.Alpha = _Multiply_86658fa4e2ae43d7a5dc89ccf2a1e0f6_Out_2_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENESELECTIONPASS 1
        
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float _ScrollSpeed;
        float _DisolveHeight;
        float4 _DisolveColor;
        float _ColorIntensity;
        float _DistortionStrength;
        float _Intensity;
        float4 _Color;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Negate_float(float In, out float Out)
        {
            Out = -1 * In;
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        float Unity_SimpleNoise_ValueNoise_Deterministic_float (float2 uv)
        {
            float2 i = floor(uv);
            float2 f = frac(uv);
            f = f * f * (3.0 - 2.0 * f);
            uv = abs(frac(uv) - 0.5);
            float2 c0 = i + float2(0.0, 0.0);
            float2 c1 = i + float2(1.0, 0.0);
            float2 c2 = i + float2(0.0, 1.0);
            float2 c3 = i + float2(1.0, 1.0);
            float r0; Hash_Tchou_2_1_float(c0, r0);
            float r1; Hash_Tchou_2_1_float(c1, r1);
            float r2; Hash_Tchou_2_1_float(c2, r2);
            float r3; Hash_Tchou_2_1_float(c3, r3);
            float bottomOfGrid = lerp(r0, r1, f.x);
            float topOfGrid = lerp(r2, r3, f.x);
            float t = lerp(bottomOfGrid, topOfGrid, f.y);
            return t;
        }
        
        void Unity_SimpleNoise_Deterministic_float(float2 UV, float Scale, out float Out)
        {
            float freq, amp;
            Out = 0.0f;
            freq = pow(2.0, float(0));
            amp = pow(0.5, float(3-0));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(1));
            amp = pow(0.5, float(3-1));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(2));
            amp = pow(0.5, float(3-2));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        }
        
        void Unity_Minimum_float(float A, float B, out float Out)
        {
            Out = min(A, B);
        };
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_67337f168dfa479d9a883c4fafe2a198_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float _Property_5a7a2fb9586746d1be9ee501985debd2_Out_0_Float = _DistortionStrength;
            float _Property_c5de34ce4dd34257aa46153b38e65cf1_Out_0_Float = _DisolveHeight;
            float _Multiply_1b904e8515e4436784b052d5290249e9_Out_2_Float;
            Unity_Multiply_float_float(_Property_5a7a2fb9586746d1be9ee501985debd2_Out_0_Float, _Property_c5de34ce4dd34257aa46153b38e65cf1_Out_0_Float, _Multiply_1b904e8515e4436784b052d5290249e9_Out_2_Float);
            float _Negate_2cdd1f9236ca4f99b473b6a143fab7c4_Out_1_Float;
            Unity_Negate_float(_Property_c5de34ce4dd34257aa46153b38e65cf1_Out_0_Float, _Negate_2cdd1f9236ca4f99b473b6a143fab7c4_Out_1_Float);
            float4 _Combine_1ffde5e2bc8f46568c1f46429934ed69_RGBA_4_Vector4;
            float3 _Combine_1ffde5e2bc8f46568c1f46429934ed69_RGB_5_Vector3;
            float2 _Combine_1ffde5e2bc8f46568c1f46429934ed69_RG_6_Vector2;
            Unity_Combine_float(0, _Negate_2cdd1f9236ca4f99b473b6a143fab7c4_Out_1_Float, 0, 0, _Combine_1ffde5e2bc8f46568c1f46429934ed69_RGBA_4_Vector4, _Combine_1ffde5e2bc8f46568c1f46429934ed69_RGB_5_Vector3, _Combine_1ffde5e2bc8f46568c1f46429934ed69_RG_6_Vector2);
            float2 _TilingAndOffset_9c375bb7cbc84550854725f0dcd297f4_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (0.5, 0.5), _Combine_1ffde5e2bc8f46568c1f46429934ed69_RG_6_Vector2, _TilingAndOffset_9c375bb7cbc84550854725f0dcd297f4_Out_3_Vector2);
            float _Split_bdd629fda6994e0bbcb19b8d138454ad_R_1_Float = _TilingAndOffset_9c375bb7cbc84550854725f0dcd297f4_Out_3_Vector2[0];
            float _Split_bdd629fda6994e0bbcb19b8d138454ad_G_2_Float = _TilingAndOffset_9c375bb7cbc84550854725f0dcd297f4_Out_3_Vector2[1];
            float _Split_bdd629fda6994e0bbcb19b8d138454ad_B_3_Float = 0;
            float _Split_bdd629fda6994e0bbcb19b8d138454ad_A_4_Float = 0;
            float _Property_779d4f7aa46041068d9eb85898afa8cc_Out_0_Float = _ScrollSpeed;
            float _Multiply_2491c09d99db4705b275c993b790bf3f_Out_2_Float;
            Unity_Multiply_float_float(_Property_779d4f7aa46041068d9eb85898afa8cc_Out_0_Float, IN.TimeParameters.x, _Multiply_2491c09d99db4705b275c993b790bf3f_Out_2_Float);
            float _Negate_142d92d602014019a061bf0817486570_Out_1_Float;
            Unity_Negate_float(_Multiply_2491c09d99db4705b275c993b790bf3f_Out_2_Float, _Negate_142d92d602014019a061bf0817486570_Out_1_Float);
            float4 _Combine_fa6c3af54e834aa1908446ab4e32a900_RGBA_4_Vector4;
            float3 _Combine_fa6c3af54e834aa1908446ab4e32a900_RGB_5_Vector3;
            float2 _Combine_fa6c3af54e834aa1908446ab4e32a900_RG_6_Vector2;
            Unity_Combine_float(0, _Negate_142d92d602014019a061bf0817486570_Out_1_Float, 0, 0, _Combine_fa6c3af54e834aa1908446ab4e32a900_RGBA_4_Vector4, _Combine_fa6c3af54e834aa1908446ab4e32a900_RGB_5_Vector3, _Combine_fa6c3af54e834aa1908446ab4e32a900_RG_6_Vector2);
            float2 _TilingAndOffset_1a302799df4748eb9f9b8574ddd13f43_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_fa6c3af54e834aa1908446ab4e32a900_RG_6_Vector2, _TilingAndOffset_1a302799df4748eb9f9b8574ddd13f43_Out_3_Vector2);
            float _SimpleNoise_c8d43ffb87824260a5afd97caa5a85fe_Out_2_Float;
            Unity_SimpleNoise_Deterministic_float(_TilingAndOffset_1a302799df4748eb9f9b8574ddd13f43_Out_3_Vector2, 18, _SimpleNoise_c8d43ffb87824260a5afd97caa5a85fe_Out_2_Float);
            float2 _TilingAndOffset_a65fc9e3b24d46e48831bc8a9581e477_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_fa6c3af54e834aa1908446ab4e32a900_RG_6_Vector2, _TilingAndOffset_a65fc9e3b24d46e48831bc8a9581e477_Out_3_Vector2);
            float _SimpleNoise_f517e2c01fae4e669449d9f1ec034063_Out_2_Float;
            Unity_SimpleNoise_Deterministic_float(_TilingAndOffset_a65fc9e3b24d46e48831bc8a9581e477_Out_3_Vector2, 50, _SimpleNoise_f517e2c01fae4e669449d9f1ec034063_Out_2_Float);
            float _Minimum_d714df9c1ea343599c548bbfcfdb2070_Out_2_Float;
            Unity_Minimum_float(_SimpleNoise_c8d43ffb87824260a5afd97caa5a85fe_Out_2_Float, _SimpleNoise_f517e2c01fae4e669449d9f1ec034063_Out_2_Float, _Minimum_d714df9c1ea343599c548bbfcfdb2070_Out_2_Float);
            float _Multiply_67baf8ac851a416f82a2fab271182c71_Out_2_Float;
            Unity_Multiply_float_float(_Split_bdd629fda6994e0bbcb19b8d138454ad_G_2_Float, _Minimum_d714df9c1ea343599c548bbfcfdb2070_Out_2_Float, _Multiply_67baf8ac851a416f82a2fab271182c71_Out_2_Float);
            float _Multiply_4b5faed5858b4ea0a6d9c334709834dc_Out_2_Float;
            Unity_Multiply_float_float(_Multiply_1b904e8515e4436784b052d5290249e9_Out_2_Float, _Multiply_67baf8ac851a416f82a2fab271182c71_Out_2_Float, _Multiply_4b5faed5858b4ea0a6d9c334709834dc_Out_2_Float);
            float _Clamp_dc90f4823ab64ea79075060fda77a52b_Out_3_Float;
            Unity_Clamp_float(_Multiply_4b5faed5858b4ea0a6d9c334709834dc_Out_2_Float, -1, 0, _Clamp_dc90f4823ab64ea79075060fda77a52b_Out_3_Float);
            float4 _Combine_a42b8c2d2b00478386645f2d832d6bb6_RGBA_4_Vector4;
            float3 _Combine_a42b8c2d2b00478386645f2d832d6bb6_RGB_5_Vector3;
            float2 _Combine_a42b8c2d2b00478386645f2d832d6bb6_RG_6_Vector2;
            Unity_Combine_float(0, _Clamp_dc90f4823ab64ea79075060fda77a52b_Out_3_Float, 0, 0, _Combine_a42b8c2d2b00478386645f2d832d6bb6_RGBA_4_Vector4, _Combine_a42b8c2d2b00478386645f2d832d6bb6_RGB_5_Vector3, _Combine_a42b8c2d2b00478386645f2d832d6bb6_RG_6_Vector2);
            float2 _TilingAndOffset_b00c802fe5e84288a8beeadd19fbd581_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_a42b8c2d2b00478386645f2d832d6bb6_RG_6_Vector2, _TilingAndOffset_b00c802fe5e84288a8beeadd19fbd581_Out_3_Vector2);
            float4 _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_67337f168dfa479d9a883c4fafe2a198_Out_0_Texture2D.tex, _Property_67337f168dfa479d9a883c4fafe2a198_Out_0_Texture2D.samplerstate, _Property_67337f168dfa479d9a883c4fafe2a198_Out_0_Texture2D.GetTransformedUV(_TilingAndOffset_b00c802fe5e84288a8beeadd19fbd581_Out_3_Vector2) );
            float _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_R_4_Float = _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4.r;
            float _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_G_5_Float = _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4.g;
            float _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_B_6_Float = _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4.b;
            float _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_A_7_Float = _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4.a;
            float _Remap_43ce58d5fc9e4c7cb6dba9f00db18f6d_Out_3_Float;
            Unity_Remap_float(_Property_c5de34ce4dd34257aa46153b38e65cf1_Out_0_Float, float2 (0, 1), float2 (2, -2), _Remap_43ce58d5fc9e4c7cb6dba9f00db18f6d_Out_3_Float);
            float4 _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RGBA_4_Vector4;
            float3 _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RGB_5_Vector3;
            float2 _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RG_6_Vector2;
            Unity_Combine_float(0, _Remap_43ce58d5fc9e4c7cb6dba9f00db18f6d_Out_3_Float, 0, 0, _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RGBA_4_Vector4, _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RGB_5_Vector3, _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RG_6_Vector2);
            float2 _TilingAndOffset_2726eefb16f34bce8cb5e073f113e152_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RG_6_Vector2, _TilingAndOffset_2726eefb16f34bce8cb5e073f113e152_Out_3_Vector2);
            float _Split_40376b728a2c4f88881a5da8c76b59bd_R_1_Float = _TilingAndOffset_2726eefb16f34bce8cb5e073f113e152_Out_3_Vector2[0];
            float _Split_40376b728a2c4f88881a5da8c76b59bd_G_2_Float = _TilingAndOffset_2726eefb16f34bce8cb5e073f113e152_Out_3_Vector2[1];
            float _Split_40376b728a2c4f88881a5da8c76b59bd_B_3_Float = 0;
            float _Split_40376b728a2c4f88881a5da8c76b59bd_A_4_Float = 0;
            float _Add_81b74321eeb74761a400e98cff6f55bb_Out_2_Float;
            Unity_Add_float(_Split_40376b728a2c4f88881a5da8c76b59bd_G_2_Float, _Minimum_d714df9c1ea343599c548bbfcfdb2070_Out_2_Float, _Add_81b74321eeb74761a400e98cff6f55bb_Out_2_Float);
            float _Clamp_ff5668e2d3e2443ca20e0505031de19e_Out_3_Float;
            Unity_Clamp_float(_Add_81b74321eeb74761a400e98cff6f55bb_Out_2_Float, 0, 1, _Clamp_ff5668e2d3e2443ca20e0505031de19e_Out_3_Float);
            float _Clamp_f3bb0624e5aa4a02923604f9c5839c27_Out_3_Float;
            Unity_Clamp_float(_Clamp_ff5668e2d3e2443ca20e0505031de19e_Out_3_Float, 0, 1, _Clamp_f3bb0624e5aa4a02923604f9c5839c27_Out_3_Float);
            float _Multiply_86658fa4e2ae43d7a5dc89ccf2a1e0f6_Out_2_Float;
            Unity_Multiply_float_float(_SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_A_7_Float, _Clamp_f3bb0624e5aa4a02923604f9c5839c27_Out_3_Float, _Multiply_86658fa4e2ae43d7a5dc89ccf2a1e0f6_Out_2_Float);
            surface.Alpha = _Multiply_86658fa4e2ae43d7a5dc89ccf2a1e0f6_Out_2_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
        // Render State
        Cull Back
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float _ScrollSpeed;
        float _DisolveHeight;
        float4 _DisolveColor;
        float _ColorIntensity;
        float _DistortionStrength;
        float _Intensity;
        float4 _Color;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Negate_float(float In, out float Out)
        {
            Out = -1 * In;
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        float Unity_SimpleNoise_ValueNoise_Deterministic_float (float2 uv)
        {
            float2 i = floor(uv);
            float2 f = frac(uv);
            f = f * f * (3.0 - 2.0 * f);
            uv = abs(frac(uv) - 0.5);
            float2 c0 = i + float2(0.0, 0.0);
            float2 c1 = i + float2(1.0, 0.0);
            float2 c2 = i + float2(0.0, 1.0);
            float2 c3 = i + float2(1.0, 1.0);
            float r0; Hash_Tchou_2_1_float(c0, r0);
            float r1; Hash_Tchou_2_1_float(c1, r1);
            float r2; Hash_Tchou_2_1_float(c2, r2);
            float r3; Hash_Tchou_2_1_float(c3, r3);
            float bottomOfGrid = lerp(r0, r1, f.x);
            float topOfGrid = lerp(r2, r3, f.x);
            float t = lerp(bottomOfGrid, topOfGrid, f.y);
            return t;
        }
        
        void Unity_SimpleNoise_Deterministic_float(float2 UV, float Scale, out float Out)
        {
            float freq, amp;
            Out = 0.0f;
            freq = pow(2.0, float(0));
            amp = pow(0.5, float(3-0));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(1));
            amp = pow(0.5, float(3-1));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(2));
            amp = pow(0.5, float(3-2));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        }
        
        void Unity_Minimum_float(float A, float B, out float Out)
        {
            Out = min(A, B);
        };
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_67337f168dfa479d9a883c4fafe2a198_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float _Property_5a7a2fb9586746d1be9ee501985debd2_Out_0_Float = _DistortionStrength;
            float _Property_c5de34ce4dd34257aa46153b38e65cf1_Out_0_Float = _DisolveHeight;
            float _Multiply_1b904e8515e4436784b052d5290249e9_Out_2_Float;
            Unity_Multiply_float_float(_Property_5a7a2fb9586746d1be9ee501985debd2_Out_0_Float, _Property_c5de34ce4dd34257aa46153b38e65cf1_Out_0_Float, _Multiply_1b904e8515e4436784b052d5290249e9_Out_2_Float);
            float _Negate_2cdd1f9236ca4f99b473b6a143fab7c4_Out_1_Float;
            Unity_Negate_float(_Property_c5de34ce4dd34257aa46153b38e65cf1_Out_0_Float, _Negate_2cdd1f9236ca4f99b473b6a143fab7c4_Out_1_Float);
            float4 _Combine_1ffde5e2bc8f46568c1f46429934ed69_RGBA_4_Vector4;
            float3 _Combine_1ffde5e2bc8f46568c1f46429934ed69_RGB_5_Vector3;
            float2 _Combine_1ffde5e2bc8f46568c1f46429934ed69_RG_6_Vector2;
            Unity_Combine_float(0, _Negate_2cdd1f9236ca4f99b473b6a143fab7c4_Out_1_Float, 0, 0, _Combine_1ffde5e2bc8f46568c1f46429934ed69_RGBA_4_Vector4, _Combine_1ffde5e2bc8f46568c1f46429934ed69_RGB_5_Vector3, _Combine_1ffde5e2bc8f46568c1f46429934ed69_RG_6_Vector2);
            float2 _TilingAndOffset_9c375bb7cbc84550854725f0dcd297f4_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (0.5, 0.5), _Combine_1ffde5e2bc8f46568c1f46429934ed69_RG_6_Vector2, _TilingAndOffset_9c375bb7cbc84550854725f0dcd297f4_Out_3_Vector2);
            float _Split_bdd629fda6994e0bbcb19b8d138454ad_R_1_Float = _TilingAndOffset_9c375bb7cbc84550854725f0dcd297f4_Out_3_Vector2[0];
            float _Split_bdd629fda6994e0bbcb19b8d138454ad_G_2_Float = _TilingAndOffset_9c375bb7cbc84550854725f0dcd297f4_Out_3_Vector2[1];
            float _Split_bdd629fda6994e0bbcb19b8d138454ad_B_3_Float = 0;
            float _Split_bdd629fda6994e0bbcb19b8d138454ad_A_4_Float = 0;
            float _Property_779d4f7aa46041068d9eb85898afa8cc_Out_0_Float = _ScrollSpeed;
            float _Multiply_2491c09d99db4705b275c993b790bf3f_Out_2_Float;
            Unity_Multiply_float_float(_Property_779d4f7aa46041068d9eb85898afa8cc_Out_0_Float, IN.TimeParameters.x, _Multiply_2491c09d99db4705b275c993b790bf3f_Out_2_Float);
            float _Negate_142d92d602014019a061bf0817486570_Out_1_Float;
            Unity_Negate_float(_Multiply_2491c09d99db4705b275c993b790bf3f_Out_2_Float, _Negate_142d92d602014019a061bf0817486570_Out_1_Float);
            float4 _Combine_fa6c3af54e834aa1908446ab4e32a900_RGBA_4_Vector4;
            float3 _Combine_fa6c3af54e834aa1908446ab4e32a900_RGB_5_Vector3;
            float2 _Combine_fa6c3af54e834aa1908446ab4e32a900_RG_6_Vector2;
            Unity_Combine_float(0, _Negate_142d92d602014019a061bf0817486570_Out_1_Float, 0, 0, _Combine_fa6c3af54e834aa1908446ab4e32a900_RGBA_4_Vector4, _Combine_fa6c3af54e834aa1908446ab4e32a900_RGB_5_Vector3, _Combine_fa6c3af54e834aa1908446ab4e32a900_RG_6_Vector2);
            float2 _TilingAndOffset_1a302799df4748eb9f9b8574ddd13f43_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_fa6c3af54e834aa1908446ab4e32a900_RG_6_Vector2, _TilingAndOffset_1a302799df4748eb9f9b8574ddd13f43_Out_3_Vector2);
            float _SimpleNoise_c8d43ffb87824260a5afd97caa5a85fe_Out_2_Float;
            Unity_SimpleNoise_Deterministic_float(_TilingAndOffset_1a302799df4748eb9f9b8574ddd13f43_Out_3_Vector2, 18, _SimpleNoise_c8d43ffb87824260a5afd97caa5a85fe_Out_2_Float);
            float2 _TilingAndOffset_a65fc9e3b24d46e48831bc8a9581e477_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_fa6c3af54e834aa1908446ab4e32a900_RG_6_Vector2, _TilingAndOffset_a65fc9e3b24d46e48831bc8a9581e477_Out_3_Vector2);
            float _SimpleNoise_f517e2c01fae4e669449d9f1ec034063_Out_2_Float;
            Unity_SimpleNoise_Deterministic_float(_TilingAndOffset_a65fc9e3b24d46e48831bc8a9581e477_Out_3_Vector2, 50, _SimpleNoise_f517e2c01fae4e669449d9f1ec034063_Out_2_Float);
            float _Minimum_d714df9c1ea343599c548bbfcfdb2070_Out_2_Float;
            Unity_Minimum_float(_SimpleNoise_c8d43ffb87824260a5afd97caa5a85fe_Out_2_Float, _SimpleNoise_f517e2c01fae4e669449d9f1ec034063_Out_2_Float, _Minimum_d714df9c1ea343599c548bbfcfdb2070_Out_2_Float);
            float _Multiply_67baf8ac851a416f82a2fab271182c71_Out_2_Float;
            Unity_Multiply_float_float(_Split_bdd629fda6994e0bbcb19b8d138454ad_G_2_Float, _Minimum_d714df9c1ea343599c548bbfcfdb2070_Out_2_Float, _Multiply_67baf8ac851a416f82a2fab271182c71_Out_2_Float);
            float _Multiply_4b5faed5858b4ea0a6d9c334709834dc_Out_2_Float;
            Unity_Multiply_float_float(_Multiply_1b904e8515e4436784b052d5290249e9_Out_2_Float, _Multiply_67baf8ac851a416f82a2fab271182c71_Out_2_Float, _Multiply_4b5faed5858b4ea0a6d9c334709834dc_Out_2_Float);
            float _Clamp_dc90f4823ab64ea79075060fda77a52b_Out_3_Float;
            Unity_Clamp_float(_Multiply_4b5faed5858b4ea0a6d9c334709834dc_Out_2_Float, -1, 0, _Clamp_dc90f4823ab64ea79075060fda77a52b_Out_3_Float);
            float4 _Combine_a42b8c2d2b00478386645f2d832d6bb6_RGBA_4_Vector4;
            float3 _Combine_a42b8c2d2b00478386645f2d832d6bb6_RGB_5_Vector3;
            float2 _Combine_a42b8c2d2b00478386645f2d832d6bb6_RG_6_Vector2;
            Unity_Combine_float(0, _Clamp_dc90f4823ab64ea79075060fda77a52b_Out_3_Float, 0, 0, _Combine_a42b8c2d2b00478386645f2d832d6bb6_RGBA_4_Vector4, _Combine_a42b8c2d2b00478386645f2d832d6bb6_RGB_5_Vector3, _Combine_a42b8c2d2b00478386645f2d832d6bb6_RG_6_Vector2);
            float2 _TilingAndOffset_b00c802fe5e84288a8beeadd19fbd581_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_a42b8c2d2b00478386645f2d832d6bb6_RG_6_Vector2, _TilingAndOffset_b00c802fe5e84288a8beeadd19fbd581_Out_3_Vector2);
            float4 _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_67337f168dfa479d9a883c4fafe2a198_Out_0_Texture2D.tex, _Property_67337f168dfa479d9a883c4fafe2a198_Out_0_Texture2D.samplerstate, _Property_67337f168dfa479d9a883c4fafe2a198_Out_0_Texture2D.GetTransformedUV(_TilingAndOffset_b00c802fe5e84288a8beeadd19fbd581_Out_3_Vector2) );
            float _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_R_4_Float = _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4.r;
            float _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_G_5_Float = _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4.g;
            float _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_B_6_Float = _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4.b;
            float _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_A_7_Float = _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4.a;
            float _Remap_43ce58d5fc9e4c7cb6dba9f00db18f6d_Out_3_Float;
            Unity_Remap_float(_Property_c5de34ce4dd34257aa46153b38e65cf1_Out_0_Float, float2 (0, 1), float2 (2, -2), _Remap_43ce58d5fc9e4c7cb6dba9f00db18f6d_Out_3_Float);
            float4 _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RGBA_4_Vector4;
            float3 _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RGB_5_Vector3;
            float2 _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RG_6_Vector2;
            Unity_Combine_float(0, _Remap_43ce58d5fc9e4c7cb6dba9f00db18f6d_Out_3_Float, 0, 0, _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RGBA_4_Vector4, _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RGB_5_Vector3, _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RG_6_Vector2);
            float2 _TilingAndOffset_2726eefb16f34bce8cb5e073f113e152_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RG_6_Vector2, _TilingAndOffset_2726eefb16f34bce8cb5e073f113e152_Out_3_Vector2);
            float _Split_40376b728a2c4f88881a5da8c76b59bd_R_1_Float = _TilingAndOffset_2726eefb16f34bce8cb5e073f113e152_Out_3_Vector2[0];
            float _Split_40376b728a2c4f88881a5da8c76b59bd_G_2_Float = _TilingAndOffset_2726eefb16f34bce8cb5e073f113e152_Out_3_Vector2[1];
            float _Split_40376b728a2c4f88881a5da8c76b59bd_B_3_Float = 0;
            float _Split_40376b728a2c4f88881a5da8c76b59bd_A_4_Float = 0;
            float _Add_81b74321eeb74761a400e98cff6f55bb_Out_2_Float;
            Unity_Add_float(_Split_40376b728a2c4f88881a5da8c76b59bd_G_2_Float, _Minimum_d714df9c1ea343599c548bbfcfdb2070_Out_2_Float, _Add_81b74321eeb74761a400e98cff6f55bb_Out_2_Float);
            float _Clamp_ff5668e2d3e2443ca20e0505031de19e_Out_3_Float;
            Unity_Clamp_float(_Add_81b74321eeb74761a400e98cff6f55bb_Out_2_Float, 0, 1, _Clamp_ff5668e2d3e2443ca20e0505031de19e_Out_3_Float);
            float _Clamp_f3bb0624e5aa4a02923604f9c5839c27_Out_3_Float;
            Unity_Clamp_float(_Clamp_ff5668e2d3e2443ca20e0505031de19e_Out_3_Float, 0, 1, _Clamp_f3bb0624e5aa4a02923604f9c5839c27_Out_3_Float);
            float _Multiply_86658fa4e2ae43d7a5dc89ccf2a1e0f6_Out_2_Float;
            Unity_Multiply_float_float(_SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_A_7_Float, _Clamp_f3bb0624e5aa4a02923604f9c5839c27_Out_3_Float, _Multiply_86658fa4e2ae43d7a5dc89ccf2a1e0f6_Out_2_Float);
            surface.Alpha = _Multiply_86658fa4e2ae43d7a5dc89ccf2a1e0f6_Out_2_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "Sprite Unlit"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
        
        // Render State
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITEFORWARD
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 color : INTERP1;
             float3 positionWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.color.xyzw = input.color;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.color = input.color.xyzw;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float _ScrollSpeed;
        float _DisolveHeight;
        float4 _DisolveColor;
        float _ColorIntensity;
        float _DistortionStrength;
        float _Intensity;
        float4 _Color;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Negate_float(float In, out float Out)
        {
            Out = -1 * In;
        }
        
        float Unity_SimpleNoise_ValueNoise_Deterministic_float (float2 uv)
        {
            float2 i = floor(uv);
            float2 f = frac(uv);
            f = f * f * (3.0 - 2.0 * f);
            uv = abs(frac(uv) - 0.5);
            float2 c0 = i + float2(0.0, 0.0);
            float2 c1 = i + float2(1.0, 0.0);
            float2 c2 = i + float2(0.0, 1.0);
            float2 c3 = i + float2(1.0, 1.0);
            float r0; Hash_Tchou_2_1_float(c0, r0);
            float r1; Hash_Tchou_2_1_float(c1, r1);
            float r2; Hash_Tchou_2_1_float(c2, r2);
            float r3; Hash_Tchou_2_1_float(c3, r3);
            float bottomOfGrid = lerp(r0, r1, f.x);
            float topOfGrid = lerp(r2, r3, f.x);
            float t = lerp(bottomOfGrid, topOfGrid, f.y);
            return t;
        }
        
        void Unity_SimpleNoise_Deterministic_float(float2 UV, float Scale, out float Out)
        {
            float freq, amp;
            Out = 0.0f;
            freq = pow(2.0, float(0));
            amp = pow(0.5, float(3-0));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(1));
            amp = pow(0.5, float(3-1));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(2));
            amp = pow(0.5, float(3-2));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        }
        
        void Unity_Minimum_float(float A, float B, out float Out)
        {
            Out = min(A, B);
        };
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Maximum_float4(float4 A, float4 B, out float4 Out)
        {
            Out = max(A, B);
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_a6ab98372a55457aa13e0da688ef1dae_Out_0_Float = _Intensity;
            float4 _Property_5401c86ee60a4d57a8b690526731076c_Out_0_Vector4 = _Color;
            float4 _Multiply_cf9d39ea36cf465790d5239828d629c2_Out_2_Vector4;
            Unity_Multiply_float4_float4((_Property_a6ab98372a55457aa13e0da688ef1dae_Out_0_Float.xxxx), _Property_5401c86ee60a4d57a8b690526731076c_Out_0_Vector4, _Multiply_cf9d39ea36cf465790d5239828d629c2_Out_2_Vector4);
            float _Property_2cc70c8b605d4503afd61a7d0bc93d98_Out_0_Float = _ColorIntensity;
            float4 _Property_24cd9d0cd5144e71a99af8c9e6b09fab_Out_0_Vector4 = _DisolveColor;
            float _Property_c5de34ce4dd34257aa46153b38e65cf1_Out_0_Float = _DisolveHeight;
            float _Remap_43ce58d5fc9e4c7cb6dba9f00db18f6d_Out_3_Float;
            Unity_Remap_float(_Property_c5de34ce4dd34257aa46153b38e65cf1_Out_0_Float, float2 (0, 1), float2 (2, -2), _Remap_43ce58d5fc9e4c7cb6dba9f00db18f6d_Out_3_Float);
            float4 _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RGBA_4_Vector4;
            float3 _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RGB_5_Vector3;
            float2 _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RG_6_Vector2;
            Unity_Combine_float(0, _Remap_43ce58d5fc9e4c7cb6dba9f00db18f6d_Out_3_Float, 0, 0, _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RGBA_4_Vector4, _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RGB_5_Vector3, _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RG_6_Vector2);
            float2 _TilingAndOffset_2726eefb16f34bce8cb5e073f113e152_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_980ba2a8e9794e65a61f9b521bcfb1c2_RG_6_Vector2, _TilingAndOffset_2726eefb16f34bce8cb5e073f113e152_Out_3_Vector2);
            float _Split_40376b728a2c4f88881a5da8c76b59bd_R_1_Float = _TilingAndOffset_2726eefb16f34bce8cb5e073f113e152_Out_3_Vector2[0];
            float _Split_40376b728a2c4f88881a5da8c76b59bd_G_2_Float = _TilingAndOffset_2726eefb16f34bce8cb5e073f113e152_Out_3_Vector2[1];
            float _Split_40376b728a2c4f88881a5da8c76b59bd_B_3_Float = 0;
            float _Split_40376b728a2c4f88881a5da8c76b59bd_A_4_Float = 0;
            float _Property_779d4f7aa46041068d9eb85898afa8cc_Out_0_Float = _ScrollSpeed;
            float _Multiply_2491c09d99db4705b275c993b790bf3f_Out_2_Float;
            Unity_Multiply_float_float(_Property_779d4f7aa46041068d9eb85898afa8cc_Out_0_Float, IN.TimeParameters.x, _Multiply_2491c09d99db4705b275c993b790bf3f_Out_2_Float);
            float _Negate_142d92d602014019a061bf0817486570_Out_1_Float;
            Unity_Negate_float(_Multiply_2491c09d99db4705b275c993b790bf3f_Out_2_Float, _Negate_142d92d602014019a061bf0817486570_Out_1_Float);
            float4 _Combine_fa6c3af54e834aa1908446ab4e32a900_RGBA_4_Vector4;
            float3 _Combine_fa6c3af54e834aa1908446ab4e32a900_RGB_5_Vector3;
            float2 _Combine_fa6c3af54e834aa1908446ab4e32a900_RG_6_Vector2;
            Unity_Combine_float(0, _Negate_142d92d602014019a061bf0817486570_Out_1_Float, 0, 0, _Combine_fa6c3af54e834aa1908446ab4e32a900_RGBA_4_Vector4, _Combine_fa6c3af54e834aa1908446ab4e32a900_RGB_5_Vector3, _Combine_fa6c3af54e834aa1908446ab4e32a900_RG_6_Vector2);
            float2 _TilingAndOffset_1a302799df4748eb9f9b8574ddd13f43_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_fa6c3af54e834aa1908446ab4e32a900_RG_6_Vector2, _TilingAndOffset_1a302799df4748eb9f9b8574ddd13f43_Out_3_Vector2);
            float _SimpleNoise_c8d43ffb87824260a5afd97caa5a85fe_Out_2_Float;
            Unity_SimpleNoise_Deterministic_float(_TilingAndOffset_1a302799df4748eb9f9b8574ddd13f43_Out_3_Vector2, 18, _SimpleNoise_c8d43ffb87824260a5afd97caa5a85fe_Out_2_Float);
            float2 _TilingAndOffset_a65fc9e3b24d46e48831bc8a9581e477_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_fa6c3af54e834aa1908446ab4e32a900_RG_6_Vector2, _TilingAndOffset_a65fc9e3b24d46e48831bc8a9581e477_Out_3_Vector2);
            float _SimpleNoise_f517e2c01fae4e669449d9f1ec034063_Out_2_Float;
            Unity_SimpleNoise_Deterministic_float(_TilingAndOffset_a65fc9e3b24d46e48831bc8a9581e477_Out_3_Vector2, 50, _SimpleNoise_f517e2c01fae4e669449d9f1ec034063_Out_2_Float);
            float _Minimum_d714df9c1ea343599c548bbfcfdb2070_Out_2_Float;
            Unity_Minimum_float(_SimpleNoise_c8d43ffb87824260a5afd97caa5a85fe_Out_2_Float, _SimpleNoise_f517e2c01fae4e669449d9f1ec034063_Out_2_Float, _Minimum_d714df9c1ea343599c548bbfcfdb2070_Out_2_Float);
            float _Add_81b74321eeb74761a400e98cff6f55bb_Out_2_Float;
            Unity_Add_float(_Split_40376b728a2c4f88881a5da8c76b59bd_G_2_Float, _Minimum_d714df9c1ea343599c548bbfcfdb2070_Out_2_Float, _Add_81b74321eeb74761a400e98cff6f55bb_Out_2_Float);
            float _Clamp_ff5668e2d3e2443ca20e0505031de19e_Out_3_Float;
            Unity_Clamp_float(_Add_81b74321eeb74761a400e98cff6f55bb_Out_2_Float, 0, 1, _Clamp_ff5668e2d3e2443ca20e0505031de19e_Out_3_Float);
            float _OneMinus_ff5066a075204eaa8eb2193380bd74c5_Out_1_Float;
            Unity_OneMinus_float(_Clamp_ff5668e2d3e2443ca20e0505031de19e_Out_3_Float, _OneMinus_ff5066a075204eaa8eb2193380bd74c5_Out_1_Float);
            float4 _Multiply_bb8a85fcbcc3425bab40946add5e2995_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Property_24cd9d0cd5144e71a99af8c9e6b09fab_Out_0_Vector4, (_OneMinus_ff5066a075204eaa8eb2193380bd74c5_Out_1_Float.xxxx), _Multiply_bb8a85fcbcc3425bab40946add5e2995_Out_2_Vector4);
            float4 _Multiply_a1b6ca4a0fef45d285395426f1ede815_Out_2_Vector4;
            Unity_Multiply_float4_float4((_Property_2cc70c8b605d4503afd61a7d0bc93d98_Out_0_Float.xxxx), _Multiply_bb8a85fcbcc3425bab40946add5e2995_Out_2_Vector4, _Multiply_a1b6ca4a0fef45d285395426f1ede815_Out_2_Vector4);
            float4 _Maximum_fc7fb0fb31de44bf861c7b106de07979_Out_2_Vector4;
            Unity_Maximum_float4(float4(0, 0, 0, 0), _Multiply_a1b6ca4a0fef45d285395426f1ede815_Out_2_Vector4, _Maximum_fc7fb0fb31de44bf861c7b106de07979_Out_2_Vector4);
            UnityTexture2D _Property_67337f168dfa479d9a883c4fafe2a198_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float _Property_5a7a2fb9586746d1be9ee501985debd2_Out_0_Float = _DistortionStrength;
            float _Multiply_1b904e8515e4436784b052d5290249e9_Out_2_Float;
            Unity_Multiply_float_float(_Property_5a7a2fb9586746d1be9ee501985debd2_Out_0_Float, _Property_c5de34ce4dd34257aa46153b38e65cf1_Out_0_Float, _Multiply_1b904e8515e4436784b052d5290249e9_Out_2_Float);
            float _Negate_2cdd1f9236ca4f99b473b6a143fab7c4_Out_1_Float;
            Unity_Negate_float(_Property_c5de34ce4dd34257aa46153b38e65cf1_Out_0_Float, _Negate_2cdd1f9236ca4f99b473b6a143fab7c4_Out_1_Float);
            float4 _Combine_1ffde5e2bc8f46568c1f46429934ed69_RGBA_4_Vector4;
            float3 _Combine_1ffde5e2bc8f46568c1f46429934ed69_RGB_5_Vector3;
            float2 _Combine_1ffde5e2bc8f46568c1f46429934ed69_RG_6_Vector2;
            Unity_Combine_float(0, _Negate_2cdd1f9236ca4f99b473b6a143fab7c4_Out_1_Float, 0, 0, _Combine_1ffde5e2bc8f46568c1f46429934ed69_RGBA_4_Vector4, _Combine_1ffde5e2bc8f46568c1f46429934ed69_RGB_5_Vector3, _Combine_1ffde5e2bc8f46568c1f46429934ed69_RG_6_Vector2);
            float2 _TilingAndOffset_9c375bb7cbc84550854725f0dcd297f4_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (0.5, 0.5), _Combine_1ffde5e2bc8f46568c1f46429934ed69_RG_6_Vector2, _TilingAndOffset_9c375bb7cbc84550854725f0dcd297f4_Out_3_Vector2);
            float _Split_bdd629fda6994e0bbcb19b8d138454ad_R_1_Float = _TilingAndOffset_9c375bb7cbc84550854725f0dcd297f4_Out_3_Vector2[0];
            float _Split_bdd629fda6994e0bbcb19b8d138454ad_G_2_Float = _TilingAndOffset_9c375bb7cbc84550854725f0dcd297f4_Out_3_Vector2[1];
            float _Split_bdd629fda6994e0bbcb19b8d138454ad_B_3_Float = 0;
            float _Split_bdd629fda6994e0bbcb19b8d138454ad_A_4_Float = 0;
            float _Multiply_67baf8ac851a416f82a2fab271182c71_Out_2_Float;
            Unity_Multiply_float_float(_Split_bdd629fda6994e0bbcb19b8d138454ad_G_2_Float, _Minimum_d714df9c1ea343599c548bbfcfdb2070_Out_2_Float, _Multiply_67baf8ac851a416f82a2fab271182c71_Out_2_Float);
            float _Multiply_4b5faed5858b4ea0a6d9c334709834dc_Out_2_Float;
            Unity_Multiply_float_float(_Multiply_1b904e8515e4436784b052d5290249e9_Out_2_Float, _Multiply_67baf8ac851a416f82a2fab271182c71_Out_2_Float, _Multiply_4b5faed5858b4ea0a6d9c334709834dc_Out_2_Float);
            float _Clamp_dc90f4823ab64ea79075060fda77a52b_Out_3_Float;
            Unity_Clamp_float(_Multiply_4b5faed5858b4ea0a6d9c334709834dc_Out_2_Float, -1, 0, _Clamp_dc90f4823ab64ea79075060fda77a52b_Out_3_Float);
            float4 _Combine_a42b8c2d2b00478386645f2d832d6bb6_RGBA_4_Vector4;
            float3 _Combine_a42b8c2d2b00478386645f2d832d6bb6_RGB_5_Vector3;
            float2 _Combine_a42b8c2d2b00478386645f2d832d6bb6_RG_6_Vector2;
            Unity_Combine_float(0, _Clamp_dc90f4823ab64ea79075060fda77a52b_Out_3_Float, 0, 0, _Combine_a42b8c2d2b00478386645f2d832d6bb6_RGBA_4_Vector4, _Combine_a42b8c2d2b00478386645f2d832d6bb6_RGB_5_Vector3, _Combine_a42b8c2d2b00478386645f2d832d6bb6_RG_6_Vector2);
            float2 _TilingAndOffset_b00c802fe5e84288a8beeadd19fbd581_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Combine_a42b8c2d2b00478386645f2d832d6bb6_RG_6_Vector2, _TilingAndOffset_b00c802fe5e84288a8beeadd19fbd581_Out_3_Vector2);
            float4 _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_67337f168dfa479d9a883c4fafe2a198_Out_0_Texture2D.tex, _Property_67337f168dfa479d9a883c4fafe2a198_Out_0_Texture2D.samplerstate, _Property_67337f168dfa479d9a883c4fafe2a198_Out_0_Texture2D.GetTransformedUV(_TilingAndOffset_b00c802fe5e84288a8beeadd19fbd581_Out_3_Vector2) );
            float _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_R_4_Float = _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4.r;
            float _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_G_5_Float = _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4.g;
            float _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_B_6_Float = _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4.b;
            float _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_A_7_Float = _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4.a;
            float4 _Add_b218ccbc703e434493e6694dea00635d_Out_2_Vector4;
            Unity_Add_float4(_Maximum_fc7fb0fb31de44bf861c7b106de07979_Out_2_Vector4, _SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_RGBA_0_Vector4, _Add_b218ccbc703e434493e6694dea00635d_Out_2_Vector4);
            float4 _Add_2ab29bb60b7043d28f6bd161e52fcbd8_Out_2_Vector4;
            Unity_Add_float4(_Multiply_cf9d39ea36cf465790d5239828d629c2_Out_2_Vector4, _Add_b218ccbc703e434493e6694dea00635d_Out_2_Vector4, _Add_2ab29bb60b7043d28f6bd161e52fcbd8_Out_2_Vector4);
            float _Clamp_f3bb0624e5aa4a02923604f9c5839c27_Out_3_Float;
            Unity_Clamp_float(_Clamp_ff5668e2d3e2443ca20e0505031de19e_Out_3_Float, 0, 1, _Clamp_f3bb0624e5aa4a02923604f9c5839c27_Out_3_Float);
            float _Multiply_86658fa4e2ae43d7a5dc89ccf2a1e0f6_Out_2_Float;
            Unity_Multiply_float_float(_SampleTexture2D_94fd213484854bad93d7a6eedf761b6b_A_7_Float, _Clamp_f3bb0624e5aa4a02923604f9c5839c27_Out_3_Float, _Multiply_86658fa4e2ae43d7a5dc89ccf2a1e0f6_Out_2_Float);
            surface.BaseColor = (_Add_2ab29bb60b7043d28f6bd161e52fcbd8_Out_2_Vector4.xyz);
            surface.Alpha = _Multiply_86658fa4e2ae43d7a5dc89ccf2a1e0f6_Out_2_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}