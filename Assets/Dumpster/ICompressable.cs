using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICompressable
{
    public byte[] ToData();
    public void FromData(byte[] data);
}