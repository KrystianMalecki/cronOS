using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "CodeBasement", menuName = "CodeBasement", order = 1)]
public class CodeBasement : ScriptableObject
{
    [AllowNesting]
    public List<CodeBlock> codeBlocks = new List<CodeBlock>();
}
[System.Serializable]
public class CodeBlock
{
    [Button]
    public void CopyToClipboard()
    {
        EditorGUIUtility.systemCopyBuffer = code;
    }
    public string tag;
    [NaughtyAttributes.ResizableTextArea]
    public string code;

    public CodeBlock(string tag, string code)
    {
        this.tag = tag;
        this.code = code;
    }
  
}