using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(RoinStatesKit))]
public class StatesKitEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Save State"))
        {
            FindObjectOfType<RoinPartsAndStates>().WriteValuesToStatesKit((RoinStatesKit)target);
        }
    }
}
