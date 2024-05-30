using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panel_Install : panel_Base
{
    public GameObject prefab_go_Sequence;

    public scaffold01_1 scaffold01_1;

    private GameObject go_SequencePreviewRoot;
    private int sequenceIndex;

    private Button btn_Next;
    private Button btn_Prev;
    private Button btn_Close;

    private List<Space_3_Sequence> sequences = new List<Space_3_Sequence>();
    private Space_3_Sequence prevSequence = null;

    protected override void Awake()
    {
        base.Awake();
        GetComponent();
    }

    private void GetComponent()
    {
        go_SequencePreviewRoot = gameObject.Search(nameof(go_SequencePreviewRoot)).gameObject;

        btn_Next = gameObject.Search<Button>(nameof(btn_Next));
        btn_Next.onClick.AddListener(NextSequence);

        btn_Prev = gameObject.Search<Button>(nameof(btn_Prev));
        btn_Prev.onClick.AddListener(PrevSequence);

        btn_Close = gameObject.Search<Button>(nameof(btn_Close));
        btn_Close.onClick.AddListener(Close);
    }

    public override void Open()
    {
        base.Open();
        ResetSequence();
        Space_3.instance.Control_PlayerMovement(false);
        Space_3.instance.Control_VirtualCamera(eVirtualCameraState.vcam_install);
    }

    public override void Close()
    {
        base.Close();
        sequenceIndex = -1;
        prevSequence = null;
        scaffold01_1.Action_ResetObjects();
        DestroySequenceUI();

        UIManager.instance.OpenPanel<panel_TriggerMenu>();

        Space_3.instance.Control_PlayerMovement(true);
        Space_3.instance.Control_VirtualCamera(eVirtualCameraState.none);
    }

    private void Update()
    {
//        if (!SpatialBridge.networkingService.isMasterClient)
//        {
//            return;
//        }

//        if (Input.GetKeyDown(KeyCode.Alpha1))
//        {
//#if UNITY_EDITOR
//            PrevSequence();
//#else
//            SpatialBridge.networkingService.remoteEvents.RaiseEventAll((byte)RemoteEventIDs.Uninstall);
//#endif
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha2))
//        {
//#if UNITY_EDITOR
//            NextSequence();
//#else
//            SpatialBridge.networkingService.remoteEvents.RaiseEventAll((byte)RemoteEventIDs.Install);
//#endif
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha3))
//        {
//            ResetSequence();
//        }
    }



    /// <summary>
    /// 시퀀스데이터 초기화
    /// </summary>
    /// <param name="item"></param>
    private void CreateSequenceUI()
    {
        for (int i = 0; i < DBManager.instance.Installs.Count; i++)
        {
            Install install = DBManager.instance.Installs[i];
            Space_3_Sequence space_3_Sequence = Instantiate(prefab_go_Sequence, go_SequencePreviewRoot.transform).GetComponent<Space_3_Sequence>();
            sequences.Add(space_3_Sequence);
            space_3_Sequence.SetData(install);
        }
    }

    /// <summary>
    /// 시퀀스 삭제
    /// </summary>
    private void DestroySequenceUI()
    {
        for (int i = go_SequencePreviewRoot.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(go_SequencePreviewRoot.transform.GetChild(i).gameObject);
        }
        sequences.Clear();
    }


    public void ResetSequence()
    {
        sequenceIndex = -1;
        prevSequence = null;
        scaffold01_1.Action_ResetObjects();
        DestroySequenceUI();
        CreateSequenceUI();
        NextSequence();
    }

    public void NextSequence()
    {
        if (prevSequence != null)
        {
            prevSequence.SetSequenceState(SEQUENCE_STATE.AFTER);
            scaffold01_1.Action_scaffold_RenderMode((eBuildScaffold)sequenceIndex, BlendMode.Opaque, prevSequence.sequence);
            scaffold01_1.Action_scaffold_Position((eBuildScaffold)sequenceIndex, BlendMode.Opaque, prevSequence.sequence);
        }
        if (sequences.Count > sequenceIndex + 1)
        {
            sequenceIndex++;
            prevSequence = sequences[sequenceIndex];

            prevSequence.SetSequenceState(SEQUENCE_STATE.FOCUS);
            scaffold01_1.Action_scaffold_RenderMode((eBuildScaffold)sequenceIndex, BlendMode.Transparent, prevSequence.sequence);
            scaffold01_1.Action_scaffold_Position((eBuildScaffold)sequenceIndex, BlendMode.Transparent, prevSequence.sequence);
            scaffold01_1.Action_scaffold_Active((eBuildScaffold)sequenceIndex, true);

        }
    }

    public void GotoSequence(int sequence)
    {
        if (sequenceIndex < sequence)
        {
            NextSequence();
            GotoSequence(sequence);
        }
        if (sequenceIndex > sequence)
        {
            PrevSequence();
            GotoSequence(sequence);
        }
    }

    public void PrevSequence()
    {
        if (sequenceIndex > 0)
        {
            prevSequence.SetSequenceState(SEQUENCE_STATE.BEFORE);
            scaffold01_1.Action_scaffold_RenderMode((eBuildScaffold)sequenceIndex, BlendMode.Opaque, prevSequence.sequence);
            scaffold01_1.Action_scaffold_Position((eBuildScaffold)sequenceIndex, BlendMode.Opaque, prevSequence.sequence);
            scaffold01_1.Action_scaffold_Active((eBuildScaffold)sequenceIndex, false);
            sequenceIndex--;

            prevSequence = sequences[sequenceIndex];
            prevSequence.SetSequenceState(SEQUENCE_STATE.FOCUS);
            scaffold01_1.Action_scaffold_RenderMode((eBuildScaffold)sequenceIndex, BlendMode.Transparent, prevSequence.sequence);
            scaffold01_1.Action_scaffold_Position((eBuildScaffold)sequenceIndex, BlendMode.Transparent, prevSequence.sequence);
            scaffold01_1.Action_scaffold_Active((eBuildScaffold)sequenceIndex, true);
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

                if(!serverProperties.ContainsKey("idinstall"))
                    serverProperties.Add("idinstall", sequenceIndex);
                else
                    serverProperties["idinstall"] = sequenceIndex;

                // SetServerProperties 메서드 호출
                SpatialBridge.networkingService.SetServerProperties(serverProperties);
                break;
            case RemoteEventIDs.Checklist:
                break;
            case RemoteEventIDs.Checkout:
                break;
            case RemoteEventIDs.Uninstall:
                PrevSequence();
                break;
            default:
                break;
        }
    }
}
