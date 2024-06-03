using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LookAt : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Util.LookAtCamera(transform, SpatialBridge.actorService.localActor.avatar.position);
    }
}
