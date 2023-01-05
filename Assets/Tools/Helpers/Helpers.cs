using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



public static class StaticHelper
{
    public static void TestFunction(Action a, [CallerMemberName] string fromName = "unknown name",
        [CallerFilePath] string fromPath = @"\unknown path")
    {
        double time = Time.realtimeSinceStartupAsDouble;
        for (int i = 0; i < 10000; i++)
        {
            a.Invoke();
        }

        double timeEnd = Time.realtimeSinceStartupAsDouble;
        Debug.LogWarning("Time elapsed to run function from " + fromName + " in " +
                         fromPath.Substring(fromPath.LastIndexOf('\\') + 1) + " :" + (timeEnd - time));
    }

    public static IEnumerable<T> Iterate<T>(this IEnumerator<T> iterator)
    {
        while (iterator.MoveNext())
            yield return iterator.Current;
    }

    public static string ToFormattedString<T>(this IEnumerable<T> ie, string splitter = ", ")
    {
        return string.Join(splitter, ie);
    }

    public static string ToFormattedString2(this IDictionary ie, string splitter = ", ")
    {
        string s = "";
        foreach (var v in ie.Keys)
        {
            s += v + " - " + ie[v] + ", ";
        }

        return s;
    }

    public static string ToFormattedString<T>(this T[] ie, string splitter = ", ")
    {
        return string.Join(splitter, ie);
    }

    public static string ToDisplayString(this BitArray ba, string splitter = ", ")
    {
        string output = "";
        for (int i = 0; i < ba.Length; i++)
        {
            output += ba[i] + splitter;
        }

        return output;
    }

    public static string ToConvertedString<T>(this T[] ie, string splitter = ", ",
        Converter<T, String> converter = null)
    {
        if (converter == null)
        {
            return string.Join(splitter, ie);
        }

        return string.Join(splitter, Array.ConvertAll(ie, x => converter(x)));
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


    public static string GetRangeBetweenFirstLast(this string input, string key, int offset = 0,
        bool includeKeys = true)
    {
        return input.GetRangeBetweenFirstLast(key, key, offset, includeKeys);
    }

    public static string GetRangeBetweenFirstLast(this string input, string startKey, string endKey, int offset = 0,
        bool includeKeys = true)
    {
        int startPos = input.IndexOf(startKey, offset) + 1 + (includeKeys ? 0 : 1);
        int endPos = input.LastIndexOf(endKey) - startPos + (includeKeys ? 0 : -1);
        return input.Substring(startPos, endPos);
    }

    public static string GetRangeBetweenFirstNext(this string input, string key, int offset = 0,
        bool includeKeys = true)
    {
        return input.GetRangeBetweenFirstNext(key, key, offset, includeKeys);
    }

    public static string GetRangeBetweenFirstNext(this string input, string startKey, string endKey, int offset = 0,
        bool includeKeys = true)
    {
        int startPos = input.IndexOf(startKey, offset) + 1 + (includeKeys ? 0 : 1);

        int endPos = input.IndexOf(endKey, startPos) - startPos + (includeKeys ? 0 : -1);
        Debug.Log(
            $"start pos:{startPos}.endPos{endPos}.calc of first{input.IndexOf(startKey, offset)}. calc of sendobnd{input.IndexOf(endKey, startPos)}");
        return input.Substring(startPos, endPos);
    }

    public static byte[] SetByteValue(this byte[] array, byte[] data, int index)
    {
        Array.Copy(data, 0, array, index, data.Length);
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


    public static List<string> GetRangeBetweenFirstNext(this List<string> input, string startKey, string endKey,
        int offset = 0, bool includeKeys = true)
    {
        List<string> workableInput = new List<string>(input.Skip(offset));

        int start = workableInput.FindIndex(x => x == startKey) + (includeKeys ? 0 : 1);
        int end = workableInput.FindIndex(start + 1, x => x == endKey) + (includeKeys ? 0 : -1);

        return workableInput.Skip(start).Take(end - start + 1).ToList();
    }






    public static Dictionary<string, string> ParseTextAsObjectKeyValue(string text)
    {
        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
        string[] lines = text.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        foreach (string line in lines)
        {
            int pairPoint = line.IndexOf(":");
            if (pairPoint > 0)
            {
                string key = line.Substring(0, pairPoint);
                string value = line.Substring(pairPoint + 1).Trim();
                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    value = value.Substring(1, value.Length - 2);
                }
                keyValuePairs.Add(key, value);
            }
            else
            {
                //todo 9 throw error
            }
        }
        return keyValuePairs;
    }
    public static Dictionary<string, object> ParseTextAsObjectKeyValueWithType(string text)
    {
        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
        string[] lines = text.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        foreach (string line in lines)
        {
            int typePoint = line.IndexOf(":");
            int pairPoint = line.IndexOf(":", typePoint + 1);
            if (pairPoint > 0)
            {
                string type = line.Substring(0, typePoint).Trim();
                string key = line.Substring(typePoint + 1, pairPoint - typePoint - 1).Trim();
                string value = line.Substring(pairPoint + 1).Trim();
                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    value = value.Substring(1, value.Length - 2);
                }
                keyValuePairs.Add(key, Convert.ChangeType(value, ToType(type)));
            }
            else
            {
                //todo 9 throw error
            }

        }
        return keyValuePairs;
    }
    public static Type ToType(string type) =>
       type switch
       {
           "int" => typeof(int),
           "string" => typeof(string),
           "float" => typeof(float),
           "bool" => typeof(bool),
           "char" => typeof(char),
           "byte" => typeof(byte),
           "double" => typeof(double),
           "short" => typeof(short),
           "long" => typeof(long),
           "object" => null,//todo 99 implement
           _ => null

       };
}

/*
[Serializable]
//  [MainClass]
public struct SystemTextureFile
{
    public short width;
    public short height;
    [SerializeField] public SystemColor[] colors;

    private int arrayLength
    {
        get { return width * height; }
    }


    public byte[] ToData()
    {
        byte[] bytes = new byte[sizeof(short) + sizeof(short) + (sizeof(byte) * arrayLength)];
        int counter = 0;
        bytes.SetByteValue(width.ToBytes(), counter);
        counter += sizeof(short);
        bytes.SetByteValue(height.ToBytes(), counter);
        counter += sizeof(short);

        bytes.SetByteValue(Array.ConvertAll(colors, x => x.byteValue), counter);
        return bytes;
    }

    public static SystemTextureFile FromData(byte[] data)
    {
        SystemTextureFile stf = new SystemTextureFile();
        int counter = 0;
        stf.width = BitConverter.ToInt16(data, counter);
        counter += sizeof(short);
        stf.height = BitConverter.ToInt16(data, counter);
        counter += sizeof(short);
        byte[] bytes = new byte[sizeof(byte) * stf.arrayLength];
        Array.Copy(data, counter, bytes, 0, bytes.Length);


        stf.colors = Array.ConvertAll(bytes, x => new SystemColor(x));

        return stf;
    }
}*/