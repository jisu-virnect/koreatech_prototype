using System;

#region core
[Serializable]
public class Wrapper<T>
{
    public T[] array;
}
#endregion

#region common
[Serializable]
public class Section
{
    public int index;
    public string type;
    public string title;
    public string step1;
    public string step2;
}

[Serializable]
public class CheckEnvironment
{
    public int index;
    public string title;
    public string summary;
    public string caption;
    public string popuptitle;
    public string popupsummary;
    public int ischecked;
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
public class WorkScaffold : Section
{
}

[Serializable]
public class Uninstall: Install
{
}
#endregion