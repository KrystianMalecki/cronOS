using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;

public class BaseLibrary
{
    public bool sync = false;

    protected Hardware hardware;
    public void Init(Hardware hardware)
    {
        this.hardware = hardware;
    }
}
