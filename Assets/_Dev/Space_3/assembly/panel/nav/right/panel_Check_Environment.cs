using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panel_Check_Environment : panel_Base
{
    public int remainCheckEnvironment { get; set; }
    private GameObject go_Check_Environment;
    private ScrollRect sview_;

    protected override void Awake()
    {
        base.Awake();
        go_Check_Environment = ResourceManager.instance.LoadData<GameObject>(nameof(go_Check_Environment));
        sview_ = gameObject.Search<ScrollRect>(nameof(sview_));
    }

    public override void Open(Action act = null)
    {
        base.Open();
        ClearElement();

        remainCheckEnvironment = 0;

        List<CheckEnvironment> checkEnvironments = DBManager.instance.CheckEnvironments;
        for (int i = 0; i < checkEnvironments.Count; i++)
        {
            GameObject go = Instantiate(go_Check_Environment, sview_.content);
            go.GetComponent<go_Check_Environment>().SetData(checkEnvironments[i]);
        }

        Space_3.instance.Control_PlayerMovement(false);
        Space_3.instance.Control_VirtualCamera(eVirtualCameraState.vcam_before);
    }
    protected override IEnumerator Action_Opening()
    {
        yield return null;
        sview_.verticalScrollbar.value = 0;
    }


    public override void Close(Action act = null)
    {
        base.Close();
        //UIManager.instance.HideToast<toast_Basic>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.instance.ClosePanels(Define.before);
            UIManager.instance.OpenPanel<panel_TriggerMenu>(Define.trigger);
            UIManager.instance.GetPanel<panel_TopNavigation>().ResetStep();
        }
    }

    private void ClearElement()
    {
        for (int i = sview_.content.childCount - 1; i >= 0; i--)
        {
            Destroy(sview_.content.GetChild(i).gameObject);
        }
    }
}
