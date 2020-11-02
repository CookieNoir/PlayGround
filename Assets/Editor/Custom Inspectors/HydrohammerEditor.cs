using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(HydraulicHammer))]
public class HydrohammerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        HydraulicHammer accessory = (HydraulicHammer)target;
        if (GUILayout.Button("Force Equip"))
            FindObjectOfType<Example>().EquipAccessoryWithForce(accessory);
    }
}
