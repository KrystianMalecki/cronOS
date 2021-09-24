using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers
{

}

public static class StaticHelper
{


    public static string ToArrayString<T>(this IEnumerable<T> ie)
    {

        return string.Join(",", ie);
    }
    public static string ToArrayString<T>(this T[] ie)
    {

        return string.Join(",", ie);
    }
}
