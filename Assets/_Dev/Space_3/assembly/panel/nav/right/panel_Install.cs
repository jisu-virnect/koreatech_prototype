using SpatialSys.UnitySDK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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
    //private Button btn_Prev;
    //private Button btn_Close;
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

        //btn_Prev = gameObject.Search<Button>(nameof(btn_Prev));
        //btn_Prev.onClick.AddListener(PrevSequence);

        //btn_Close = gameObject.Search<Button>(nameof(btn_Close));
        //btn_Close.onClick.AddListener(()=> Close());

        ScrollRect sview_Install = gameObject.Search<ScrollRect>(nameof(sview_Install));
        content = sview_Install.content;

        //caching prefab
        go_Install = ResourceManager.instance.LoadData<GameObject>(nameof(go_Install));

        img_Next = gameObject.Search<Image>(nameof(img_Next));
        tmp_Next = gameObject.Search<TMP_Text>(nameof(tmp_Next));

    }
    private RectTransform content;

    public override void Open(Action act = null)
    {
        base.Open();

        ResetSequence();

        //Space_3.instance.Control_PlayerMovement(false);
        //Space_3.instance.Control_VirtualCamera(eVirtualCameraState.vcam_install);
    }

    public override void Close(Action act = null)
    {
        base.Close();

        prevInstall = null;
        installIndex = -1;

        //scaffold01_1.Action_ResetObjects();
        DestroySequenceUI();


        //Section section = DBManager.instance.Sections.FirstOrDefault(x => x.index == 1);

        //UIManager.instance.OpenPanel<panel_TopNavigation>(Define.trigger).SetData(section);
        //UIManager.instance.OpenPanel<panel_TriggerMenu>(Define.trigger).SetData(section);

        //Space_3.instance.Control_PlayerMovement(true);
        //Space_3.instance.Control_VirtualCamera(eVirtualCameraState.none);

        SpatialBridge.cameraService.ClearTargetOverride();

        UIManager.instance.OpenPanel<panel_TopNavigation>().NextStep();
        UIManager.instance.OpenPanel<panel_Check_Install>();
    }

    public void ResetSequence()
    {
        installIndex = -1;
        prevInstall = null;

        btn_Next.interactable = true;
        img_Next.gameObject.SetActive(false);

        scaffold01_1.Action_ResetObjects();

        DestroySequenceUI();
        CreateSequenceUI();
        NextSequence();

        GameObject target_CheckInstall = ResourceManager.instance.LoadData<GameObject>(nameof(target_CheckInstall));
        SpatialBridge.actorService.localActor.avatar.SetPositionRotation(target_CheckInstall.transform.position, target_CheckInstall.transform.rotation);

        SpatialBridge.cameraService.SetTargetOverride(ResourceManager.instance.LoadData<GameObject>("target_Install").transform, SpatialCameraMode.Actor);
    }

    /// <summary>
    /// 시퀀스 삭제
    /// </summary>
    private void DestroySequenceUI()
    {
        scaffold_wire.SetActive(false);

        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        go_Installs.Clear();
    }

    /// <summary>
    /// 시퀀스데이터 초기화
    /// </summary>
    /// <param name="item"></param>
    private void CreateSequenceUI()
    {
        scaffold_wire.SetActive(true);

        for (int i = 0; i < installs.Count; i++)
        {
            Install install = installs[i];
            go_Install temp = Instantiate(go_Install, content).GetComponent<go_Install>();
            go_Installs.Add(temp);
            temp.InitData(install);
        }
    }

    /// <summary>
    /// 다음 시퀀스로 넘기기
    /// </summary>
    public void NextSequence()
    {
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

            tmp_Next.text = (installIndex + 1).ToString() + "단계 설치하기";
        }
        else
        {
            btn_Next.interactable = false;
            tmp_Next.text = "설치 완료";
            img_Next.gameObject.SetActive(true);

            UIManager.instance.ShowHideToast<toast_Basic>("[비계 설치] 단계가 완료되었습니다. [비계 점검] 단계로 넘어갑니다.", 3f, () =>
            {
                UIManager.instance.GetPanel<panel_TopNavigation>().NextStep();
                Close();

            }).SetData(new packet_toast_basic(eToastColor.green, eToastIcon.toast_success));
        }

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

    public void PrevSequence()
    {
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
