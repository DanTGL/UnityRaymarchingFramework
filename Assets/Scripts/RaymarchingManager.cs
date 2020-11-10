using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class RaymarchingManager : MonoBehaviour {

    public ComputeShader computeShader;

    private RenderTexture target;

    private Camera cam;

    private int raymarchKernel;

    struct RaymarchObject {
        public int shape;
        public float radius;
        public Vector3 position;
        public Vector3 scale;
        public Color surfaceColor;
    }

    private List<RaymarchObject> objects;

    void Start() {
        cam = GetComponent<Camera>();
        if (computeShader != null) {
            raymarchKernel = computeShader.FindKernel("CSMain");
        }

        FindObjects();
    }

    void FindObjects() {
        objects = new List<RaymarchObject>();
        RaymarchingObject[] foundObjs = FindObjectsOfType<RaymarchingObject>();
        for (int i = 0; i < foundObjs.Length; i++) {
            RaymarchObject obj;
            obj.shape = foundObjs[i].GetShape();
            obj.radius = foundObjs[i].GetRadius();
            obj.position = foundObjs[i].GetPosition();
            obj.scale = foundObjs[i].GetScale();
            obj.surfaceColor = foundObjs[i].GetSurfaceColor();
            objects.Add(obj);
        }
        
        
    }

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture source, RenderTexture dest) {
        if (computeShader == null) {
            Graphics.Blit(source, dest);
            return;
        }
        
        InitRenderTexture();

        FindObjects();

        ComputeBuffer buffer = new ComputeBuffer(objects.Count, 48);
        buffer.SetData(objects.ToArray());

        computeShader.SetMatrix("_CameraToWorld", cam.cameraToWorldMatrix);
        computeShader.SetMatrix("_CameraInverseProjection", cam.projectionMatrix.inverse);
        computeShader.SetInt("_numShapes", objects.Count);
        computeShader.SetBuffer(raymarchKernel, "dataBuffer", buffer);
        //computeShader.SetTexture(raymarchKernel, "_SkyboxTexture", RenderSettings.skybox.mainTexture);
        computeShader.SetTexture(raymarchKernel, "Result", target);
        computeShader.Dispatch(raymarchKernel, cam.pixelWidth / 8, cam.pixelHeight / 8, 1);
        buffer.Release();

        Graphics.Blit(target, dest);
    }

    void InitRenderTexture() {
        if (target == null || target.width != Screen.width || target.height != Screen.height) {

            if (target != null) {
                target.Release();
            }

            target = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            target.enableRandomWrite = true;
            target.Create();
        }
    }

}
