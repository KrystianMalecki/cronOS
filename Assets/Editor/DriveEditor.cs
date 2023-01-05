using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(Drive))]
[CanEditMultipleObjects]
public class DriveEditor : Editor
{
    SerializedProperty freeSpaces;
    SerializedProperty fakeListProperty;
    public List<int> fakeList;
    SerializedObject editorSO;

    void OnEnable()
    {
        editorSO = new SerializedObject(this, serializedObject.targetObject);

        freeSpaces = serializedObject.FindProperty("freeSpaces");
        fakeListProperty = editorSO.FindProperty("fakeList");

        Debug.Log($"{editorSO}-{freeSpaces}-{fakeList}-{fakeListProperty}-{fakeListProperty.editable}");
    }

    private void Awake()
    {
        OnEnable();
    }

    public override void OnInspectorGUI()
    {
        //  serializedObject.Update();
        GUI.enabled = true;
        EditorGUILayout.PropertyField(fakeListProperty);
        //  serializedObject.ApplyModifiedProperties();
    }
}