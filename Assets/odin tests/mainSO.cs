using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OdinSerializer;
using System;
[CreateAssetMenu(fileName = "mainSO", menuName = "ScriptableObjects/mainSO")]

public class mainSO : SerializedScriptableObject
{
    [NonSerialized, OdinSerialize]
    public subClass1 subClass1;
}

