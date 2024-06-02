using SpatialSys.UnitySDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panel_Check_Install : panel_Base
{
    GameObject scaffold;
    scaffold01_1 scaffold01_1;
    protected override void Awake()
    {
        base.Awake();
        scaffold = ResourceManager.instance.LoadData<GameObject>(nameof(scaffold));
        scaffold01_1 = scaffold.GetComponent<scaffold01_1>();
    }

    public override void Open(Action act = null)
    {
        base.Open(act);
        scaffold01_1.Action_ObjectsColliderEnable(true);
    }

    public override void Close(Action act = null)
    {
        base.Close(act);
        scaffold01_1.Action_ResetObjects();
    }
}
