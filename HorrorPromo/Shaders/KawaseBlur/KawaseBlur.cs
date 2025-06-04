using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class KawaseBlur : ScriptableRendererFeature
{
    [System.Serializable]
    public class KawaseBlurSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        public Material blurMaterial = null;

        [Range(2, 15)]
        public int blurPasses = 1;

        [Range(1, 4)]
        public int downsample = 1;
        public bool copyToFramebuffer;
        public string targetName = "_blurTexture";
    }

    public KawaseBlurSettings settings = new KawaseBlurSettings();

    class CustomRenderPass : ScriptableRenderPass
    {
        public Material blurMaterial;
        public int passes;
        public int downsample;
        public bool copyToFramebuffer;
        public string targetName;
        string profilerTag;

        int tmpId1;
        int tmpId2;

        RenderTargetIdentifier tmpRT1;
        RenderTargetIdentifier tmpRT2;

        public CustomRenderPass(string profilerTag)
        {
            this.profilerTag = profilerTag;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            // Используем дескриптор камеры для создания временных текстур
            RenderTextureDescriptor descriptor = cameraTextureDescriptor;
            descriptor.width /= downsample;
            descriptor.height /= downsample;
            descriptor.depthBufferBits = 0;
            descriptor.msaaSamples = 1;

            tmpId1 = Shader.PropertyToID("tmpBlurRT1");
            tmpId2 = Shader.PropertyToID("tmpBlurRT2");

            cmd.GetTemporaryRT(tmpId1, descriptor, FilterMode.Bilinear);
            cmd.GetTemporaryRT(tmpId2, descriptor, FilterMode.Bilinear);

            tmpRT1 = new RenderTargetIdentifier(tmpId1);
            tmpRT2 = new RenderTargetIdentifier(tmpId2);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            // Получаем cameraColorTarget внутри Execute - это единственное безопасное место!
            var source = renderingData.cameraData.renderer.cameraColorTarget;

            if (blurMaterial == null)
            {
                Debug.LogError("Blur material is missing");
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get(profilerTag);

            using (new ProfilingScope(cmd, new ProfilingSampler(profilerTag)))
            {
                RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
                opaqueDesc.depthBufferBits = 0;

                // Первый проход
                cmd.SetGlobalFloat("_offset", 1.5f);
                cmd.Blit(source, tmpRT1, blurMaterial);

                // Промежуточные проходы
                for (var i = 1; i < passes - 1; i++)
                {
                    cmd.SetGlobalFloat("_offset", 0.5f + i);
                    cmd.Blit(tmpRT1, tmpRT2, blurMaterial);

                    // Обмен текстурами
                    (tmpRT1, tmpRT2) = (tmpRT2, tmpRT1);
                }

                // Финальный проход
                cmd.SetGlobalFloat("_offset", 0.5f + passes - 1f);
                if (copyToFramebuffer)
                {
                    cmd.Blit(tmpRT1, source, blurMaterial);
                }
                else
                {
                    cmd.Blit(tmpRT1, tmpRT2, blurMaterial);
                    cmd.SetGlobalTexture(targetName, tmpRT2);
                }
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            // Освобождаем временные текстуры
            if (tmpId1 != 0)
            {
                cmd.ReleaseTemporaryRT(tmpId1);
                tmpId1 = 0;
            }

            if (tmpId2 != 0)
            {
                cmd.ReleaseTemporaryRT(tmpId2);
                tmpId2 = 0;
            }
        }
    }

    CustomRenderPass scriptablePass;

    public override void Create()
    {
        scriptablePass = new CustomRenderPass("KawaseBlur");
        scriptablePass.blurMaterial = settings.blurMaterial;
        scriptablePass.passes = settings.blurPasses;
        scriptablePass.downsample = settings.downsample;
        scriptablePass.copyToFramebuffer = settings.copyToFramebuffer;
        scriptablePass.targetName = settings.targetName;
        scriptablePass.renderPassEvent = settings.renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // Просто добавляем пасс без доступа к cameraColorTarget
        renderer.EnqueuePass(scriptablePass);
    }
}


