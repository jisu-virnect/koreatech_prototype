using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class go_Section : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        InitTriggerEvent();

        GameObject go_PolygonGrid_Glow = gameObject.SearchGameObject(nameof(go_PolygonGrid_Glow)).gameObject;
        animator = go_PolygonGrid_Glow.GetComponent<Animator>();
        SetAnimation(false);
    }

    private void InitTriggerEvent()
    {
        SpatialTriggerEvent triggerEvent = gameObject.Search<SpatialTriggerEvent>(nameof(triggerEvent));
        triggerEvent.onEnterEvent.unityEvent.AddListener(() => OnTriggerEnter_Spatial(name));
        triggerEvent.onExitEvent.unityEvent.AddListener(() => OnTriggerExit_Spatial(name));
    }

    private void OnTriggerEnter_Spatial(string name)
    {
        SetAnimation(true);
        var remoteEventSubIDs = Util.String2Enum<RemoteEventSubIDs_Space>(name);
#if UNITY_EDITOR
        switch (remoteEventSubIDs)
        {
            case RemoteEventSubIDs_Space.before:
               Space_3.instance.Trigger_BeforeZone();
                break;
            case RemoteEventSubIDs_Space.install:
                Space_3.instance.Trigger_InstallZone();
                break;
            case RemoteEventSubIDs_Space.after:
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
        var remoteEventSubIDs = Util.String2Enum<RemoteEventSubIDs_Space>(name);
        switch (remoteEventSubIDs)
        {
            case RemoteEventSubIDs_Space.install:
            case RemoteEventSubIDs_Space.before:
            case RemoteEventSubIDs_Space.after:
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

    /// <summary>
    /// 폴리곤그리드 애니메이션
    /// </summary>
    /// <param name="active"></param>
    private void SetAnimation(bool active)
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

}
