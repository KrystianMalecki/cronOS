using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using NaughtyAttributes;
using Libraries.system.output.graphics.system_colorspace;

public class CodeGenerator : MonoBehaviour
{
    public AttributeTreePack pta = new AttributeTreePack();
    [ResizableTextArea]
    public string outputCode = "";

    [Button]
    public void a()
    {
        makePta(typeof(test));
    }
    private void makePta(System.Type t)
    {


        Attribute[] attrs = Attribute.GetCustomAttributes(t);    
        pta = new AttributeTreePack();

        Debug.Log(attrs.GetValuesToString());
        foreach (System.Attribute attr in attrs)
        {
            if (attr is StructureAttribute)
            {
                StructureAttribute sa = (StructureAttribute)attr;
                pta.structureName = t.Name;
                Debug.Log(sa.name);
                var fis = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                foreach (FieldInfo pi in fis)
                {
                    Attribute[] Piattrs = Attribute.GetCustomAttributes(pi);
                    Debug.Log(Piattrs.GetValuesToString());
                    foreach (Attribute piattr in Piattrs)
                    {
                        if (piattr is FieldAttribute)
                        {
                            FieldAttribute fa = (FieldAttribute)piattr;
                            AttributeTreeField atf = new AttributeTreeField();
                            atf.name = pi.Name;
                            string type = string.IsNullOrEmpty(fa.type) ? pi.FieldType.Name : fa.type;
                            if (pi.FieldType.IsArray)
                            {
                                type = type.Substring(0, type.Length - 2);
                            }

                            atf.type = type;


                            atf.size = fa.arraySize;


                            pta.fields.Add(atf);


                        }
                    }
                }
            }
        }

        outputCode = @"
public byte[] ToData()
    {
";
        outputCode += " byte[] __bytes = new byte[";
        for (int i = 0; i < pta.fields.Count - 1; i++)
        {
            outputCode += pta.fields[i].GetSizeOf() + "+";
        }
        outputCode += pta.fields[pta.fields.Count - 1].GetSizeOf() + "];\n";
        outputCode += "int __counter = 0;\n";
        for (int i = 0; i < pta.fields.Count; i++)
        {
            outputCode += pta.fields[i].GetSetByteValue();
        }
        outputCode += @"
    return bytes;
    }
";
        outputCode += "\n public static " + pta.structureName + " FromData(byte[] data)\n";
        outputCode += "{\n";
        outputCode += pta.structureName + " __structure = new  " + pta.structureName + "();";
        outputCode += "int __counter = 0;";
        for (int i = 0; i < pta.fields.Count; i++)
        {
            outputCode += pta.fields[i].GetSetWithConverter();
        }


        outputCode += @"
        return __structure;
    }";
    }
    [Serializable]
    public class AttributeTreePack
    {
        public string structureName;
        public List<AttributeTreeField> fields = new List<AttributeTreeField>();
    }
    [Serializable]
    public class AttributeTreeField
    {
        public string name;
        public string type;
        public string size;
        public string GetSizeOf()
        {
            return "(sizeof(" + type + ")" + (string.IsNullOrEmpty(size) ? "" : "*" + size) + ")";
        }
        public string GetSetByteValue()
        {
            return "    __bytes.SetByteValue(" + name + ".ToBytes(),__counter);__counter += " + GetSizeOf() + ";\n";
        }
        public string GetSetWithConverter()
        {
            return "    __structure." + name + " = BitConverter.To" + type + "(data, __counter);__counter += " + "(sizeof(" + type + ")" + (string.IsNullOrEmpty(size) ? "" : "*__structure." + size) + ")" + ";\n";
        }
    }
    [Serializable]
    [Structure]
    public struct test
    {
        [Field]
        public short width;
        [Field]
        public int size;

        [Field(type = "byte", arraySize = "20")]
        public string str;
        [Field(arraySize = "size")]
        public byte[] bytes;
        [Field]
        public char ch;
        [Field]
        public bool b;
        [Field]
        public byte byt;
        [Field]
        public SystemColor sc;
        /*

        public byte[] ToData()
        {
            byte[] __bytes = new byte[(sizeof(Int16)) + (sizeof(Int32)) + (sizeof(byte) * 20) + (sizeof(Byte) * size) + (sizeof(Char)) + (sizeof(Boolean)) + (sizeof(Byte)) + (sizeof(SystemColor))];
            int __counter = 0;
            __bytes.SetByteValue(width.ToBytes(), __counter); __counter += (sizeof(Int16));
            __bytes.SetByteValue(size.ToBytes(), __counter); __counter += (sizeof(Int32));
            __bytes.SetByteValue(str.ToBytes(), __counter); __counter += (sizeof(byte) * 20);
            __bytes.SetByteValue(bytes.ToBytes(), __counter); __counter += (sizeof(Byte) * size);
            __bytes.SetByteValue(ch.ToBytes(), __counter); __counter += (sizeof(Char));
            __bytes.SetByteValue(b.ToBytes(), __counter); __counter += (sizeof(Boolean));
            __bytes.SetByteValue(byt.ToBytes(), __counter); __counter += (sizeof(Byte));
            __bytes.SetByteValue(sc.ToBytes(), __counter); __counter += (sizeof(SystemColor));

            return bytes;
        }

        public static test FromData(byte[] data)
        {
            test __structure = new test(); int __counter = 0; __structure.width = BitConverter.ToInt16(data, __counter); __counter += (sizeof(Int16));
            __structure.size = BitConverter.ToInt32(data, __counter); __counter += (sizeof(Int32));
            __structure.str = BitConverter.Tobyte(data, __counter); __counter += (sizeof(byte) * __structure.20);
            __structure.bytes = BitConverter.ToByte(data, __counter); __counter += (sizeof(Byte) * __structure.size);
            __structure.ch = BitConverter.ToChar(data, __counter); __counter += (sizeof(Char));
            __structure.b = BitConverter.ToBoolean(data, __counter); __counter += (sizeof(Boolean));
            __structure.byt = BitConverter.ToByte(data, __counter); __counter += (sizeof(Byte));
            __structure.sc = BitConverter.ToSystemColor(data, __counter); __counter += (sizeof(SystemColor));

            return __structure;
        }

        */

    }
}
