    using System;
    using Player;
    using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AnimeLinesPass : ScriptableRenderPass
{
    RenderTargetIdentifier source;
    RenderTargetHandle destination;
    private PlayerMovement player;
    private GameObject camera;
    
    public float normalizedSpeed = 0f;

    AnimeLinesPassFeature.AnimeLinesPassFeatureSettings settings;

    public AnimeLinesPass(AnimeLinesPassFeature.AnimeLinesPassFeatureSettings settings)
    {
        this.settings = settings; 
        renderPassEvent = settings.renderPassEvent;
    }

    // This method is called before executing the render pass.
    // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
    // When empty this render pass will render to the active camera render target.
    // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
    // The render pipeline will ensure target setup and clearing happens in a performant manner.

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        source = renderingData.cameraData.renderer.cameraColorTarget;
        cmd.GetTemporaryRT(destination.id, renderingData.cameraData.cameraTargetDescriptor);
        
        //camera = renderingData.cameraData.camera.gameObject;
        //player = obj.GetComponentInParent<PlayerMovement>();
    }

    // Here you can implement the rendering logic.
    // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
    // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
    // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();

        settings.material.SetFloat("_SpeedLinesRemap", Mathf.Lerp(1f, 0.9f, normalizedSpeed));
        
        cmd.Blit(source, destination.Identifier(), settings.material, 0);
        cmd.Blit(destination.Identifier(), source);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    // Cleanup any allocated resources that were created during the execution of this render pass.

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(destination.id);
    }
}


