using SpatialSys.UnitySDK;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class go_Check_Environment : MonoBehaviour
{
    private GameObject img_Focus;

    private TMP_Text tmp_Index;
    private TMP_Text tmp_Title;
    private TMP_Text tmp_Summary;

    private Image img_Check1;
    private Image img_Check2;

    private Button btn_Submit;

    private CheckEnvironment checkEnvironment;
    private panel_Check_Environment panel_Check_Environment;
    private void Awake()
    {
        img_Focus = gameObject.SearchGameObject(nameof(img_Focus));

        tmp_Index = gameObject.Search<TMP_Text>(nameof(tmp_Index));
        tmp_Title = gameObject.Search<TMP_Text>(nameof(tmp_Title));
        tmp_Summary = gameObject.Search<TMP_Text>(nameof(tmp_Summary));

        img_Check1 = gameObject.Search<Image>(nameof(img_Check1));
        img_Check2 = gameObject.Search<Image>(nameof(img_Check2));

        btn_Submit = gameObject.Search<Button>(nameof(btn_Submit));
        btn_Submit.onClick.AddListener(OnClick_Submit);

        panel_Check_Environment = UIManager.instance.GetPanel<panel_Check_Environment>();
    }

    private void OnClick_Submit()
    {
        //팝업 오픈
        popup_Basic popup_Basic = UIManager.instance.OpenPopup<popup_Basic>();
        popup_Basic.SetData(new packet_popup_Basic(checkEnvironment.popuptitle, checkEnvironment.popupsummary));
        SoundManager.instance.PlayVoice(Util.String2Enum<eAudioClips>("voice_1_1_modal_"+(checkEnvironment.index+1).ToString()));

        //체크 시 버튼 비활성화
        img_Check1.gameObject.SetActive(true);
        btn_Submit.interactable = false;

        img_Focus.SetActive(false);

        //남은 체크개수 카운트
        panel_Check_Environment.remainCheckEnvironment--;
        if(panel_Check_Environment.remainCheckEnvironment == 0)
        {
            popup_Basic.SetAction(() =>
            {
                Space_3.instance.Control_VirtualCamera(eVirtualCameraState.none);
                Space_3.instance.Control_PlayerMovement(false);

                UIManager.instance.ClosePanel<panel_Check_Environment>();

                SoundManager.instance.PlayVoice(eAudioClips.voice_1_1_toast_end);
                UIManager.instance
                    .ShowHideToast<toast_Basic>("<b>[작업현장 조사]</b> 단계가 완료되었습니다. [안전 장비] 단계로 넘어갑니다.", 5.5f, () =>
                    {
                        UIManager.instance.GetPanel<panel_TopNavigation>().NextStep();
                        
                        SoundManager.instance.PlayVoice(eAudioClips.voice_1_2_toast_guide);
                        //UIManager.instance
                        //.ShowHideToast<toast_Basic>("비계 작업에 필요한 안전 장비를 확인합니다.", 3f, () =>
                        //{
                        //    UIManager.instance.OpenPanel<panel_SafetyTools>().ResetStep();
                        //    //체크포인트들 켜주기
                        //})
                        //.SetData(new packet_toast_basic(eToastColor.blue, eToastIcon.toast_idle));

                        popup_Basic popup_Basic = UIManager.instance.OpenPopup<popup_Basic>();
                        popup_Basic.SetData(new packet_popup_Basic("안전 장비", "비계 작업에 필요한 안전 장비를 확인합니다."));
                        popup_Basic.SetAction(() => UIManager.instance.OpenPanel<panel_SafetyTools>().ResetStep());


                    })
                    .SetData(new packet_toast_basic(eToastColor.green, eToastIcon.toast_success));
            });
        }
    }

    public void SetData(CheckEnvironment checkEnvironment)
    {
        this.checkEnvironment = checkEnvironment;
        tmp_Index.text = (checkEnvironment.index + 1).ToString("00");
        tmp_Title.text = checkEnvironment.title;
        tmp_Summary.text = checkEnvironment.summary;
        img_Check1.gameObject.SetActive(checkEnvironment.ischecked == 1 ? true : false);
        btn_Submit.interactable = checkEnvironment.ischecked == 1 ? false : true;
        panel_Check_Environment.remainCheckEnvironment += checkEnvironment.ischecked == 1 ? 0 : 1;
        img_Focus.SetActive(checkEnvironment.ischecked == 1? false : true);
    }

}
