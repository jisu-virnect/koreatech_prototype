using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;
using System;

[Serializable]
public class Sequence
{
    public int index;
    public string title;
    public string summary;
}

[Serializable]
public class Wrapper<T>
{
    public T[] array;
}