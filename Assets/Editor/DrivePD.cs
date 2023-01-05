using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DriveSO))]
public class DrivePD : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }

    Rect rect;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.width /= 3;
        EditorGUI.PropertyField(position, property, GUIContent.none);


        DriveSO mainObject = ((DriveSO)property.GetTargetObjectOfProperty());
        if (mainObject != null)
        {
            position.x += position.width;
            if (GUI.Button(position, "Open Editor"))
            {
                mainObject.GenerateCacheData();
                mainObject.OpenEditor();
            }

            position.x += position.width;

            if (GUI.Button(position, "Generate parent links"))
            {
                mainObject.GenerateCacheData();
            }
        }
        else
        {
            position.x += position.width;
            GUI.Box(position, "DriveSO is null");
            Debug.Log("DriveSO is null");
        }
    }
}