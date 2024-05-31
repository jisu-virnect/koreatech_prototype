using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;
using TMPro;
using System;
using Cinemachine;
using System.Linq;

public class Space_3 : MonoBehaviour, IAvatarInputActionsListener
{
    public Dictionary<eVirtualCameraState, CinemachineVirtualCamera> virtualCameras { get; private set; } = new Dictionary<eVirtualCameraState, CinemachineVirtualCamera>();
    public static Space_3 instance;
    public TMP_Text tmp_Mode;
    public GameObject HMD;
    public TMP_Text tmp_LocalAvatarName;


    public Transform target;
    private float dist = 0f;

    private eServiceMode serviceMode = eServiceMode.None;

    private void Awake()
    {
        instance = this;
        GetVirtualCamera();
    }
    private void Start()
    {
        Trigger_World();
    }

    private void GetVirtualCamera()
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

    CinemachineVirtualCamera prevVirtualCamera = null;

    public void Control_VirtualCamera(eVirtualCameraState eVirtualCameraState = eVirtualCameraState.none)
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
        }
    }

    private void Update()
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
            tmp_Mode.text = serviceMode.ToString();
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

    private void OnEnable()
    {
        SpatialBridge.networkingService.remoteEvents.onEvent += HandleEventReceived;
        SpatialBridge.networkingService.onConnectionStatusChanged += HandleConnectionStatusChanged;
    }

    private void OnDisable()
    {
        SpatialBridge.networkingService.remoteEvents.onEvent -= HandleEventReceived;
        SpatialBridge.networkingService.onConnectionStatusChanged -= HandleConnectionStatusChanged;
    }

    #region handler
    /// <summary>
    /// 스페이셜 접속상태변경 이벤트
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

                SpatialBridge.coreGUIService.SetCoreGUIOpen(SpatialCoreGUITypeFlags.All, false);
                SpatialBridge.coreGUIService.SetCoreGUIOpen(SpatialCoreGUITypeFlags.ParticipantsList, true);
                SpatialBridge.cameraService.rotationMode = SpatialCameraRotationMode.DragToRotate;
                FirstGetServerProperties();
                break;
            case ServerConnectionStatus.Disconnecting:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 최초 서버에서 받는 프로퍼티(현재 방 상태에 따른 UI 변경)
    /// </summary>
    private void FirstGetServerProperties()
    {
        IReadOnlyDictionary<string, object> getServerProperties = SpatialBridge.networkingService.GetServerProperties();

        if (getServerProperties.ContainsKey(RemoteEventIDs.SpaceState.ToString()))
        {
            switch (Util.String2Enum<RemoteEventSubIDs>((string)getServerProperties[RemoteEventIDs.SpaceState.ToString()]))
            {
                case RemoteEventSubIDs.install:
                    UIManager.instance.OpenPanel<panel_Install>();
                    break;
                case RemoteEventSubIDs.before:
                    break;
                case RemoteEventSubIDs.after:
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
        switch (remoteEventIDs)
        {
            case RemoteEventIDs.SpaceState:
                {
                    RemoteEventSubIDs remoteEventSubIDs = Util.String2Enum<RemoteEventSubIDs>((string)args.eventArgs[0]);
                    SpatialBridge.networkingService.SetServerProperties(new Dictionary<string, object> { { RemoteEventIDs.SpaceState.ToString(), remoteEventSubIDs.ToString() } });
                    switch (remoteEventSubIDs)
                    {
                        case RemoteEventSubIDs.world:
                            Trigger_World();
                            break;
                        case RemoteEventSubIDs.before:
                            Trigger_BeforeZone();
                            break;
                        case RemoteEventSubIDs.install:
                            Trigger_InstallZone();
                            break;
                        case RemoteEventSubIDs.after:
                            Trigger_AfterZone();
                            break;
                        default:
                            break;
                    }
                }
                break;
            case RemoteEventIDs.Install:
            case RemoteEventIDs.Uninstall:
            case RemoteEventIDs.Checklist:
            case RemoteEventIDs.Checkout:
            case RemoteEventIDs.SendMagicNumber:
            case RemoteEventIDs.PrivateMessage:
            default:
                break;
        }
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
    void ToastMessage(string message)
    {
        SpatialBridge.coreGUIService.DisplayToastMessage(message);
    }
    void NoneMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UIManager.instance.OpenPanel<panel_TopNavigation>("a");
            UIManager.instance.OpenPanel<panel_Install>("a");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UIManager.instance.ClosePanels("a");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UIManager.instance.ShowHideToast<toast_Base>("aaa", 1f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //UIManager.instance.ShowHideToast<toast_Base>("bbb");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            UIManager.instance.ShowHideToast<toast_Base>("ccc", 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            UIManager.instance.OpenPanel<panel_GlobalMessage>().SetData<string>("aaaa");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            UIManager.instance.OpenPanel<panel_GlobalMessage>().SetData<string>("bbbbb");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            UIManager.instance.OpenPanel<panel_TopNavigation>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
        }
    }

    public void Control_PlayerMovement(bool active)
    {
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
            UIManager.instance.OpenPanel<panel_Install>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UIManager.instance.ClosePanel<panel_Install>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UIManager.instance.OpenPanel<panel_Check_Install>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UIManager.instance.ClosePanel<panel_Check_Install>();
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

    #region etc

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