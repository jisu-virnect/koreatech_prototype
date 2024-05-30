using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class go_Section : MonoBehaviour
{
    private GameObject go_PolygonGrid_Glow;
    private Animator animator;
    private SpatialTriggerEvent triggerEvent;
    private void Awake()
    {
        go_PolygonGrid_Glow = gameObject.Search(nameof(go_PolygonGrid_Glow)).gameObject;

        animator = go_PolygonGrid_Glow.GetComponent<Animator>();
        SetAnimation(false);

        InitTriggerEvent();
    }

    public void SetAnimation(bool active)
    {
        if (active)
        {
            animator.Play(Define.polygonGrid_Glow_Appear, -1, 0);
        }
        else
        {
            animator.Play(Define.polygonGrid_Glow_DisAppear, -1, 0);
        }
    }

    private void InitTriggerEvent()
    {
        triggerEvent = gameObject.Search<SpatialTriggerEvent>(nameof(triggerEvent));
        triggerEvent.onEnterEvent.unityEvent.AddListener(() => OnTriggerEnter_Spatial(name));
        triggerEvent.onExitEvent.unityEvent.AddListener(() => OnTriggerExit_Spatial(name));
    }

    private void OnTriggerEnter_Spatial(string name)
    {
        SetAnimation(true);
        var remoteEventSubIDs = Util.String2Enum<RemoteEventSubIDs>(name);
#if UNITY_EDITOR
        switch (remoteEventSubIDs)
        {
            case RemoteEventSubIDs.before:
               Space_3.instance.Trigger_BeforeZone();
                break;
            case RemoteEventSubIDs.install:
                Space_3.instance.Trigger_InstallZone();
                break;
            case RemoteEventSubIDs.after:
                Space_3.instance.Trigger_AfterZone();
                break;
            default:
                break;
        }
#else
        SpatialBridge.networkingService.remoteEvents.RaiseEventAll((byte)RemoteEventIDs.SpaceState, new object[] { remoteEventSubIDs.ToString() });
#endif
    }

    private void OnTriggerExit_Spatial(string name)
    {
        SetAnimation(false);
        var remoteEventSubIDs = Util.String2Enum<RemoteEventSubIDs>(name);
        switch (remoteEventSubIDs)
        {
            case RemoteEventSubIDs.install:
            case RemoteEventSubIDs.before:
            case RemoteEventSubIDs.after:
#if UNITY_EDITOR
                Space_3.instance.Trigger_World();
#else
                SpatialBridge.networkingService.remoteEvents.RaiseEventAll((byte)RemoteEventIDs.SpaceState, new object[] { RemoteEventSubIDs.world.ToString() });
#endif
                break;
            default:
                break;
        }
    }

}
