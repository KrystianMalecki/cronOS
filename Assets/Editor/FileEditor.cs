using Libraries.system.filesystem;
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

        /*   currentWindow.currentFileSP = fileSP;
           currentWindow.currentFileSO = fileSO;*/
        currentWindow.currentFileSO = new SerializedObject(currentWindow);
        currentWindow.currentFileSP = currentWindow.currentFileSO.FindProperty("currentFile");
        //  currentWindow.dataProperities = new Dictionary<int, SerializedProperty>();
        currentWindow._dataSP = null;
        currentWindow._filesSP = null;
        currentWindow._dataSP = null;
        currentWindow.dataSize = currentWindow.currentFile.data.Length;
        currentWindow.scroll = Vector2.zero;

    }
    int size = 32;
    int page = 0;
    SerializedProperty _permissionsSP;
    SerializedProperty _filesSP;
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
    SerializedProperty filesSP
    {
        get
        {
            if (_filesSP == null)
            {
                _filesSP = currentFileSP.FindPropertyRelative("files");
            }
            return _filesSP;
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

    Vector2 scroll = Vector2.zero;

    int dataSize = 0;
    int dataPos = 0;
    int dataPosData = 0;


    float windowWidth = 0;
    float boxWidth = 30;
    bool fold = true;
    void OnGUI()
    {
        windowWidth = position.width - 10;
        boxWidth = Mathf.Min((windowWidth - 20) / (size + 1), 30);

        EditorGUI.BeginChangeCheck();

        fold = (EditorGUILayout.BeginFoldoutHeaderGroup(fold, "Main Data"));
        if (fold)
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
            GUILayout.Space(EditorGUIUtility.singleLineHeight);

        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        //files
        EditorGUILayout.PropertyField(filesSP);
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Width(windowWidth));
        GUILayout.EndHorizontal();

      //  GUILayout.Space(EditorGUIUtility.singleLineHeight);

        //clampting

        //page navigation
        DrawPagePickers();

        GUILayout.Space(EditorGUIUtility.singleLineHeight);

        DrawSpecialControls();
        // array
        DrawDataArray();
        //save all
        if (EditorGUI.EndChangeCheck())
        {
            currentFileSO.ApplyModifiedProperties();

        }

        //   currentFileSP.serializedObject.Update();
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

        GUILayout.Label("Pos:", GUILayout.Width(30));

        dataPos = EditorGUILayout.IntField(GUIContent.none, dataPos, GUILayout.Width(30), GUILayout.ExpandWidth(false));

        GUILayout.Label("Item:", GUILayout.Width(30));

        dataPosData = EditorGUILayout.IntField(GUIContent.none, dataPosData, GUILayout.Width(30), GUILayout.ExpandWidth(false));

        if (GUILayout.Button("Insert", GUILayout.Width(50)))
        {
            ArrayUtility.Insert(ref currentFile.data, dataPos, (byte)dataPosData);
            currentFileSO.Update();
        }
        GUILayout.Label(" ", GUILayout.ExpandWidth(true));

        GUILayout.Label("Length:", GUILayout.Width(50));

        dataSize = EditorGUILayout.IntField(GUIContent.none, dataSize, GUILayout.Width(40), GUILayout.ExpandWidth(false));

        if (GUILayout.Button("Resize", GUILayout.Width(50)))
        {
            Array.Resize(ref currentFile.data, dataSize);
            currentFileSO.Update();
        }

        GUI.enabled = true;
        GUILayout.Space(80);
        GUILayout.EndHorizontal();
    }
    void DrawDataArray()
    {
        int start = page * size * size;
        int length = dataSP.arraySize;
        if (length > size * size)
        {
            length = size * size;
        }
        scroll = GUILayout.BeginScrollView(scroll);
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Width(windowWidth));
        for (int i = start; i < start + length; i++)
        {
            if (currentFile.data.Length <= i)
            {
                break;
            }
            if (i % size == 0)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Width(windowWidth));

            }
            //  EditorGUILayout.PropertyField(GetDataAt(i), GUIContent.none);

            currentFile.data[i] = (byte)EditorGUILayout.IntField(GUIContent.none, currentFile.data[i], GUILayout.Width(boxWidth), GUILayout.MinWidth(1));
            var typeRect = GUILayoutUtility.GetLastRect();
            GUI.Label(typeRect, new GUIContent("", "byte " + i));
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
            scroll = Vector2.zero;
        }
        GUI.enabled = true;
        GUILayout.Label(" ", GUILayout.ExpandWidth(true));

        GUILayout.Label("Page: " + page, GUILayout.Width(60));

        GUILayout.Label(" ", GUILayout.ExpandWidth(true));

        GUI.enabled = ((page + 1) * size * size <= dataSP.arraySize);
        if (GUILayout.Button("Next Page"))
        {
            page++;
            scroll = Vector2.zero;
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
}
[System.Serializable]
public class FileSO : UnityEngine.Object
{
    [SerializeField]
    public File file;

    public FileSO(File file)
    {
        this.file = file;
    }
}