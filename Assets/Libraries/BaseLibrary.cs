using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// This class is required to make quick async versions of libraries
/// </summary>
public class BaseLibrary
{
    /// <summary>
    /// 'False' value hidden as properity
    /// </summary>
    public static bool no => false;
    /// <summary>
    /// 'True' value hidden as properity
    /// </summary>
    public static bool yes => true;
    /// <summary>
    /// Value that should be used to make <see cref="MainThreadDelegate{T}"/> be able to be async in async version of library
    /// </summary>
    public static bool sync => yes;

}
