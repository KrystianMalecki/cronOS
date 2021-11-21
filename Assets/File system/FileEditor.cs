using Libraries.system.file_system;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FileEditor : EditorWindow
{
    public static FileEditor currentWindow;

    public File currentFile = null;
    public SerializedProperty currentFileSP = null;
    public SerializedObject currentFileSO = null;
    //  [SerializeReference]

    //  public File lastOne = null;

    public static void DisplayCurrentFile(File file, SerializedProperty serializedProperty, SerializedObject serializedObject = null)
    {
        if (currentWindow == null)
        {
            ShowWindow(file, serializedProperty, serializedObject);
        }
        else
        {
            currentWindow.currentFileSP = serializedProperty;
            currentWindow.ChangeCurrentFile(file);
        }
    }
    public static void ShowWindow(File file, SerializedProperty serializedProperty, SerializedObject serializedObject = null)
    {

        currentWindow = EditorWindow.GetWindow<FileEditor>(typeof(FileEditor));

        currentWindow.currentFileSO = serializedObject;
        currentWindow.currentFileSP = serializedProperty;

        currentWindow.ChangeCurrentFile(file);





    }
    public void ChangeCurrentFile(File file)
    {
        if (currentFile != file && currentFile != null)
        {
            //   lastOne = currentFile;
        }
        currentFile = file;

        currentFile.OnValidate();
        toggleTopFields = true;
        SetValues();
        dataViewScrollPos = Vector2.zero;
        textScrollPos = Vector2.zero;
        UpdateWindow();
    }
    public void SetValues()
    {
        TryFixEditorValues();

        permissionsSP = currentFileSP.FindPropertyRelative("permissions");

        childrenSP = currentFileSP.FindPropertyRelative("children");

        dataSP = currentFileSP.FindPropertyRelative("data");

        dataArraySize = currentFile.GetDataArraySize();

    }
    SerializedProperty FindPropertyOfFile(File f)
    {
        SerializedProperty buffer;
        if (currentFileSO.targetObject.GetType() == typeof(DriveSO))
        {
            buffer = currentFileSO.FindProperty("root");
        }
        else
        {
            buffer = currentFileSO.FindProperty("currentFile");
        }
        Debug.Log($"{f} {buffer} ");
        Path p = new Path(f.GetFullPath(), buffer.GetTargetObjectOfProperty() as File);
        for (int i = 1; i < p.fileparts.Count; i++)
        {
            Debug.Log(p.fileparts[i]);
            int pos = p.fileparts[i - 1].children.FindIndex(x => x == p.fileparts[i]);
            buffer = buffer.FindPropertyRelative($"children.items.Array.data[{pos}]");

        }
        return buffer;
    }
    void TryFixEditorValues()
    {
        if (currentFileSO == null)
        {
            Debug.LogWarning("Something went wrong with currentFileSO!");
            currentFileSO = new SerializedObject(currentWindow);
        }

        if (currentFileSP == null)
        {
            Debug.LogWarning("Something went wrong with currentFileSP!");
            if (currentFileSO.targetObject != null)
            {
                if (currentFileSO.targetObject.GetType() == typeof(DriveSO))
                {
                    currentFileSP = currentFileSO.FindProperty("root");

                }
                else
                {
                    currentFileSP = currentFileSO.FindProperty("currentFile");

                }
            }
        }
    }
    int size = 32;
    int page = 0;
    SerializedProperty permissionsSP;
    SerializedProperty childrenSP;
    SerializedProperty dataSP;


    Vector2 dataViewScrollPos = Vector2.zero;
    Vector2 textScrollPos = Vector2.zero;

    int dataArraySize = 0;
    int dataSelectedPos = 0;
    int dataSelectedValue = 0;

    string dataAsString = "";


    float windowWidth = 0;
    float boxWidth = 28;

    bool toggleTopFields = true;
    bool toggleBottomPart = true;
    bool toggleData = false;

    bool toggleTextDataField = false;
    void OnGUI()
    {

        if (currentFile == null || currentWindow == null)
        {
            Debug.LogWarning($"Something went wrong with currentFile{currentFile} or currentWindow{currentWindow}!");

            this.Close();
            return;
        }
        TryFixEditorValues();
        if (currentFile == null || currentFileSO == null || currentFileSP == null || currentWindow == null)
        {
            Debug.LogWarning($"Something went wrong with currentFile{currentFile} or currentFileSO{currentFileSO} or currentFileSP{currentFileSP} or currentWindow{currentWindow}!");

            this.Close();
            return;
        }

        windowWidth = position.width - 10;

        EditorGUI.BeginChangeCheck();

        toggleTopFields = (EditorGUILayout.BeginFoldoutHeaderGroup(toggleTopFields, "Main values"));

        if (toggleTopFields)
        {
            DrawTopGoersBar();

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Width(windowWidth));
            //name
            GUILayout.Label("Name: ", GUILayout.Width(50));
            string before = currentFile.name;
            currentFile.name = GUILayout.TextField(currentFile.name, GUILayout.ExpandWidth(true));
            if (before != currentFile.name)
            {
                currentFileSO.Update();
            }
            GUILayout.EndHorizontal();


            DrawButtonPath();




            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            //permissions
            GUILayout.Label("Permissions: ");

            EditorGUILayout.PropertyField(permissionsSP);
            // currentFileSO.Update();

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            GUI.enabled = currentFile.parent != null;
            if (GUILayout.Button("Delete file"))
            {
                if (EditorUtility.DisplayDialog("Delete file?",
                $"Are you sure want to delete {currentFile.name}?", "Yes", "No"))
                {
                    File parent = currentFile.parent;
                    parent.RemoveChild(currentFile);
                    currentFileSO.Update();
                    DisplayCurrentFile(parent, FindPropertyOfFile(parent), currentFileSO);

                }
            }
            GUI.enabled = true;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        GUILayout.Space(EditorGUIUtility.singleLineHeight);

        //children
        EditorGUILayout.PropertyField(childrenSP);
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Width(windowWidth));
        GUILayout.EndHorizontal();

        //  GUILayout.Space(EditorGUIUtility.singleLineHeight);

        //clampting

        /*  toggleBottomPart = (EditorGUILayout.BeginFoldoutHeaderGroup(toggleBottomPart, "Data"));
          if (toggleBottomPart)
          {*/



     



        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        // array
        GUILayout.BeginVertical(GUILayout.ExpandHeight(true));

        toggleData = (EditorGUILayout.BeginFoldoutHeaderGroup(toggleData, "Byte Data"));

        if (toggleData)
        {
            DrawPagePickers();

            DrawSpecialControls();
            DrawDataArray();
        }
        GUILayout.EndVertical();
        EditorGUILayout.EndFoldoutHeaderGroup();

        GUILayout.BeginVertical(GUILayout.ExpandHeight(true));

        toggleTextDataField = (EditorGUILayout.BeginFoldoutHeaderGroup(toggleTextDataField, "Text Data"));

        if (toggleTextDataField)
        {
            DrawTextData();
        }
        GUILayout.EndVertical();

        EditorGUILayout.EndFoldoutHeaderGroup();

        GUILayout.EndHorizontal();
        /*  }
          EditorGUILayout.EndFoldoutHeaderGroup();*/


        //save all
        if (EditorGUI.EndChangeCheck())
        {
            currentFileSO.ApplyModifiedProperties();

        }

        //   currentFileSP.serializedObject.Update();
    }
    public void UpdateWindow()
    {
        currentFileSO.ApplyModifiedProperties();
        SetValues();
        dataAsString = null;
        EditorUtility.SetDirty(currentWindow);

        EditorUtility.SetDirty(currentFileSO.targetObject);
        Repaint();
    }
    void DrawButtonPath()
    {
        string path = currentFile.GetFullPath();
        string[] pathParts = path.Split('/');
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Width(windowWidth));

        GUILayout.Label("Full path: " + path, GUILayout.ExpandWidth(false));
        if (GUILayout.Button("Copy path", GUILayout.Width(90)))
        {
            EditorGUIUtility.systemCopyBuffer = path;

        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Width(windowWidth));
        File father = currentFile;
        List<File> files = new List<File>();
        files.Add(father);
        while (father.parent != null)
        {
            father = father.parent;
            files.Add(father);
        }
        //todo 0 just use Path()
        files.Reverse();
        /*  File lastOneFile = father;
          DrawPathButton(father);
          for (int i = 1; i < pathParts.Length; i++)
          {
              files.Insert(i, lastOneFile.GetChildByName(pathParts[i]));
              DrawPathButton(files[i]);

              lastOneFile = files[i];
          }*/
        for (int i = 0; i < files.Count; i++)
        {
            DrawPathButton(files[i]);
        }

        GUILayout.EndHorizontal();
    }
    void DrawTopGoersBar()
    {
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Width(windowWidth));

        /*  GUI.enabled = lastOne != null && lastOne != currentFile;
          string lastName = lastOne == null ? "" : (lastOne.name);
          if (GUILayout.Button("Go back to last one: " + lastName))
          {
              FileEditor.DisplayCurrentFile(lastOne, FindPropertyOfFile(lastOne), currentFileSO);
          }
          GUILayout.Space(80);*/

        /* GUI.enabled = currentFile.parent != null;
         string parentName = currentFile.parent == null ? "" : (currentFile.parent.name);
         if (GUILayout.Button("Go to parent: " + parentName))
         {
             FileEditor.DisplayCurrentFile(currentFile.parent, FindPropertyOfFile(currentFile.parent), currentFileSO);
         }

         GUI.enabled = true;*/
        GUILayout.EndHorizontal();
    }
    void DrawSpecialControls()
    {
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

        GUILayout.Space(40);

        GUILayout.Label("Pos:", GUILayout.Width(40));

        dataSelectedPos = EditorGUILayout.IntField(GUIContent.none, dataSelectedPos, GUILayout.Width(40), GUILayout.ExpandWidth(false));

        GUILayout.Label("Item:", GUILayout.Width(40));

        dataSelectedValue = EditorGUILayout.IntField(GUIContent.none, dataSelectedValue, GUILayout.Width(40), GUILayout.ExpandWidth(false));
        GUI.enabled = (dataSelectedPos > -1 && dataSelectedPos < currentFile.GetDataArraySize() + 1) && (dataSelectedValue > -1 && dataSelectedValue < 256);
        if (GUILayout.Button("Insert", GUILayout.Width(60)))
        {
            ArrayUtility.Insert(ref currentFile.data, dataSelectedPos, (byte)dataSelectedValue);
            currentFileSO.Update();
            dataAsString = null;

            UpdateWindow();
        }
        var typeRect = GUILayoutUtility.GetLastRect();
        GUI.Label(typeRect, new GUIContent("", "Insert before selected byte"));

        GUI.enabled = (dataSelectedPos > -1 && dataSelectedPos < currentFile.GetDataArraySize()) && (dataSelectedValue > -1 && dataSelectedValue < 256);

        if (GUILayout.Button("Set", GUILayout.Width(60)))
        {
            currentFile.data[dataSelectedPos] = (byte)dataSelectedValue;
            currentFileSO.Update();
            dataAsString = null;
            UpdateWindow();
        }
        if (GUILayout.Button("Remove", GUILayout.Width(60)))
        {
            ArrayUtility.RemoveAt(ref currentFile.data, dataSelectedPos);
            currentFileSO.Update();
            dataAsString = null;
            UpdateWindow();
        }
        GUI.enabled = true;
        GUILayout.Label("Length:", GUILayout.Width(70));

        dataArraySize = EditorGUILayout.IntField(GUIContent.none, dataArraySize, GUILayout.Width(60), GUILayout.ExpandWidth(false));

        if (GUILayout.Button("Resize", GUILayout.Width(70)))
        {
            Array.Resize(ref currentFile.data, dataArraySize);
            currentFileSO.Update();
            UpdateWindow();
        }
        if (GUILayout.Button("Copy", GUILayout.Width(70)))
        {
            EditorGUIUtility.systemCopyBuffer = JsonUtility.ToJson(new ByteArray(currentFile.data));
        }
        if (GUILayout.Button("Paste", GUILayout.Width(70)))
        {
            try
            {
                ByteArray ba = JsonUtility.FromJson<ByteArray>(EditorGUIUtility.systemCopyBuffer);
                currentFile.data = ba.array;
                currentFileSO.Update();
                UpdateWindow();
            }
            catch (Exception e)
            {
                Debug.LogError("Error when pasting byte array:" + e.Message);
            }


        }
        GUILayout.Label(" ", GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Toggle text data", GUILayout.Width(150)))
        {
            toggleTextDataField = !toggleTextDataField;
        }

        GUILayout.EndHorizontal();
    }
    void DrawTextData()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
        GUILayout.Label("As Text Input:", GUILayout.Width(90));
        if (string.IsNullOrEmpty(dataAsString))
        {
            dataAsString = currentFile.data.ToEncodedString();
        }
        textScrollPos = EditorGUILayout.BeginScrollView(textScrollPos, GUILayout.ExpandHeight(true));
        //  GUILayout.BeginVertical(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        dataAsString = EditorGUILayout.TextArea(dataAsString, GUILayout.MinWidth(100), GUILayout.ExpandHeight(true)/*, GUILayout.MinHeight(50)*/);
        //  GUILayout.EndVertical();

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Replace Data", GUILayout.ExpandWidth(true)))
        {
            currentFile.data = dataAsString.ToBytes();
            currentFileSO.Update();
            UpdateWindow();
        }

        GUILayout.EndVertical();

    }
    void DrawDataArray()
    {
        int start = page * size * size;
        int length = dataSP.arraySize;
        if (length > size * size)
        {
            length = size * size;
        }
        dataViewScrollPos = GUILayout.BeginScrollView(dataViewScrollPos);

        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false), GUILayout.Width(100));
        int indexChanged = dataSelectedPos;
        EditorGUI.BeginChangeCheck();
        for (int i = start; i < start + length; i++)
        {
            if (currentFile.data.Length <= i)
            {
                break;
            }
            if (i % size == 0)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true)/*, GUILayout.Width(windowWidth)*/);

            }
            //  EditorGUILayout.PropertyField(GetDataAt(i), GUIContent.none);
            if (i == dataSelectedPos)
            {
                GUI.backgroundColor = Color.black;
            }
            int input = EditorGUILayout.IntField(GUIContent.none, currentFile.data[i], GUILayout.Width(boxWidth), GUILayout.MinWidth(boxWidth));
            if (input != currentFile.data[i])
            {
                if (input > 255)
                {
                    input = 255;
                }
                if (input < 0)
                {
                    input = 0;
                }
                currentFile.data[i] = (byte)input;

                indexChanged = i;
            }
            GUI.backgroundColor = Color.white;

            var typeRect = GUILayoutUtility.GetLastRect();
            GUI.Label(typeRect, new GUIContent("", "byte " + i));
        }

        if (EditorGUI.EndChangeCheck())
        {
            currentFileSO.Update();

            dataAsString = null;
            dataSelectedPos = indexChanged;
        }

        GUILayout.EndHorizontal();

        GUILayout.EndScrollView();
    }
    void DrawPagePickers()
    {
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        GUI.enabled = page > 0;
        if (GUILayout.Button("Previous Page"))
        {
            page--;
            dataViewScrollPos = Vector2.zero;
        }
        GUI.enabled = true;
        GUILayout.Label(" ", GUILayout.ExpandWidth(true));

        GUILayout.Label("Page: " + page, GUILayout.Width(60));

        GUILayout.Label(" ", GUILayout.ExpandWidth(true));

        GUI.enabled = ((page + 1) * size * size <= dataSP.arraySize);
        if (GUILayout.Button("Next Page"))
        {
            page++;
            dataViewScrollPos = Vector2.zero;
        }
        GUI.enabled = true;

        GUILayout.EndHorizontal();
    }
    void DrawPathButton(File file)
    {
        if (file == null)
        {
            return;
        }
        GUI.enabled = file.name != currentFile.name;
        if (GUILayout.Button(file.name, GUILayout.MinWidth(1), GUILayout.ExpandWidth(false)))
        {
            FileEditor.DisplayCurrentFile(file, FindPropertyOfFile(file), currentFileSO);
        }
        GUI.enabled = true;
        GUILayout.Label("/", GUILayout.MinWidth(1), GUILayout.ExpandWidth(false));
    }
    [Serializable]
    private class ByteArray
    {
        [SerializeField]
        public byte[] array;
        public ByteArray(byte[] array)
        {
            this.array = array;
        }
    }
}


