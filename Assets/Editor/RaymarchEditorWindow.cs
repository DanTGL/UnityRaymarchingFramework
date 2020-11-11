using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RaymarchEditorWindow : Editor {

    private int controlIdHash = "RaymarchEditorWindow".GetHashCode();

    void OnSceneGUI() {
        int controlId = EditorGUIUtility.GetControlID(controlIdHash, FocusType.Keyboard);

        Event e = Event.current;
        switch (e.type) {
            case EventType.MouseDown:
                if (HandleUtility.nearestControl == controlId && e.button == 0) {
                	GUIUtility.hotControl = controlId;
                	GUIUtility.keyboardControl = controlId;
                	e.Use();
                }

                break;
            case EventType.MouseUp:
                if (GUIUtility.hotControl == controlId && e.button == 0) {
                    GUIUtility.hotControl = 0;
                    e.Use();
                }

                break;
            case EventType.MouseDrag:
                break;
            case EventType.Repaint:
                break;
            case EventType.Layout:
                Matrix4x4 matrix = Handles.matrix.SetTRS(HandleUtility.CalcLineTranslation();

                Vector3 pointWorldPos = matrix.MultiplyPoint3x4(position)

                break;
        }
    }
}
