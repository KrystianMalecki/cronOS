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
using Libraries.system.output.music;
using Libraries.system.output.graphics.system_screen_buffer;

[Serializable]
public class Hardware /*API for in game usage*/
{
    [NonSerialized] internal HardwareInternal hardwareInternal;
    [ThreadStatic] public static Hardware currentThreadInstance;
    public Hardware thisHardware => this;
    public string testData;
    public bool focused => hardwareInternal.focused;
    internal DriveSO mainDrive => hardwareInternal.mainDrive;
    internal InputManager inputManager => hardwareInternal.inputManager;
    internal ScreenManager screenManager => hardwareInternal.screenManager;
    internal StackExecutor stackExecutor => hardwareInternal.stackExecutor;
    internal AudioManager audioManager => hardwareInternal.audioManager;

    public static File currentFile = null;
    public static SystemScreenBuffer screenBuffer = new SystemScreenBuffer();
    internal Hardware(HardwareInternal hardwareInternal)
    {
        this.hardwareInternal = hardwareInternal;
        hardwareInternal.hardware = this;
    }
    internal void Init()
    {
        currentFile = mainDrive.drive.GetFileByPath("/sys");
        //  hardwareInternal.hardware = this;
        // hardwareInternal.stackExecutor.hardware = this;
        // hardwareInternal.inputManager.hardware = this;

        /* runtime.Init(ownPointer);
         fileSystem.Init(ownPointer);
         keyHandler.Init(ownPointer);
         mouseHandler.Init(ownPointer);
         screen.Init(ownPointer);
         audioHandler.Init(ownPointer);*/
    }

}