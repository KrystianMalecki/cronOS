using Libraries.system.graphics.system_color;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Helpers
{

}

public static class StaticHelper
{


    public static string ToArrayString<T>(this IEnumerable<T> ie)
    {
        return string.Join(",", ie);
    }
    public static string ToArrayString<T>(this T[] ie)
    {
        return string.Join(",", ie);
    }
    public static LibraryData ToLibraryData(this Type type)
    {
        return new LibraryData(type.GetTypeInfo().Assembly.GetName().FullName.ToString(), type.GetTypeInfo().Namespace);
    }
    public static char ToChar(this byte onebyte)
    {
        //  char c = '\u0009';
        return ((char)onebyte);
    }
    /*  public static unsafe byte[] ConvertToBytes<T>(T value) where T : unmanaged
      {
          byte* pointer = (byte*)&value;

          byte[] bytes = new byte[sizeof(T)];
          for (int i = 0; i < sizeof(T); i++)
          {
              bytes[i] = pointer[i];
          }

          return bytes;
      }*/
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
    public static byte[] GetRange(this byte[] variable, int start, int length)
    {
        return variable;
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

