using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class go_Check_Environment : MonoBehaviour
{
    private TMP_Text tmp_Index;
    private TMP_Text tmp_Summary;

    private Image img_Check1;
    private Image img_Check2;

    private Button btn_Submit;

    private CheckEnvironment checkEnvironment;

    private void Awake()
    {
        tmp_Index = gameObject.Search<TMP_Text>(nameof(tmp_Index));
        tmp_Summary = gameObject.Search<TMP_Text>(nameof(tmp_Summary));

        img_Check1 = gameObject.Search<Image>(nameof(img_Check1));
        img_Check2 = gameObject.Search<Image>(nameof(img_Check2));

        btn_Submit = gameObject.Search<Button>(nameof(btn_Submit));
        btn_Submit.onClick.AddListener(OnClick_Submit);
    }

    private void OnClick_Submit()
    {
        popup_Basic popup_Basic = UIManager.instance.GetPopup<popup_Basic>();
        popup_Basic.SetData(new packet_popup_Basic(checkEnvironment.popuptitle, checkEnvironment.popupsummary));
        //popup_Basic.SetAction(OnClick);

        img_Check1.gameObject.SetActive(true);
        btn_Submit.interactable = false;
    }
    public void SetData(CheckEnvironment checkEnvironment)
    {
        this.checkEnvironment = checkEnvironment;
        tmp_Index.text = (checkEnvironment.index + 1).ToString("00");
        tmp_Summary.text = checkEnvironment.title + "<size=8>" + checkEnvironment.summary + "</size>";
        img_Check1.gameObject.SetActive(checkEnvironment.ischecked == 1 ? true : false);
        btn_Submit.interactable = checkEnvironment.ischecked == 1 ? false : true;
    }

}
