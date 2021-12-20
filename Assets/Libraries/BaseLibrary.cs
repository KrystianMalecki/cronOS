using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;

public class BaseLibrary
{
    public bool sync = false;

    protected Cronos.System system;
    public void Init(Cronos.System system)
    {
        this.system = system;
    }
}
