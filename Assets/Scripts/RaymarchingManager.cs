using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ImageEffectAllowedInSceneView]
public class RaymarchingManager : MonoBehaviour {

    public ComputeShader computeShader;

    private RenderTexture target;

    private Camera cam;

    private int raymarchKernel;

    void Start() {
        cam = GetComponent<Camera>();
        if (computeShader != null) {
            raymarchKernel = computeShader.FindKernel("CSMain");
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture dest) {
        if (computeShader == null) {
            Graphics.Blit(source, dest);
            return;
        }

        if (target != null) {
            target.Release();
        }

        target = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0);
        target.enableRandomWrite = true;
        target.Create();

        computeShader.SetTexture(raymarchKernel, "Result", target);
        computeShader.Dispatch(raymarchKernel, cam.pixelWidth / 8, cam.pixelHeight / 8, 1);

        Graphics.Blit(target, dest);
    }

}
