using System;

[Serializable]
public class CheckEnvironment
{
    public int index;
    public string title;
    public string summary;
    public string caption;
}

[Serializable]
public class SafetyTools
{
    public int index;
    public string title;
    public string summary;
    public string caption;
}

[Serializable]
public class Install
{
    public int index;
    public string title;
    public string summary;
    public string renderMode;
}

[Serializable]
public class CheckInstall
{
    public int index;
    public string title;
    public string summary;
    public string renderMode;
}


[Serializable]
public class Work
{
}

[Serializable]
public class Uninstall: Install
{
}

[Serializable]
public class Wrapper<T>
{
    public T[] array;
}