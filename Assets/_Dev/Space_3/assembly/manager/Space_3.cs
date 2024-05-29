using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;
using TMPro;
using System;

public class Space_3 : MonoBehaviour, IAvatarInputActionsListener
{
    public static Space_3 instance;
    public TMP_Text tmp_Mode;
    public GameObject HMD;
    public TMP_Text tmp_LocalAvatarName;
    public List<SpatialTriggerEvent> triggerEvents = new List<SpatialTriggerEvent>();

    public Transform target;
    float dist = 0f;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
    }

    private void Update()
    {
        //LocalAvatarName();
        //LookAtCamera(HMD.transform);
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
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
                SetTriggerEvent();
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
                case RemoteEventSubIDs.None:
                    break;
                case RemoteEventSubIDs.Install:
                    Space_3_SequenceManager.instance.OpenPanel<panel_Install>();
                    break;
                case RemoteEventSubIDs.Uninstall:
                    break;
                case RemoteEventSubIDs.Checklist:
                    break;
                case RemoteEventSubIDs.Checkout:
                    break;
                default:
                    break;
            }
        }
    }


    #region Trigger
    /// <summary>
    /// 트리거 됨에 따른 상태 변경
    /// </summary>
    private void SetTriggerEvent()
    {
        if (!SpatialBridge.networkingService.isMasterClient)
        {
            return;
        }

        for (int i = 0; i < triggerEvents.Count; i++)
        {
            string name = triggerEvents[i].name;
            triggerEvents[i].onEnterEvent.unityEvent.AddListener(() => OnTriggerEnter_Spatial(name));
            triggerEvents[i].onExitEvent.unityEvent.AddListener(() => OnTriggerExit_Spatial(name));
        }
    }
    private void OnTriggerEnter_Spatial(string name)
    {
        var remoteEventSubIDs = Util.String2Enum<RemoteEventSubIDs>(name);
        switch (remoteEventSubIDs)
        {
            case RemoteEventSubIDs.Install:
                SpatialBridge.networkingService.remoteEvents.RaiseEventAll((byte)RemoteEventIDs.SpaceState, new object[] { remoteEventSubIDs.ToString() });
                break;
            case RemoteEventSubIDs.Uninstall:
                break;
            case RemoteEventSubIDs.Checklist:
                break;
            case RemoteEventSubIDs.Checkout:
                break;
            case RemoteEventSubIDs.None:
                break;
            default:
                break;
        }
    }
    public GameObject PolygonGrid_Glow;
    private void OnTriggerExit_Spatial(string name)
    {
        var remoteEventSubIDs = Util.String2Enum<RemoteEventSubIDs>(name);
        Debug.Log(remoteEventSubIDs);
        switch (remoteEventSubIDs)
        {
            case RemoteEventSubIDs.Install:
                SpatialBridge.networkingService.remoteEvents.RaiseEventAll((byte)RemoteEventIDs.SpaceState, new object[] { RemoteEventSubIDs.None.ToString() });
                break;
            case RemoteEventSubIDs.Uninstall:
                break;
            case RemoteEventSubIDs.Checklist:
                break;
            case RemoteEventSubIDs.Checkout:
                break;
            default:
                break;
        }
    }
    private void HandleEventReceived(NetworkingRemoteEventArgs args)
    {
        RemoteEventIDs remoteEventIDs = (RemoteEventIDs)args.eventID;
        switch (remoteEventIDs)
        {
            case RemoteEventIDs.SendMagicNumber:
                break;
            case RemoteEventIDs.PrivateMessage:
                break;
            case RemoteEventIDs.SpaceState:
                RemoteEventSubIDs remoteEventSubIDs = Util.String2Enum<RemoteEventSubIDs>((string)args.eventArgs[0]);
                switch (remoteEventSubIDs)
                {
                    case RemoteEventSubIDs.None:
                        Space_3_SequenceManager.instance.ClosePanel<panel_ChecklistAndInstall>();
                        SpatialBridge.networkingService.SetServerProperties(new Dictionary<string, object> { { RemoteEventIDs.SpaceState.ToString(), remoteEventSubIDs.ToString() } });
                        PolygonGrid_Glow.SetActive(false);
                        break;
                    case RemoteEventSubIDs.Install:
                        Space_3_SequenceManager.instance.OpenPanel<panel_ChecklistAndInstall>();
                        SpatialBridge.networkingService.SetServerProperties(new Dictionary<string, object> { { RemoteEventIDs.SpaceState.ToString(), remoteEventSubIDs.ToString() } });
                        PolygonGrid_Glow.SetActive(true);
                        break;
                    case RemoteEventSubIDs.Uninstall:
                        break;
                    case RemoteEventSubIDs.Checklist:
                        break;
                    case RemoteEventSubIDs.Checkout:
                        break;
                    default:
                        break;
                }
                break;
            case RemoteEventIDs.Install:
                break;
            case RemoteEventIDs.Uninstall:
                break;
            case RemoteEventIDs.Checklist:
                break;
            case RemoteEventIDs.Checkout:
                break;
            default:
                break;
        }
    }
    #endregion



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
            SpatialBridge.cameraService.SetTargetOverride(target, SpatialCameraMode.Actor);
            //SpatialBridge.cameraService.lockCameraRotation = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SpatialBridge.cameraService.ClearTargetOverride();
            //SpatialBridge.cameraService.lockCameraRotation = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SetAllAvatarsVisibilityLocally(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SetAllAvatarsVisibilityLocally(false);
        }
    }
    void InputMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpatialBridge.inputService.StartAvatarInputCapture(false, false, false, false, this);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpatialBridge.inputService.ReleaseInputCapture(this);
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
            Space_3_SequenceManager.instance.OpenPanel<panel_Install>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Space_3_SequenceManager.instance.ClosePanel<panel_Install>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Space_3_SequenceManager.instance.OpenPanel<panel_Checkout>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Space_3_SequenceManager.instance.ClosePanel<panel_Checkout>();
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



    void ToastMessage(string message)
    {
        SpatialBridge.coreGUIService.DisplayToastMessage(message);
    }

    eServiceMode serviceMode = eServiceMode.Camera;


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

    public void OnAvatarMoveInput(InputPhase inputPhase, Vector2 inputMove)
    {
        throw new NotImplementedException();
    }

    public void OnAvatarJumpInput(InputPhase inputPhase)
    {
        throw new NotImplementedException();
    }

    public void OnAvatarSprintInput(InputPhase inputPhase)
    {
        throw new NotImplementedException();
    }

    public void OnAvatarActionInput(InputPhase inputPhase)
    {
        throw new NotImplementedException();
    }

    public void OnAvatarAutoSprintToggled(bool on)
    {
        throw new NotImplementedException();
    }

    public void OnInputCaptureStarted(InputCaptureType type)
    {
        throw new NotImplementedException();
    }

    public void OnInputCaptureStopped(InputCaptureType type)
    {
        throw new NotImplementedException();
    }
}