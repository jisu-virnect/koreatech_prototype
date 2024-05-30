using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class panel_GlobalMessage : panel_Base
{
    private TMP_Text tmp_GlobalMessage;
    protected override void Awake()
    {
        base.Awake();
        tmp_GlobalMessage = gameObject.Search<TMP_Text>(nameof(tmp_GlobalMessage));
    }

    public override void SetData<T>(T t)
    {
        base.SetData(t);
        string message = t as string;
        tmp_GlobalMessage.text = message;
    }
}
