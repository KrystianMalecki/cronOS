using Libraries.system.output.graphics.system_colorspace;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Helpers
{

}

public static class StaticHelper
{
    public static List<string> SplitString2Q(string input)
    {
        bool qSearch = false;
        List<string> output = new List<string>();
        string currentPart = "";
        bool lastCharIsEscape = false;
        for (int i = 0; i < input.Length; i++)
        {

            char c = input[i];
            if (c == '"' && !lastCharIsEscape)
            {
                if (qSearch)
                {
                    qSearch = false;
                    output.Add(currentPart);
                    currentPart = "";
                }
                else
                {
                    qSearch = true;
                }
            }
            else if (c == ' ' && !qSearch)
            {
                output.Add(currentPart);
                currentPart = "";
            }
            else
            {
                if (c == '\\')
                {
                    lastCharIsEscape = true;
                }
                else
                {
                    lastCharIsEscape = false;
                    currentPart += c;
                }
            }
        }
        if (!string.IsNullOrWhiteSpace(currentPart) || !string.IsNullOrEmpty(currentPart))
        {
            output.Add(currentPart);
        }
        return output;
    }
    public static void TestFunction(Action a, [CallerMemberName] string fromName = "unknown name", [CallerFilePath] string fromPath = @"\unknown path")
    {

        double time = Time.realtimeSinceStartupAsDouble;
        for (int i = 0; i < 10000; i++)
        {

            a.Invoke();

        }
        double timeEnd = Time.realtimeSinceStartupAsDouble;
        Debug.LogWarning("Time to run function from " + fromName + " in " + fromPath.Substring(fromPath.LastIndexOf('\\') + 1) + " :" + (timeEnd - time));

    }
    public static IEnumerable<T> Iterate<T>(this IEnumerator<T> iterator)
    {
        while (iterator.MoveNext())
            yield return iterator.Current;
    }
    public static string GetValuesToString<T>(this IEnumerable<T> ie, string splitter = ", ")
    {
        return string.Join(splitter, ie);
    }
    public static string GetValuesToString(this IDictionary ie, string splitter = ", ")
    {
        string s = "";
        foreach (var v in ie.Keys)
        {
            s += v + " - " + ie[v] + ", ";
        }
        return s;
    }
    public static string GetValuesToString<T>(this T[] ie, string splitter = ", ")
    {
        return string.Join(splitter, ie);
    }
    public static LibraryData ToLibraryData(this Type type)
    {
        return new LibraryData(type.GetTypeInfo().Assembly.GetName().FullName.ToString(), type.GetTypeInfo().Namespace);
    }
    public static char ToChar(this byte onebyte)
    {
        return ((char)onebyte);
    }
    public static Vector2 ToVector2(this Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }
    public static Vector2Int ToVectorInt2(this Vector3Int v3)
    {
        return new Vector2Int(v3.x, v3.y);
    }
    public static Vector3 ToVector3(this Vector2 v2)
    {
        return new Vector3(v2.x, v2.y);
    }
    public static Vector3Int ToVectorInt3(this Vector2Int v2)
    {
        return new Vector3Int(v2.x, v2.y, 0);
    }
    public static byte[] ToBytes(this int variable)
    {
        return BitConverter.GetBytes(variable);
    }
    public static byte[] ToBytes(this short variable)
    {
        return BitConverter.GetBytes(variable);
    }
    public static byte[] ToBytes(this byte variable)
    {
        return new byte[] { variable };
    }
    public static byte[] ToBytes(this string variable)
    {
        return ProcessorManager.mainEncoding.GetBytes(variable);
    }
    public static string ToEncodedString(this byte[] variable)
    {
        return ProcessorManager.mainEncoding.GetString(variable);
    }
    /*  public static byte[] GetRange(this byte[] variable, int start, int length)
      {
          return variable;
      }*/
    public static string GetRangeBetweenFirstLast(this string input, string key, int offset = 0)
    {
        int startPos = input.IndexOf(key, offset) + 1;
        int endPos = input.LastIndexOf(key) - startPos;
        return input.Substring(startPos, endPos);
    }
    public static string GetRangeBetweenFirstNext(this string input, string key, int offset = 0)
    {
        int startPos = input.IndexOf(key, offset) + 1;

        int endPos = input.IndexOf(key, startPos) - startPos;
        Debug.Log($"start pos:{startPos}.endPos{endPos}.calc of first{input.IndexOf(key, offset)}. calc of sendobnd{input.IndexOf(key, startPos)}");
        return input.Substring(startPos, endPos);
    }
    public static byte[] SetByteValue(this byte[] array, byte[] data, int index)
    {
        for (int i = 0; i < data.Length; i++)
        {
            array[i + index] = data[i];
        }
        return array;
    }
    public static byte[] _UnsafeSerialize<T>(this T data) where T : struct
    {
        var formatter = new BinaryFormatter();
        var stream = new MemoryStream();
        formatter.Serialize(stream, data);
        return stream.ToArray();
    }
    public static T _UnsafeDeserialize<T>(this byte[] array) where T : struct
    {
        var stream = new MemoryStream(array);
        var formatter = new BinaryFormatter();
        return (T)formatter.Deserialize(stream);
    }
    public static byte[] _MarshaSerialize<T>(this T s) where T : struct
    {
        var size = Marshal.SizeOf(typeof(T));
        var array = new byte[size];
        var ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(s, ptr, true);
        Marshal.Copy(ptr, array, 0, size);
        Marshal.FreeHGlobal(ptr);
        return array;
    }

    public static T _MarshalDeserialize<T>(this byte[] array) where T : struct
    {
        var size = Marshal.SizeOf(typeof(T));
        var ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(array, 0, ptr, size);
        var s = (T)Marshal.PtrToStructure(ptr, typeof(T));
        Marshal.FreeHGlobal(ptr);
        return s;
    }
}


[Serializable]
//  [MainClass]
public struct SystemTextureFile
{
    public short width;
    public short height;
    [SerializeField]
    public SystemColor[] colors;
    private int arrayLength
    {
        get
        {
            return width * height;
        }
    }
    /*
      public byte[] ToData()
    {
        //  return this.UnsafeSerialize();
        //                      field1              field2      arrayfield3 + paste argument1
        byte[] bytes = new byte[sizeof(short) + sizeof(short) + (sizeof(byte) * arrayLength)];
        int counter = 0;
        //loop and add      field    stationary counter     increment counter
        bytes.SetByteValue(width.ToBytes(), counter); counter += sizeof(short);
        bytes.SetByteValue(height.ToBytes(), counter); counter += sizeof(short);
        //                                      another argument or just space to paste OR ToBytes method OR getByte per item
        bytes.SetByteValue(Array.ConvertAll(colors, x => x.value), counter);
        return bytes;
    }
    public static SystemTextureFile FromData(byte[] data)
    {
        // return data.UnsafeDeserialize<SystemTextureFile>();
        //      class field
        SystemTextureFile stf = new SystemTextureFile();
        int counter = 0;
        //needs to be mapped
        stf.width = BitConverter.ToInt16(data, counter); counter += sizeof(short);
        stf.height = BitConverter.ToInt16(data, counter); counter += sizeof(short);
        //                      arrayfield3     argument1 with prefix
        byte[] bytes = new byte[sizeof(byte) * stf.arrayLength];
        //stationary
        Array.Copy(data, counter, bytes, 0, bytes.Length);
        //                                      another argument or just space to paste OR ToBytes method OR getByte per item
        stf.colors = Array.ConvertAll(bytes, x => new SystemColor(x));

        return stf;
    }
     */
    public byte[] ToData()
    {
        byte[] bytes = new byte[sizeof(short) + sizeof(short) + (sizeof(byte) * arrayLength)];
        int counter = 0;
        bytes.SetByteValue(width.ToBytes(), counter); counter += sizeof(short);
        bytes.SetByteValue(height.ToBytes(), counter); counter += sizeof(short);
        bytes.SetByteValue(Array.ConvertAll(colors, x => x.value), counter);
        return bytes;
    }
    public static SystemTextureFile FromData(byte[] data)
    {
        SystemTextureFile stf = new SystemTextureFile();
        int counter = 0;
        stf.width = BitConverter.ToInt16(data, counter); counter += sizeof(short);
        stf.height = BitConverter.ToInt16(data, counter); counter += sizeof(short);
        byte[] bytes = new byte[sizeof(byte) * stf.arrayLength];
        Array.Copy(data, counter, bytes, 0, bytes.Length);
        stf.colors = Array.ConvertAll(bytes, x => new SystemColor(x));

        return stf;
    }

}
