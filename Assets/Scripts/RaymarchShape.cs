using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaymarchShape : MonoBehaviour {

    enum ShapeType {
        NONE,
        SPHERE,
        CUBE,
        CYLINDER
    }

    [SerializeField]
    private ShapeType shape = ShapeType.NONE;

    [SerializeField]
    private Vector3 size = Vector3.one;

    [SerializeField]
    private Color surfaceColor = Color.white;

    public Vector3 GetPosition() {
        return transform.position;
    }

    public int GetShape() {
        return (int) shape;
    }

    public Color GetSurfaceColor() {
        return surfaceColor;
    }

    public Vector3 GetSize() {
        return Vector3.Scale(size, transform.localScale);
    }

    public Vector3 GetRotation() {
        return transform.rotation.eulerAngles * Mathf.Deg2Rad;
    }

}
