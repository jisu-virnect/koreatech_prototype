using SpatialSys.UnitySDK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;
using UnityEngine.UI;

public class panel_Install : panel_Base
{
    private int installIndex;
    private GameObject go_Install;

    private GameObject scaffold;
    private GameObject scaffold_wire;
    private scaffold01_1 scaffold01_1;

    private List<Install> installs;

    private List<go_Install> go_Installs = new List<go_Install>();
    private go_Install prevInstall = null;


    private Button btn_Next;
    private Button btn_Prev;
    private Button btn_Close;

    private Image img_Next;
    private TMP_Text tmp_Next;

    protected override void Awake()
    {
        base.Awake();
        GetComponent();
    }

    private void GetComponent()
    {
        installs = DBManager.instance.Installs;

        scaffold = ResourceManager.instance.LoadData<GameObject>(nameof(scaffold));
        scaffold01_1 = scaffold.GetComponent<scaffold01_1>();
        scaffold_wire = ResourceManager.instance.LoadData<GameObject>(nameof(scaffold_wire));

        btn_Next = gameObject.Search<Button>(nameof(btn_Next));
        btn_Next.onClick.AddListener(NextSequence);

        btn_Prev = gameObject.Search<Button>(nameof(btn_Prev));
        btn_Prev.onClick.AddListener(PrevSequence);

        btn_Close = gameObject.Search<Button>(nameof(btn_Close));
        btn_Close.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayEffect(eAudioClips.effect_close);
            Close();
        });

        ScrollRect sview_Install = gameObject.Search<ScrollRect>(nameof(sview_Install));
        content = sview_Install.content;

        //caching prefab
        go_Install = ResourceManager.instance.LoadData<GameObject>(nameof(go_Install));

        img_Next = gameObject.Search<Image>(nameof(img_Next));
        tmp_Next = gameObject.Search<TMP_Text>(nameof(tmp_Next));

        ClearData();
    }
    private RectTransform content;

    public override void Open(Action act = null)
    {
        base.Open();

        SetData();

        //ui
        packet_mapdata_root packet_Mapdata_Root = new packet_mapdata_root();
        packet_Mapdata_Root.title = "비계작업안전 시공도서";
        packet_Mapdata_Root.packet_mapdatas = new packet_mapdata[] { new packet_mapdata("평면도", "plan3"), new packet_mapdata("입면도", "plan4") }; ;
        UIManager.instance.OpenPanel<panel_PlanMap>(Define.before).SetData(packet_Mapdata_Root);

        SoundManager.instance.PlayVoice(eAudioClips.voice_2_toast_guide);
        UIManager.instance.ShowToast<toast_Basic>("비계 설치 또는 해체 버튼을 눌러 순서를 확인합니다.")
            .SetData(new packet_toast_basic(eToastColor.blue, eToastIcon.toast_idle));

        //카메라타겟 고정
        SpatialBridge.cameraService.SetTargetOverride(ResourceManager.instance.LoadData<GameObject>("target_Install").transform, SpatialCameraMode.Actor);

        //캐릭터고정
        Space_3.instance.Control_PlayerMovement(false);
    }
    public override void Close(Action act = null)
    {
        base.Close();
        ClearData();

        UIManager.instance.HideToast<toast_Basic>();
        UIManager.instance.GetPanel<panel_TopNavigation>().ResetStep();
        UIManager.instance.OpenPanel<panel_TriggerMenu>();
        UIManager.instance.ClosePanel<panel_PlanMap>();

        //카메라타겟 리셋
        SpatialBridge.cameraService.ClearTargetOverride();

        //캐릭터 이동가능
        Space_3.instance.Control_PlayerMovement(true);
    }
    private void SetData()
    {
        CreateSequenceUI();
        NextSequence();
    }

    private void ClearData()
    {
        installIndex = -1;
        prevInstall = null;
        DestroySequenceUI();
        scaffold01_1.Action_ResetObjects();
    }


    /// <summary>
    /// 시퀀스UI 생성
    /// </summary>
    /// <param name="item"></param>
    private void CreateSequenceUI()
    {
        scaffold_wire.SetActive(true);

        for (int i = 0; i < installs.Count; i++)
        {
            Install install = installs[i];
            go_Install temp = Instantiate(go_Install, content).GetComponent<go_Install>();
            temp.InitData(install);
            go_Installs.Add(temp);
        }
    }

    /// <summary>
    /// 시퀀스UI 삭제
    /// </summary>
    private void DestroySequenceUI()
    {
        scaffold_wire.SetActive(false);
        Util.DestroyChildrenGameObject(content);
        go_Installs.Clear();
    }

    /// <summary>
    /// 다음 시퀀스로 넘기기
    /// </summary>
    public void NextSequence()
    {
        SoundManager.instance.PlayEffect(eAudioClips.effect_next);
        if (prevInstall != null)
        {
            //prevInstall.SetData(SEQUENCE_STATE.BEFORE);
            scaffold01_1.Action_scaffold_RenderMode((eBuildScaffold)installIndex, BlendMode.Opaque, prevInstall.sequence);
            scaffold01_1.Action_scaffold_Position((eBuildScaffold)installIndex, BlendMode.Opaque, prevInstall.sequence);
        }

        if (go_Installs.Count > installIndex + 1)
        {
            installIndex++;
            prevInstall = go_Installs[installIndex];

            prevInstall.SetData(SEQUENCE_STATE.AFTER);
            scaffold01_1.Action_scaffold_RenderMode((eBuildScaffold)installIndex, BlendMode.Transparent, prevInstall.sequence);
            scaffold01_1.Action_scaffold_Position((eBuildScaffold)installIndex, BlendMode.Transparent, prevInstall.sequence);
            scaffold01_1.Action_scaffold_Active((eBuildScaffold)installIndex, true);

            //tmp_Next.text = (installIndex + 1).ToString() + "단계 설치하기";
        }
        else
        {
            //NextStep();
        }
    }

    /// <summary>
    /// 다음스탭 - 체크인스톨
    /// </summary>
    private void NextStep()
    {
        btn_Next.interactable = false;
        tmp_Next.text = "설치 완료";
        img_Next.gameObject.SetActive(true);

        UIManager.instance.ShowHideToast<toast_Basic>("[비계 설치] 단계가 완료되었습니다. [비계 점검] 단계로 넘어갑니다.", 3f, () =>
        {
            Close();

            //상단네비 다음스탭
            UIManager.instance.GetPanel<panel_TopNavigation>().NextStep();

            ////지도갱신
            //packet_mapdata[] packet_Mapdatas = new packet_mapdata[] { new packet_mapdata("강관비계", "plan5") };
            //packet_mapdata_root packet_Mapdata_Root = new packet_mapdata_root();
            //packet_Mapdata_Root.title = "조립 시 안전 기준";
            //packet_Mapdata_Root.packet_mapdatas = packet_Mapdatas;
            //UIManager.instance.GetPanel<panel_PlanMap>().SetData(packet_Mapdata_Root);

            UIManager.instance.OpenPanel<panel_Check_Install>();

        }).SetData(new packet_toast_basic(eToastColor.green, eToastIcon.toast_success));
    }

    public void GotoSequence(int sequence)
    {
        if (installIndex < sequence)
        {
            NextSequence();
            GotoSequence(sequence);
        }
        if (installIndex > sequence)
        {
            PrevSequence();
            GotoSequence(sequence);
        }
    }

    /// <summary>
    /// 이전시퀀스
    /// </summary>
    public void PrevSequence()
    {
        SoundManager.instance.PlayEffect(eAudioClips.effect_prev);
        if (installIndex > 0)
        {
            prevInstall.SetData(SEQUENCE_STATE.BEFORE);
            scaffold01_1.Action_scaffold_RenderMode((eBuildScaffold)installIndex, BlendMode.Opaque, prevInstall.sequence);
            scaffold01_1.Action_scaffold_Position((eBuildScaffold)installIndex, BlendMode.Opaque, prevInstall.sequence);
            scaffold01_1.Action_scaffold_Active((eBuildScaffold)installIndex, false);
            installIndex--;

            prevInstall = go_Installs[installIndex];
            //prevInstall.SetData(SEQUENCE_STATE.AFTER);
            scaffold01_1.Action_scaffold_RenderMode((eBuildScaffold)installIndex, BlendMode.Transparent, prevInstall.sequence);
            scaffold01_1.Action_scaffold_Position((eBuildScaffold)installIndex, BlendMode.Transparent, prevInstall.sequence);
            scaffold01_1.Action_scaffold_Active((eBuildScaffold)installIndex, true);
        }
    }

    private void OnEnable()
    {
        SpatialBridge.networkingService.remoteEvents.onEvent += HandleEventReceived;
        SpatialBridge.networkingService.onMasterClientChanged += HandleMasterClientChanged;
    }

    private void OnDisable()
    {
        SpatialBridge.networkingService.remoteEvents.onEvent -= HandleEventReceived;
        SpatialBridge.networkingService.onMasterClientChanged -= HandleMasterClientChanged;
    }

    private void HandleMasterClientChanged(int actorNumber)
    {

    }

    Dictionary<string, object> serverProperties = new Dictionary<string, object>();


    private void HandleEventReceived(NetworkingRemoteEventArgs args)
    {
        switch ((RemoteEventIDs)args.eventID)
        {
            case RemoteEventIDs.SendMagicNumber:
                break;
            case RemoteEventIDs.PrivateMessage:
                SpatialBridge.coreGUIService.DisplayToastMessage((string)args.eventArgs[0]);
                break;
            case RemoteEventIDs.Install:
                NextSequence();

                if (!serverProperties.ContainsKey("idinstall"))
                    serverProperties.Add("idinstall", installIndex);
                else
                    serverProperties["idinstall"] = installIndex;

                // SetServerProperties 메서드 호출
                SpatialBridge.networkingService.SetServerProperties(serverProperties);
                break;
            case RemoteEventIDs.Uninstall:
                PrevSequence();
                break;
            default:
                break;
        }
    }
}
