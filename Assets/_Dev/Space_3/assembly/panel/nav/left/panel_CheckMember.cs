using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class panel_CheckMember : panel_Base
{
    private GameObject go_CheckMember;
    private RectTransform content;

    protected override void Awake()
    {
        base.Awake();

        go_CheckMember = ResourceManager.instance.LoadData<GameObject>(nameof(go_CheckMember));
        ScrollRect sview_CheckMember = gameObject.Search<ScrollRect>(nameof(sview_CheckMember));
        content = sview_CheckMember.content;
    }

    private Dictionary<string, go_CheckMember> go_CheckMembers = new Dictionary<string, go_CheckMember>();


    public void AddMember(bool isHost, string memberName)
    {
        GameObject go = Instantiate(go_CheckMember, content);
        go_CheckMember temp = go.GetComponent<go_CheckMember>();
        temp.SetData(go_CheckMembers.Count+1, isHost, memberName);
        go_CheckMembers.Add(memberName, temp);
    }

    public void RemoveMember(string memberName)
    {
        if (go_CheckMembers.ContainsKey(memberName))
        {
            Destroy(go_CheckMembers[memberName].gameObject);
            go_CheckMembers.Remove(memberName);
        }
        RefreshMember();
    }

    private void RefreshMember()
    {
        Dictionary<string, go_CheckMember>.ValueCollection v = go_CheckMembers.Values;
        int index = 1;
        foreach (var item in v)
        {
            item.SetData(index);
            index++;
        }
    }
}

public class packet_CheckMember
{
    public int isHost;
    public string memberName;
}
