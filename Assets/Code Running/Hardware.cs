using Libraries.system.output.graphics;
using Libraries.system.file_system;
using System.Text;
using System;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Concurrent;
using helper;
using System.Linq;
using System.Text.RegularExpressions;
using Libraries.system;
using Libraries.system.input;

[Serializable]
public class Hardware
{
    //todo 0 add shell
    //  public static SimpleShell
    //todo rename to 
    public int number = 0;
    public dynamic shell;
    public Runtime runtime = new Runtime();
    public FileSystem fileSystem = new FileSystem();
    public KeyHandler keyHandler = new KeyHandler();
    public MouseHandler mouseHandler = new MouseHandler();
    public Screen screen = new Screen();

    public Hardware ownPointer => this;
    [UnityEngine.SerializeField] internal bool currentlySelected;
    [UnityEngine.SerializeField] internal HardwareInternal hardwareInternal = new HardwareInternal();

    public void Init()
    {
        hardwareInternal.mainDrive.GenerateCacheData();

        hardwareInternal.hardware = this;
        hardwareInternal.stackExecutor.hardware = this;
        hardwareInternal.inputManager.hardware = this;
        runtime.Init(ownPointer);
        fileSystem.Init(ownPointer);
        keyHandler.Init(ownPointer);
        mouseHandler.Init(ownPointer);
        screen.Init(ownPointer);
    }
}