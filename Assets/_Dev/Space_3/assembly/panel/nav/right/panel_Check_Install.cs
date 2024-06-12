using SpatialSys.UnitySDK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class panel_Check_Install : panel_Base
{
    private List<CheckInstall> checkInstalls;

    private GameObject go_Check_Install;
    private GameObject go_TapAni;
    private Dictionary<int, go_Check_Install> go_Check_Installs = new Dictionary<int, go_Check_Install>();
    private RectTransform content;

    //private GameObject scaffold;
    //private scaffold01_1 scaffold01_1;

    private Button btn_Checked;
    private Image btn_Image;
    private Image img_Checked;
    private TMP_Text tmp_Checked;

    ScrollRect sview_Check_Install;

    private int remainCheckCount; //남은 체크 카운트

    private GameObject ladder;
    private GameObject ladder_error;
    private GameObject trigger_6;
    private GameObject go_사다리;

    private GameObject work_scaffolding;
    private GameObject work_scaffolding_error;
    private GameObject trigger_7;
    private GameObject go_작업발판;

    public void SetLadder(bool active)
    {
        SoundManager.instance.PlayEffect(eAudioClips.effect_click);
        ladder.SetActive(active);
        ladder_error.SetActive(!active);
        trigger_6.SetActive(active);
    }
    public void SetScaffolding(bool active)
    {
        SoundManager.instance.PlayEffect(eAudioClips.effect_click);
        work_scaffolding.SetActive(active);
        work_scaffolding_error.SetActive(!active);
        trigger_7.SetActive(active);
    }

    protected override void Awake()
    {
        base.Awake();
        //ui
        checkInstalls = DBManager.instance.CheckInstalls;

        go_Check_Install = ResourceManager.instance.LoadData<GameObject>(nameof(go_Check_Install));
        go_TapAni = gameObject.SearchGameObject(nameof(go_TapAni));
        sview_Check_Install = gameObject.Search<ScrollRect>(nameof(sview_Check_Install));
        content = sview_Check_Install.content;

        //component
        btn_Checked = gameObject.Search<Button>(nameof(btn_Checked));
        btn_Checked.onClick.AddListener(OnClick_Check_Install);
        btn_Image = btn_Checked.targetGraphic as Image;
        img_Checked = gameObject.Search<Image>(nameof(img_Checked));
        tmp_Checked = gameObject.Search<TMP_Text>(nameof(tmp_Checked));

        //trigger
        GameObject triggerCheck = ResourceManager.instance.LoadData<GameObject>(nameof(triggerCheck));
        var v = triggerCheck.GetComponentsInChildren<SpatialTriggerEvent>(true);
        foreach (var item in v)
        {
            int Index = int.Parse(item.name.Split("_")[1]);
            item.onEnterEvent.unityEvent.AddListener(() => OnTriggerEnter_Check_Install(Index));
            item.onExitEvent.unityEvent.AddListener(() => OnTriggerExit_Check_Install(Index));
        }

        //spatial interation
        ladder = ResourceManager.instance.LoadData<GameObject>(nameof(ladder));
        ladder_error = ResourceManager.instance.LoadData<GameObject>(nameof(ladder_error));
        trigger_6 = ResourceManager.instance.LoadData<GameObject>(nameof(trigger_6));
        go_사다리 = ResourceManager.instance.LoadData<GameObject>(nameof(go_사다리));

        work_scaffolding = ResourceManager.instance.LoadData<GameObject>(nameof(work_scaffolding));
        work_scaffolding_error = ResourceManager.instance.LoadData<GameObject>(nameof(work_scaffolding_error));
        trigger_7 = ResourceManager.instance.LoadData<GameObject>(nameof(trigger_7));
        go_작업발판 = ResourceManager.instance.LoadData<GameObject>(nameof(go_작업발판));

        target_CheckInstall = ResourceManager.instance.LoadData<GameObject>(nameof(target_CheckInstall));
    }
    private GameObject target_CheckInstall;

    public override void Open(Action act = null)
    {
        base.Open(act);

        //지도갱신
        packet_mapdata[] packet_Mapdatas = new packet_mapdata[] { new packet_mapdata("강관비계", "plan5") };
        packet_mapdata_root packet_Mapdata_Root = new packet_mapdata_root();
        packet_Mapdata_Root.title = "조립 시 안전 기준";
        packet_Mapdata_Root.packet_mapdatas = packet_Mapdatas;
        UIManager.instance.OpenPanel<panel_PlanMap>().SetData(packet_Mapdata_Root);
        SoundManager.instance.PlayVoice(eAudioClips.voice_3_toast_guide);
        UIManager.instance.ShowToast<toast_Basic>("표시된 지점으로 이동하여 점검을 완료하세요.")
            .SetData(new packet_toast_basic(eToastColor.blue, eToastIcon.toast_idle));

        ResetData();
        InitData();
        IsTriggered_Button(0, false);

        //텔레포트
        SpatialBridge.actorService.localActor.avatar.SetPositionRotation(target_CheckInstall.transform.position, target_CheckInstall.transform.rotation);

        //못나가게 콜라이더 활성화
        ResourceManager.instance.LoadData<GameObject>("wall").SetActive(true);

        //F 인터렉션 활성화
        go_사다리.SetActive(true);
        go_작업발판.SetActive(true);

        SetLadder(false);
        SetScaffolding(false);
    }
    
    public override void Close(Action act = null)
    {
        base.Close(act);

        //벽 콜라이더 제거
        ResourceManager.instance.LoadData<GameObject>("wall").SetActive(false);

        //F인터렉션 비활성화
        //go_사다리.SetActive(false);
        //go_작업발판.SetActive(false);
        SetLadder(false);
        SetScaffolding(false);

        //텔레포트
        SpatialBridge.actorService.localActor.avatar.SetPositionRotation(target_CheckInstall.transform.position, target_CheckInstall.transform.rotation);

    }
    protected override IEnumerator Action_Opening()
    {
        yield return null;
        //sview_Check_Install.verticalScrollbar.value = 0;
        Util.RefreshLayout(gameObject, "Content");
        Util.RefreshLayout(gameObject, "Content");
        Util.RefreshLayout(gameObject, "Content");
    }

    /// <summary>
    /// 기존 데이터 삭제
    /// </summary>
    private void ResetData()
    {
        Util.DestroyChildrenGameObject(content);
        go_Check_Installs.Clear();
        remainCheckCount = 0;
        img_Checked.gameObject.SetActive(false);
    }

    private void InitData()
    {
        for (int i = 0; i < checkInstalls.Count; i++)
        {
            GameObject go = Instantiate(go_Check_Install, content);
            go_Check_Install script = go.GetComponent<go_Check_Install>();

            CheckInstall checkInstall = checkInstalls[i].DeepCopy();
            script.SetData(checkInstall);

            go_Check_Installs.Add(checkInstall.index, script);

            remainCheckCount += checkInstall.isChecked ? 0 : 1;
        }
    }

    private void OnTriggerEnter_Check_Install(int index)
    {
        go_Check_Install go_Check_Install = go_Check_Installs[index];

        CheckInstall checkInstall = go_Check_Install.GetData();
        if (checkInstall.isChecked)
        {
            return;
        }
        IsTriggered_Button(checkInstall.index, true);
        ResourceManager.instance.LoadData<GameObject>("outline_" + index).SetActive(true);
    }

    private void OnTriggerExit_Check_Install(int index)
    {
        go_Check_Install go_Check_Install = go_Check_Installs[index];

        CheckInstall checkInstall = go_Check_Install.GetData();
        if (checkInstall.isChecked)
        {
            return;
        }
        IsTriggered_Button(checkInstall.index, false);
        ResourceManager.instance.LoadData<GameObject>("outline_" + index).SetActive(false);
    }

    private int triggeredIndex;

    /// <summary>
    ///  트리거 되었을 때 버튼 상태 변경
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isChecked"></param>
    public void IsTriggered_Button(int index, bool isChecked)
    {
        triggeredIndex = index;

        if (isChecked)
        {
            btn_Checked.interactable = true;
            btn_Image.sprite = ResourceManager.instance.LoadDataSprite("btn_370_enabled");
            tmp_Checked.text = (index + 1).ToString("00") + "번 항목 점검하기";
            go_TapAni.SetActive(true);
            go_Check_Installs[index].tmp_Summary.fontStyle = FontStyles.Bold;
        }
        else
        {
            btn_Checked.interactable = false;
            btn_Image.sprite = ResourceManager.instance.LoadDataSprite("btn_370_disabled");
            tmp_Checked.text = "점검하기";
            go_TapAni.SetActive(false);
            go_Check_Installs[index].tmp_Summary.fontStyle = FontStyles.Normal;
        }


    }

    /// <summary>
    /// 체크버튼 클릭
    /// </summary>
    public void OnClick_Check_Install()
    {
        SoundManager.instance.PlayEffect(eAudioClips.effect_click);
        if(remainCheckCount > 0)
        {
            //오브젝트 체크 갱신
            go_Check_Install go_Check_Install = go_Check_Installs[triggeredIndex];
            CheckInstall checkInstall = go_Check_Install.GetData();
            checkInstall.isChecked = true;
            go_Check_Install.SetData(checkInstall);

            //버튼 갱신
            IsTriggered_Button(triggeredIndex, false);
            ResourceManager.instance.LoadData<GameObject>("outline_" + triggeredIndex).SetActive(false);

            //횟수 차감
            remainCheckCount--;
            if(remainCheckCount == 0)//마지막 클릭이라면?
            {
                btn_Checked.interactable = false;
                btn_Image.sprite = ResourceManager.instance.LoadDataSprite("btn_370_finished");
                tmp_Checked.text = "점검완료";
                img_Checked.gameObject.SetActive(true);

                SoundManager.instance.PlayVoice(eAudioClips.voice_1_2_modal_end);
                popup_Success popup_Success = UIManager.instance.OpenPopup<popup_Success>();
                popup_Success.SetData(new packet_popup_Basic("완료", "<b>[작업 안전]</b>을 완료하였습니다.\n원하는 구역으로 이동하여 체험을 다시 진행할 수 있습니다."));
                popup_Success.SetAction(() =>
                {
                    UIManager.instance.HideToast<toast_Basic>();
                    UIManager.instance.ClosePanel<panel_PlanMap>();
                    UIManager.instance.GetPanel<panel_TopNavigation>().ResetStep();
                    UIManager.instance.OpenPanel<panel_TriggerMenu>();
                    Close();
                });
            }
        }
    }
}
