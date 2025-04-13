using Player;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AnimeLinesPassFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class AnimeLinesPassFeatureSettings
    {
        public Material material;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    AnimeLinesPass m_ScriptablePass;
    public AnimeLinesPassFeatureSettings settings = new AnimeLinesPassFeatureSettings();

    /// <inheritdoc/>
    /// 
    public override void Create()
    {
        m_ScriptablePass = new AnimeLinesPass(settings);
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        //RenderTargetIdentifier source = renderer.cameraColorTarget;
        //m_ScriptablePass.Setup(source);
        var cam = renderingData.cameraData.camera;

        var player = cam.GetComponentInParent<PlayerMovement>();
        if (player != null)
        {
            float norm = Mathf.Clamp01(player.Speed / player.maxSpeed1);
            m_ScriptablePass.normalizedSpeed = norm;
        }

        renderer.EnqueuePass(m_ScriptablePass);
    }
}


