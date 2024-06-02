using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;
using TMPro;
using System;
using Cinemachine;
using System.Linq;
using System.Runtime;

/// <summary>
/// 씬 기준 매니저
/// </summary>
public class Space_3 : MonoBehaviour, IAvatarInputActionsListener
{
    public static Space_3 instance;
    public Dictionary<eVirtualCameraState, CinemachineVirtualCamera> virtualCameras { get; private set; }
        = new Dictionary<eVirtualCameraState, CinemachineVirtualCamera>();


    #region unity
    private void Awake()
    {
        instance = this;
        InitVirtualCamera();
    }
    private void Start()
    {
        Trigger_World();
    }
    private void Update()
    {
        ChangeAvatar();
    }
    private void OnEnable()
    {
        AddHandler();
    }
    private void OnDisable()
    {
        RemoveHandler();
    }
    #endregion

    #region virtualCamera

    CinemachineVirtualCamera prevVirtualCamera = null;

    /// <summary>
    /// 버추얼카메라 초기화
    /// </summary>
    private void InitVirtualCamera()
    {
        CinemachineVirtualCamera[] cinemachineVirtualCameras = FindObjectsOfType<CinemachineVirtualCamera>(true);
        for (int i = 0; cinemachineVirtualCameras.Length > i; i++)
        {
            CinemachineVirtualCamera cinemachineVirtualCamera = cinemachineVirtualCameras[i];
            eVirtualCameraState eVirtualCameraState = Util.String2Enum<eVirtualCameraState>(cinemachineVirtualCamera.name);
            virtualCameras.Add(eVirtualCameraState, cinemachineVirtualCamera);
            cinemachineVirtualCamera.enabled = false;
        }
    }

    /// <summary>
    /// 제어 버추얼카메라
    /// </summary>
    /// <param name="eVirtualCameraState"></param>
    /// <returns></returns>
    public CinemachineVirtualCamera Control_VirtualCamera(eVirtualCameraState eVirtualCameraState = eVirtualCameraState.none)
    {
        if (prevVirtualCamera != null)
        {
            prevVirtualCamera.enabled = false;
            prevVirtualCamera = null;
        }
        if (virtualCameras.ContainsKey(eVirtualCameraState))
        {
            prevVirtualCamera = virtualCameras[eVirtualCameraState];
            prevVirtualCamera.enabled = true;
            return prevVirtualCamera;
        }
        return null;
    }
    #endregion

    #region common
    /// <summary>
    /// 아바타 변경
    /// </summary>
    private void ChangeAvatar()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SpatialBridge.actorService.localActor.avatar.SetAvatarBody(AssetType.EmbeddedAsset, "0");
        if (Input.GetKeyDown(KeyCode.Alpha2)) SpatialBridge.actorService.localActor.avatar.SetAvatarBody(AssetType.EmbeddedAsset, "1");
        if (Input.GetKeyDown(KeyCode.Alpha3)) SpatialBridge.actorService.localActor.avatar.SetAvatarBody(AssetType.EmbeddedAsset, "2");
        if (Input.GetKeyDown(KeyCode.Alpha4)) SpatialBridge.actorService.localActor.avatar.SetAvatarBody(AssetType.EmbeddedAsset, "3");
    }
    #endregion

    #region handler

    /// <summary>
    /// 핸들러 등록
    /// </summary>
    private void AddHandler()
    {
        SpatialBridge.networkingService.onConnectionStatusChanged += HandleConnectionStatusChanged; // 서버상태 변경
        SpatialBridge.networkingService.remoteEvents.onEvent += HandleEventReceived; // RPC
        SpatialBridge.actorService.onActorJoined += HandlerActorJoined; // 멤버 접속
        SpatialBridge.actorService.onActorLeft += HandlerActorLeft; // 멤버 나감
    }

    /// <summary>
    /// 핸들러 제거
    /// </summary>
    private void RemoveHandler()
    {
        SpatialBridge.networkingService.onConnectionStatusChanged -= HandleConnectionStatusChanged; // 서버상태 변경
        SpatialBridge.networkingService.remoteEvents.onEvent -= HandleEventReceived; // RPC
        SpatialBridge.actorService.onActorJoined -= HandlerActorJoined; // 멤버 접속
        SpatialBridge.actorService.onActorLeft -= HandlerActorLeft; // 멤버 나감
    }

    /// <summary>
    /// 접속상태변경 이벤트
    /// </summary>
    /// <param name="status"></param>
    private void HandleConnectionStatusChanged(ServerConnectionStatus status)
    {
        switch (status)
        {
            case ServerConnectionStatus.Disconnected:
                break;
            case ServerConnectionStatus.Connecting:
                break;
            case ServerConnectionStatus.Connected:
                InitSpatialService();
                InitGetServerProperties();
                InitAddMember();
                break;
            case ServerConnectionStatus.Disconnecting:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 스페이셜서비스 초기화
    /// </summary>
    void InitSpatialService()
    {
        //이모지 비활성화
        SpatialBridge.inputService.SetEmoteBindingsEnabled(false);

        //기본 UI 변경
        SpatialBridge.coreGUIService.SetCoreGUIOpen(SpatialCoreGUITypeFlags.All, false);
        SpatialBridge.coreGUIService.SetCoreGUIOpen(SpatialCoreGUITypeFlags.ParticipantsList, true);

        //카메라 회전 변경
        SpatialBridge.cameraService.rotationMode = SpatialCameraRotationMode.DragToRotate;

        //아바타 기본 바꾸기
        SpatialBridge.actorService.localActor.avatar.SetAvatarBody(AssetType.EmbeddedAsset, "3");
    }

    /// <summary>
    /// 최초 서버에서 받는 프로퍼로 초기화
    /// </summary>
    private void InitGetServerProperties()
    {
        IReadOnlyDictionary<string, object> getServerProperties = SpatialBridge.networkingService.GetServerProperties();

        foreach (var item in getServerProperties.Keys)
        {
            RemoteEventIDs remoteEventID = Util.String2Enum<RemoteEventIDs>(item.ToString());
            switch (remoteEventID)
            {
                case RemoteEventIDs.SendMagicNumber:
                    break;
                case RemoteEventIDs.PrivateMessage:
                    break;
                case RemoteEventIDs.SpaceState:
                    {
                        RemoteEventSubIDs_Space remoteEventSubID = Util.String2Enum<RemoteEventSubIDs_Space>(remoteEventID.ToString());
                        switch (remoteEventSubID)
                        {
                            case RemoteEventSubIDs_Space.before:
                                break;
                            case RemoteEventSubIDs_Space.install:
                                UIManager.instance.OpenPanel<panel_Install>();
                                break;
                            case RemoteEventSubIDs_Space.after:
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// event response
    /// </summary>
    /// <param name="args"></param>
    private void HandleEventReceived(NetworkingRemoteEventArgs args)
    {
        RemoteEventIDs remoteEventIDs = (RemoteEventIDs)args.eventID;
        object[] eventArgs = args.eventArgs;
        switch (remoteEventIDs)
        {
            case RemoteEventIDs.SpaceState:
                {
                    string remoteEventSub = eventArgs[0].ToString();
                    Dictionary<string, object> serverProperties = new Dictionary<string, object>();
                    serverProperties.Add(RemoteEventIDs.SpaceState.ToString(), remoteEventSub);
                    SpatialBridge.networkingService.SetServerProperties(serverProperties);

                    RemoteEventSubIDs_Space remoteEventSubIDs = Util.String2Enum<RemoteEventSubIDs_Space>(remoteEventSub);
                    switch (remoteEventSubIDs)
                    {
                        case RemoteEventSubIDs_Space.world:
                            Trigger_World();
                            break;
                        case RemoteEventSubIDs_Space.before:
                            Trigger_BeforeZone();
                            break;
                        case RemoteEventSubIDs_Space.install:
                            Trigger_InstallZone();
                            break;
                        case RemoteEventSubIDs_Space.after:
                            Trigger_AfterZone();
                            break;
                        default:
                            break;
                    }
                }
                break;
            default:
                break;
        }
    }

    //멤버
    /// <summary>
    /// 액터 조인 핸들러
    /// </summary>
    /// <param name="args"></param>
    private void HandlerActorJoined(ActorJoinedEventArgs args)
    {
        IActor actor = SpatialBridge.actorService.actors[args.actorNumber];
        AddMember(actor);
    }

    /// <summary>
    /// 액터 나감 핸들러
    /// </summary>
    /// <param name="args"></param>
    private void HandlerActorLeft(ActorLeftEventArgs args)
    {
        IActor actor = SpatialBridge.actorService.actors[args.actorNumber];
        RemoveMember(actor);
    }

    /// <summary>
    /// 최초 방 진입시 멤버추가
    /// </summary>
    private void InitAddMember()
    {
        IEnumerable<IActor> actors = SpatialBridge.actorService.actors.Values;
        foreach (IActor actor in actors)
        {
            AddMember(actor);
        }
    }
    private void AddMember(IActor actor)
    {
        UIManager.instance.GetPanel<panel_CheckMember>().AddMember(actor.isSpaceOwner, actor.displayName);
    }
    private void RemoveMember(IActor actor)
    {
        UIManager.instance.GetPanel<panel_CheckMember>().RemoveMember(actor.displayName);
    }

    #endregion

    #region trigger
    public void Trigger_World()
    {
        UIManager.instance.ClosePanels(Define.trigger);

        UIManager.instance.OpenPanel<panel_GlobalMessage>(Define.world);
        UIManager.instance.OpenPanel<panel_MiniMap>(Define.world);
        UIManager.instance.OpenPanel<panel_CustomKey>(Define.world);
    }
    public void Trigger_BeforeZone()
    {
        UIManager.instance.ClosePanels(Define.world);

        Section section = DBManager.instance.Sections.FirstOrDefault(x => x.index == 0);
        UIManager.instance.OpenPanel<panel_TopNavigation>(Define.trigger).SetData(section);
        UIManager.instance.OpenPanel<panel_TriggerMenu>(Define.trigger).SetData(section);
    }
    public void Trigger_InstallZone()
    {
        UIManager.instance.ClosePanels(Define.world);

        Section section = DBManager.instance.Sections.FirstOrDefault(x => x.index == 1);
        UIManager.instance.OpenPanel<panel_TopNavigation>(Define.trigger).SetData(section);
        UIManager.instance.OpenPanel<panel_TriggerMenu>(Define.trigger).SetData(section);
    }
    public void Trigger_AfterZone()
    {
        UIManager.instance.ClosePanels(Define.world);

        Section section = DBManager.instance.Sections.FirstOrDefault(x => x.index == 2);
        UIManager.instance.OpenPanel<panel_TopNavigation>(Define.trigger).SetData(section);
        UIManager.instance.OpenPanel<panel_TriggerMenu>(Define.trigger).SetData(section);
    }
    #endregion

    #region test mode
    private eServiceMode serviceMode = eServiceMode.None;
    private void TestMode()
    {
        //LocalAvatarName();
        //LookAtCamera(HMD.transform);
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                serviceMode = eServiceMode.None;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                serviceMode = eServiceMode.Network;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                serviceMode = eServiceMode.Camera;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                serviceMode = eServiceMode.Input;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                serviceMode = eServiceMode.UI;
            }
        }
        else
        {
            switch (serviceMode)
            {
                case eServiceMode.None:
                    NoneMode();
                    break;
                case eServiceMode.Network:
                    NetworkMode();
                    break;
                case eServiceMode.Camera:
                    CameraMode();
                    break;
                case eServiceMode.Input:
                    InputMode();
                    break;
                case eServiceMode.UI:
                    UIMode();
                    break;
                default:
                    break;
            }
        }
    }
    void ToastMessage(string message)
    {
        SpatialBridge.coreGUIService.DisplayToastMessage(message);
    }
    void NoneMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
        }
    }

    public void Control_PlayerMovement(bool active)
    {
        StartCoroutine(Co_Control_PlayerMovement(active));
    }
    private IEnumerator Co_Control_PlayerMovement(bool active)
    {
        yield return null;
        if (active)
        {
            SpatialBridge.inputService.ReleaseInputCapture(this);
        }
        else
        {
            SpatialBridge.inputService.StartAvatarInputCapture(true, true, true, true, this);
        }
    }

    void NetworkMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (!SpatialBridge.networkingService.isMasterClient)
            {
                return;
            }
            SpatialBridge.networkingService.remoteEvents.RaiseEventAll((byte)RemoteEventIDs.PrivateMessage, new object[] { Time.time.ToString() });
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SpatialBridge.networkingService.remoteEvents.RaiseEventAll((byte)RemoteEventIDs.PrivateMessage, new object[] { Time.time.ToString() });
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SpatialBridge.networkingService.remoteEvents.RaiseEventAll((byte)RemoteEventIDs.PrivateMessage, new object[] { Time.time.ToString() });
        }
    }

    private float dist = 0f;
    void CameraMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpatialBridge.cameraService.lockCameraRotation = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpatialBridge.cameraService.lockCameraRotation = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpatialBridge.cameraService.rotationMode = SpatialCameraRotationMode.DragToRotate;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            dist = SpatialBridge.cameraService.zoomDistance;
            ToastMessage(dist.ToString());
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SpatialBridge.cameraService.zoomDistance = dist;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
        }
    }
    void InputMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Control_PlayerMovement(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Control_PlayerMovement(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
        }
    }
    void UIMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
        }
    }
    public void SetAllAvatarsVisibilityLocally(bool visible)
    {
        foreach (var actor in SpatialBridge.actorService.actors.Values)
        {
            if (actor != SpatialBridge.actorService.localActor)
            {
                actor.avatar.visibleLocally = visible;
            }
        }
    }
    #endregion

    #region avatarName
    public GameObject HMD;
    public TMP_Text tmp_LocalAvatarName;

    private void LocalAvatarName()
    {
        var avator = SpatialBridge.actorService.localActor.avatar;
        var head = avator.GetAvatarBoneTransform(HumanBodyBones.Head);
        if (HMD != null && head != null)
        {
            HMD.transform.position = head.position + Vector3.up * 0.3f;
            if (tmp_LocalAvatarName != null)
            {
                tmp_LocalAvatarName.text = SpatialBridge.actorService.localActor.displayName;
            }
        }
    }
    private void LookAtCamera(Transform tr)
    {
        var camera = SpatialBridge.cameraService;
        Debug.Log(camera.position);
        tr.LookAt(2 * tr.position - camera.position);
    }
    #endregion

    #region control movement (wasd)
    public void OnAvatarMoveInput(InputPhase inputPhase, Vector2 inputMove) { }

    public void OnAvatarJumpInput(InputPhase inputPhase) { }

    public void OnAvatarSprintInput(InputPhase inputPhase) { }

    public void OnAvatarActionInput(InputPhase inputPhase) { }

    public void OnAvatarAutoSprintToggled(bool on) { }

    public void OnInputCaptureStarted(InputCaptureType type) { }

    public void OnInputCaptureStopped(InputCaptureType type) { }
    #endregion
}