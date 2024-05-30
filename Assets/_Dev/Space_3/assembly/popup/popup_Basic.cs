using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class popup_Basic : popup_Base
{
    private TMP_Text tmp_Title;
    private TMP_Text tmp_Content;
    protected override void Awake()
    {
        base.Awake();
        tmp_Title = gameObject.Search<TMP_Text>(nameof(tmp_Title));
        tmp_Content = gameObject.Search<TMP_Text>(nameof(tmp_Content));
    }
    public override void SetData<T>(T t)
    {
        base.SetData(t);
        packet_popup_Basic packet_popup_Basic = t as packet_popup_Basic;
        tmp_Title.text = packet_popup_Basic.title;
        tmp_Content.text = packet_popup_Basic.content;
    }

    public override void Close()
    {
        base.Close();
    }
}
public class packet_popup_Basic
{
    public string title;
    public string content;

    public packet_popup_Basic(string title, string content)
    {
        this.title = title;
        this.content = content;
    }
}
