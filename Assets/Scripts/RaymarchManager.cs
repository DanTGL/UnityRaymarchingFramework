using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class RaymarchManager : MonoBehaviour {
    
    public ComputeShader computeShader;

    private RenderTexture target;

    private Camera cam;

    private int raymarchKernel;

    struct ObjectData {
        public int shape;
        public Vector3 position;
        public Vector3 size;
        public Vector3 rotation;
        public Color surfaceColor;
    }

    private List<ObjectData> objects;

    void Start() {
        cam = GetComponent<Camera>();
        if (computeShader != null) {
            raymarchKernel = computeShader.FindKernel("CSMain");
        }

        FindObjects();
    }

    void FindObjects() {
        objects = new List<ObjectData>();
        RaymarchShape[] foundObjs = FindObjectsOfType<RaymarchShape>();
        for (int i = 0; i < foundObjs.Length; i++) {
            ObjectData obj;
            obj.shape = foundObjs[i].GetShape();
            obj.position = foundObjs[i].GetPosition();
            obj.size = foundObjs[i].GetSize();
            obj.rotation = foundObjs[i].GetRotation();
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

        ComputeBuffer buffer = new ComputeBuffer(objects.Count, 56);
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
