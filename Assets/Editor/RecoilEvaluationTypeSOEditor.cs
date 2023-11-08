using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RecoilEvaluationTypeSO))]
public class RecoilEvaluationTypeSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RecoilEvaluationTypeSO recoilType = (RecoilEvaluationTypeSO)target;

        EditorGUILayout.Space(30);

        if (GUILayout.Button("Apply"))
        {
            recoilType.Apply();
        }
    }
}
