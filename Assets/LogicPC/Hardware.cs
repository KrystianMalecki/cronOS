using Libraries.system.file_system;
using Libraries.system.output.graphics.system_screen_buffer;
using System;
using System.Collections.Generic;
using System.Dynamic;

[Serializable]
public class Hardware /*API for in game usage*/
{
    [NonSerialized] internal HardwareInternal hardwareInternal;
    [ThreadStatic]
    public static Hardware _currentThreadInstance;
    public static Hardware currentThreadInstance
    {
        get
        {
            return _currentThreadInstance;
        }
    }

    [ThreadStatic] public static Shell shell;

    [ThreadStatic] public static dynamic Statics;
    public static void AddStatic(object value)
    {
        if (Statics == null)
        {
            Statics = new ExpandoObject();
        }
        var dic = Hardware.Statics as IDictionary<string, object>;
        UnityEngine.Debug.Log(dic == null);
        UnityEngine.Debug.Log(value == null);

        dic.Add(value.GetType().Name.ToString(), value);
        UnityEngine.Debug.Log(dic.ToFormattedString());
    }

    public Hardware thisHardware => this;
    public string testData;
    public bool focused => hardwareInternal.focused;
    internal DriveSO mainDrive => hardwareInternal.mainDrive;
    internal InputManager inputManager => hardwareInternal.inputManager;
    internal ScreenManager screenManager => hardwareInternal.screenManager;
    internal StackExecutor stackExecutor => hardwareInternal.stackExecutor;
    internal AudioManager audioManager => hardwareInternal.audioManager;

    [ThreadStatic] public static File currentFile;
    [ThreadStatic] public static SystemScreenBuffer screenBuffer;
    internal Hardware(HardwareInternal hardwareInternal)
    {
        this.hardwareInternal = hardwareInternal;
        hardwareInternal.hardware = this;
    }
    public void SetCurrentHardwareInstance()
    {
        _currentThreadInstance = this;
        screenBuffer = new SystemScreenBuffer();
        currentFile = mainDrive.drive.GetFileByPath("/sys"); ;
        Statics = new ExpandoObject();
    }
    internal void Init()
    {
        //  currentFile = mainDrive.drive.GetFileByPath("/sys");
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