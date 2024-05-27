using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SpatialSys.UnitySDK;

public class Space_3_SequenceDetail : MonoBehaviour
{
    private TMP_Text tmp_SequenceDetail;
    private Button btn_Close;
    public void GetComponent()
    {
        tmp_SequenceDetail = gameObject.Search<TMP_Text>(nameof(tmp_SequenceDetail));
        btn_Close = gameObject.Search<Button>(nameof(btn_Close));
        btn_Close.onClick.AddListener(ClosePopop);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePopop();
        }
    }

    public void OpenSequenceDetail(Sequence sequence)
    {
        tmp_SequenceDetail.text = sequence.summary;
        gameObject.SetActive (true);
    }

    private void ClosePopop()
    {
        gameObject.SetActive(false);
    }
}
