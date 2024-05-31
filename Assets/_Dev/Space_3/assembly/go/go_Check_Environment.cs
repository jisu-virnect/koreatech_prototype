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
    private panel_Check_Environment panel_Check_Environment;
    private void Awake()
    {
        tmp_Index = gameObject.Search<TMP_Text>(nameof(tmp_Index));
        tmp_Summary = gameObject.Search<TMP_Text>(nameof(tmp_Summary));

        img_Check1 = gameObject.Search<Image>(nameof(img_Check1));
        img_Check2 = gameObject.Search<Image>(nameof(img_Check2));

        btn_Submit = gameObject.Search<Button>(nameof(btn_Submit));
        btn_Submit.onClick.AddListener(OnClick_Submit);

        panel_Check_Environment = UIManager.instance.GetPanel<panel_Check_Environment>();
    }

    private void OnClick_Submit()
    {
        //�˾� ����
        popup_Basic popup_Basic = UIManager.instance.GetPopup<popup_Basic>();
        popup_Basic.SetData(new packet_popup_Basic(checkEnvironment.popuptitle, checkEnvironment.popupsummary));

        //üũ �� ��ư ��Ȱ��ȭ
        img_Check1.gameObject.SetActive(true);
        btn_Submit.interactable = false;

        //���� üũ���� ī��Ʈ
        panel_Check_Environment.remainCheckEnvironment--;
        if(panel_Check_Environment.remainCheckEnvironment == 0)
        {
            popup_Basic.SetAction(() =>
            {
                UIManager.instance
                    .ShowHideToast<toast_Basic>("[�۾����� ����] �ܰ谡 �Ϸ�Ǿ����ϴ�. [���� ���] �ܰ�� �Ѿ�ϴ�.", 2f)
                    .SetData(new packet_toast_basic(eToastColor.green, eToastIcon.toast_success));
            });
        }
    }

    public void SetData(CheckEnvironment checkEnvironment)
    {
        this.checkEnvironment = checkEnvironment;
        tmp_Index.text = (checkEnvironment.index + 1).ToString("00");
        tmp_Summary.text = checkEnvironment.title + "\n<size=8>" + checkEnvironment.summary + "</size>";
        img_Check1.gameObject.SetActive(checkEnvironment.ischecked == 1 ? true : false);
        btn_Submit.interactable = checkEnvironment.ischecked == 1 ? false : true;
        panel_Check_Environment.remainCheckEnvironment += checkEnvironment.ischecked == 1 ? 0 : 1;
    }

}
