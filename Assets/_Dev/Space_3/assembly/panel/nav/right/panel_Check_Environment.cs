using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panel_Check_Environment : panel_Base
{
    private GameObject go_Check_Environment;
    private GameObject Content;
    protected override void Awake()
    {
        base.Awake();
        go_Check_Environment = ResourceManager.instance.LoadData<GameObject>(nameof(go_Check_Environment));
        Content = gameObject.Search(nameof(Content)).gameObject;
    }
    public override void Open()
    {
        base.Open();
        ClearElement();
        List<CheckEnvironment> checkEnvironments = DBManager.instance.CheckEnvironments;
        for (int i = 0; i < checkEnvironments.Count; i++)
        {
            GameObject go = Instantiate(go_Check_Environment, Content.transform);
            go.GetComponent<go_Check_Environment>().SetData(checkEnvironments[i]);
        }
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
        for (int i = Content.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(Content.transform.GetChild(i).gameObject);
        }
    }
}
