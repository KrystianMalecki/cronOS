using Libraries.system.file_system;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FileEditor : EditorWindow
{
    public static FileEditor currentWindow;
    [SerializeReference]
    public File currentFile = null;
    [SerializeReference]
    public SerializedProperty currentFileSP;
    [SerializeField]
    public SerializedObject currentFileSO;
    [SerializeReference]

    public File lastOne;


    public static void ShowWindow(File file/*, SerializedProperty fileSP, SerializedObject fileSO*/)
    {
        currentWindow = EditorWindow.GetWindow<FileEditor>(typeof(FileEditor));
        if (currentWindow.currentFile != file && currentWindow.currentFile != null)
        {
            currentWindow.lastOne = currentWindow.currentFile;
        }
        file.OnValidate(); //toto warning! can be expensive
        currentWindow.currentFile = file;

        currentWindow.SetValues();

        /*   currentWindow.currentFileSP = fileSP;
           currentWindow.currentFileSO = fileSO;*/
        currentWindow.currentFileSO = new SerializedObject(currentWindow);
        currentWindow.currentFileSP = currentWindow.currentFileSO.FindProperty("currentFile");
        //  currentWindow.dataProperities = new Dictionary<int, SerializedProperty>();
        currentWindow.dataViewScrollPosition = Vector2.zero;


    }
    public void SetValues()
    {
        currentWindow._dataSP = null;
        currentWindow._childrenSP = null;
        currentWindow._dataSP = null;
        currentWindow.dataArraySize = currentWindow.currentFile.data.Length;
    }
    int size = 32;
    int page = 0;
    SerializedProperty _permissionsSP;
    SerializedProperty _childrenSP;
    SerializedProperty _dataSP;
    SerializedProperty permissionsSP
    {
        get
        {
            if (_permissionsSP == null)
            {
                _permissionsSP = currentFileSP.FindPropertyRelative("permissions");
            }
            return _permissionsSP;
        }
    }
    SerializedProperty childrenSP
    {
        get
        {
            if (_childrenSP == null)
            {
                _childrenSP = currentFileSP.FindPropertyRelative("children");
            }
            return _childrenSP;
        }
    }
    SerializedProperty dataSP
    {
        get
        {
            if (_dataSP == null)
            {
                _dataSP = currentFileSP.FindPropertyRelative("data");
            }
            return _dataSP;
        }
    }

    Vector2 dataViewScrollPosition = Vector2.zero;

    int dataArraySize = 0;
    int dataSelectedPos = 0;
    int dataSelectedValue = 0;

    string dataAsString = "";


    float windowWidth = 0;
    float boxWidth = 28;

    bool toggleTopFields = true;
    bool toggleData = true;

    bool toggleTextDataField = false;
    void OnGUI()
    {
        if (currentFile == null || currentFileSO == null || currentFileSP == null || currentWindow == null)
        {
            this.Close();
            return;
        }


        windowWidth = position.width - 10;
        //  boxWidth = Mathf.Min((windowWidth - 20) / (size + 1), 40);

        EditorGUI.BeginChangeCheck();

        toggleTopFields = (EditorGUILayout.BeginFoldoutHeaderGroup(toggleTopFields, "Main values"));
        if (toggleTopFields)
        {
            DrawTopGoersBar();

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Width(windowWidth));
            //name
            GUILayout.Label("Name: ", GUILayout.Width(50));

            currentFile.name = GUILayout.TextField(currentFile.name, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();


            DrawButtonPath();




            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            //permissions
            GUILayout.Label("Permissions: ");
            EditorGUILayout.PropertyField(permissionsSP);

        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        GUILayout.Space(EditorGUIUtility.singleLineHeight);

        //children
        EditorGUILayout.PropertyField(childrenSP);
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Width(windowWidth));
        GUILayout.EndHorizontal();

        //  GUILayout.Space(EditorGUIUtility.singleLineHeight);

        //clampting
        GUILayout.Space(EditorGUIUtility.singleLineHeight);

        toggleData = (EditorGUILayout.BeginFoldoutHeaderGroup(toggleData, "Data"));
        if (toggleData)
        {


            DrawPagePickers();
            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            DrawSpecialControls();



            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            // array

            DrawDataArray();
            if (toggleTextDataField)
            {
                DrawTextData();
            }
            GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();


        //save all
        /*if (EditorGUI.EndChangeCheck())
        {
            currentFileSO.ApplyModifiedProperties();

        }*/

        //   currentFileSP.serializedObject.Update();
    }
    public void UpdateWindow()
    {
        currentFileSO.Update();
        SetValues();
        dataAsString = null;
    }
    void DrawButtonPath()
    {
        string path = currentFile.GetFullPath();
        string[] pathParts = path.Split('/');

        GUILayout.Label("Full path: " + path, GUILayout.ExpandWidth(true));
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Width(windowWidth));
        File father = currentFile;
        while (father.parent != null)
        {
            father = father.parent;
        }
        List<File> files = new List<File>();
        files.Insert(0, father);

        File lastOneFile = father;
        DrawPathButton(father);
        for (int i = 1; i < pathParts.Length; i++)
        {
            files.Insert(i, lastOneFile.GetChildByName(pathParts[i]));
            DrawPathButton(files[i]);

            lastOneFile = files[i];
        }


        GUILayout.EndHorizontal();
    }
    void DrawTopGoersBar()
    {
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Width(windowWidth));

        GUI.enabled = lastOne != null && lastOne != currentFile;
        string lastName = lastOne == null ? "" : (lastOne.name);
        if (GUILayout.Button("Go back to last one: " + lastName))
        {
            FileEditor.ShowWindow(lastOne);
        }
        GUILayout.Space(80);

        GUI.enabled = currentFile.parent != null;
        string parentName = currentFile.parent == null ? "" : (currentFile.parent.name);
        if (GUILayout.Button("Go to parent: " + parentName))
        {
            FileEditor.ShowWindow(currentFile.parent);
        }

        GUI.enabled = true;
        GUILayout.EndHorizontal();
    }
    void DrawSpecialControls()
    {
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Width(windowWidth));

        GUILayout.Space(40);

        GUILayout.Label("Pos:", GUILayout.Width(40));

        dataSelectedPos = EditorGUILayout.IntField(GUIContent.none, dataSelectedPos, GUILayout.Width(40), GUILayout.ExpandWidth(false));

        GUILayout.Label("Item:", GUILayout.Width(40));

        dataSelectedValue = EditorGUILayout.IntField(GUIContent.none, dataSelectedValue, GUILayout.Width(40), GUILayout.ExpandWidth(false));
        GUI.enabled = (dataSelectedPos > -1 && dataSelectedPos < currentFile.data.Length + 1) && (dataSelectedValue > -1 && dataSelectedValue < 256);
        if (GUILayout.Button("Insert", GUILayout.Width(60)))
        {
            ArrayUtility.Insert(ref currentFile.data, dataSelectedPos, (byte)dataSelectedValue);
            dataAsString = null;

            UpdateWindow();
        }

        var typeRect = GUILayoutUtility.GetLastRect();
        GUI.Label(typeRect, new GUIContent("", "Insert before selected byte"));

        GUI.enabled = (dataSelectedPos > -1 && dataSelectedPos < currentFile.data.Length) && (dataSelectedValue > -1 && dataSelectedValue < 256);

        if (GUILayout.Button("Set", GUILayout.Width(60)))
        {
            currentFile.data[dataSelectedPos] = (byte)dataSelectedValue;
            dataAsString = null;
            UpdateWindow();
        }
        if (GUILayout.Button("Remove", GUILayout.Width(60)))
        {
            ArrayUtility.RemoveAt(ref currentFile.data, dataSelectedPos);
            dataAsString = null;
            UpdateWindow();
        }
        GUI.enabled = true;
        GUILayout.Label("Length:", GUILayout.Width(70));

        dataArraySize = EditorGUILayout.IntField(GUIContent.none, dataArraySize, GUILayout.Width(60), GUILayout.ExpandWidth(false));

        if (GUILayout.Button("Resize", GUILayout.Width(70)))
        {
            Array.Resize(ref currentFile.data, dataArraySize);
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
                UpdateWindow();
                EditorUtility.SetDirty(currentWindow);
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

        dataAsString = EditorGUILayout.TextArea(dataAsString, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));

        if (GUILayout.Button("Replace Data", GUILayout.ExpandWidth(true)))
        {
            currentFile.data = dataAsString.ToBytes();
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
        dataViewScrollPosition = GUILayout.BeginScrollView(dataViewScrollPosition);

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
            dataAsString = null;
            dataSelectedPos = indexChanged;
        }

        GUILayout.EndHorizontal();

        GUILayout.EndScrollView();
    }
    void DrawPagePickers()
    {
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Width(windowWidth));
        GUI.enabled = page > 0;
        if (GUILayout.Button("Previous Page"))
        {
            page--;
            dataViewScrollPosition = Vector2.zero;
        }
        GUI.enabled = true;
        GUILayout.Label(" ", GUILayout.ExpandWidth(true));

        GUILayout.Label("Page: " + page, GUILayout.Width(60));

        GUILayout.Label(" ", GUILayout.ExpandWidth(true));

        GUI.enabled = ((page + 1) * size * size <= dataSP.arraySize);
        if (GUILayout.Button("Next Page"))
        {
            page++;
            dataViewScrollPosition = Vector2.zero;
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
            FileEditor.ShowWindow(file);
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


