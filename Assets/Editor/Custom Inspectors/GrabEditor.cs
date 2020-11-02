using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Grab))]
public class GrabEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Grab accessory = (Grab)target;
        if (GUILayout.Button("Force Equip"))
            FindObjectOfType<Example>().EquipAccessoryWithForce(accessory);
    }
}
