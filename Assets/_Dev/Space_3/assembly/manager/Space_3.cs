using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;
using TMPro;
using System;

public class Space_3 : MonoBehaviour
{
    public TMP_Text tmp_Mode;
    public GameObject HMD;
    public TMP_Text tmp_LocalAvatarName;
    public List<SpatialTriggerEvent> triggerEvents = new List<SpatialTriggerEvent>();

    private void Start()
    {
    }

    private void a(ServerConnectionStatus status)
    {
        Debug.Log(status);
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

    /// <summary>
    /// 트리거 됨에 따른 상태 변경
    /// </summary>
    private void SetTriggerEvent()
    {
        if(!SpatialBridge.networkingService.isMasterClient)
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
        Debug.Log(remoteEventSubIDs);
        ToastMessage("a");
        switch (remoteEventSubIDs)
        {
            case RemoteEventSubIDs.Install:
                ToastMessage("b");
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

    private void Update()
    {
        //LocalAvatarName();
        //LookAtCamera(HMD.transform);

        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    SpatialBridge.coreGUIService.SetCoreGUIEnabled(SpatialCoreGUITypeFlags.All, true);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    SpatialBridge.coreGUIService.SetCoreGUIEnabled(SpatialCoreGUITypeFlags.All, false);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    SpatialBridge.coreGUIService.SetCoreGUIOpen(SpatialCoreGUITypeFlags.All, true);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    SpatialBridge.coreGUIService.SetCoreGUIOpen(SpatialCoreGUITypeFlags.All, false);
        //}
        if((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                serviceMode = eServiceMode.None;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                serviceMode = eServiceMode.Network;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                serviceMode = eServiceMode.Camera;
            }
            tmp_Mode.text = serviceMode.ToString();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {

            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                NetworkMode();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CameraMode();
            }
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

    void ToastMessage(string message)
    {
        SpatialBridge.coreGUIService.DisplayToastMessage(message);
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
            SpatialBridge.cameraService.rotationMode = SpatialCameraRotationMode.AutoRotate;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ToastMessage(SpatialBridge.cameraService.zoomDistance.ToString());
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SpatialBridge.cameraService.SetTargetOverride(transform, SpatialCameraMode.Vehicle);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SpatialBridge.cameraService.SetTargetOverride(SpatialBridge.actorService.localActor.avatar.GetAvatarBoneTransform(HumanBodyBones.Head), SpatialCameraMode.Actor);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ToastMessage(SpatialBridge.cameraService.targetOverride.ToString());
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {

        }
    }
    eServiceMode serviceMode;
    enum eServiceMode
    {
        None = 0,
        Network,
        Camera,
    }

    private void OnEnable()
    {
        SpatialBridge.networkingService.remoteEvents.onEvent += HandleEventReceived;
        SpatialBridge.networkingService.onConnectionStatusChanged += a;
    }

    private void OnDisable()
    {
        SpatialBridge.networkingService.remoteEvents.onEvent -= HandleEventReceived;
        SpatialBridge.networkingService.onConnectionStatusChanged -= a;
    }

    private void HandleEventReceived(NetworkingRemoteEventArgs args)
    {
        RemoteEventIDs remoteEventIDs = (RemoteEventIDs)args.eventID;
        ToastMessage("c"+ remoteEventIDs.ToString());
        switch (remoteEventIDs)
        {
            case RemoteEventIDs.SendMagicNumber:
                break;
            case RemoteEventIDs.PrivateMessage:
                break;
            case RemoteEventIDs.SpaceState:
                RemoteEventSubIDs remoteEventSubIDs = Util.String2Enum<RemoteEventSubIDs>((string)args.eventArgs[0]);
                ToastMessage("d" + remoteEventSubIDs.ToString());
                switch (remoteEventSubIDs)
                {
                    case RemoteEventSubIDs.None:
                        Space_3_SequenceManager.instance.ClosePanel<panel_Install>();
                        SpatialBridge.networkingService.SetServerProperties(new Dictionary<string, object> { { RemoteEventIDs.SpaceState.ToString(), remoteEventSubIDs.ToString() } });
                        break;
                    case RemoteEventSubIDs.Install:
                        Space_3_SequenceManager.instance.OpenPanel<panel_Install>();
                        SpatialBridge.networkingService.SetServerProperties(new Dictionary<string, object> { { RemoteEventIDs.SpaceState.ToString(), remoteEventSubIDs.ToString() } });
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


}