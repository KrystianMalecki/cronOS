
using OdinSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class mainMB : SerializedMonoBehaviour
{
    [NonSerialized, OdinSerialize]
    public subClass1 subClass1;
}