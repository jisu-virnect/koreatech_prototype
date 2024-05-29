using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class popup_InstallDetail : popup_Base
{
    private TMP_Text tmp_SequenceDetail;

    protected override void Awake()
    {
        base.Awake();
        tmp_SequenceDetail = gameObject.Search<TMP_Text>(nameof(tmp_SequenceDetail));
    }
    public override void SetData<T>(T t)
    {
        Sequence sequence = t as Sequence;
        tmp_SequenceDetail.text = sequence.summary;
    }
}
