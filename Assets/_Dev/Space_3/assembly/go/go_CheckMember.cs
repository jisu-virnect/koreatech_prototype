using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class go_CheckMember : MonoBehaviour
{

    private TMP_Text tmp_Index;
    private GameObject go_Position_Host;
    private GameObject go_Position_Guest;
    private TMP_Text tmp_MemberName;
    private void Awake()
    {
        tmp_Index = gameObject.Search<TMP_Text>(nameof(tmp_Index));
        go_Position_Host = gameObject.SearchGameObject(nameof(go_Position_Host));
        go_Position_Guest = gameObject.SearchGameObject(nameof(go_Position_Guest));
        tmp_MemberName = gameObject.Search<TMP_Text>(nameof(tmp_MemberName));
    }

    /// <summary>
    /// 전체값 변경
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isHost"></param>
    /// <param name="memberName"></param>
    public void SetData(int index, bool isHost, string memberName)
    {
        tmp_Index.text = index.ToString("00");
        go_Position_Host.SetActive(false);
        go_Position_Guest.SetActive(false);
        if (isHost)
        {
            go_Position_Host.SetActive(true);
        }
        else
        {
            go_Position_Guest.SetActive(true);
        }
        tmp_MemberName.text = memberName;
    }

    /// <summary>
    /// index값만 변경
    /// </summary>
    /// <param name="index"></param>
    public void SetData(int index)
    {
        tmp_Index.text = index.ToString("00");
    }
}
