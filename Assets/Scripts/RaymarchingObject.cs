using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaymarchingObject : MonoBehaviour {

    enum ShapeType {
        NONE,
        SPHERE,
        CUBE
    }

    [SerializeField]
    private ShapeType shape = ShapeType.NONE;

    [SerializeField]
    private float radius = 1f;

    [SerializeField]
    private Color surfaceColor = Color.white;

    public Vector3 GetPosition() {
        return transform.position;
    }

    public int GetShape() {
        return (int) shape;
    }

    public float GetRadius() {
        return radius;
    }

    public Color GetSurfaceColor() {
        return surfaceColor;
    }

    public Vector3 GetScale() {
        return transform.localScale;
    }

}
