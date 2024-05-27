using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;

public class testa : MonoBehaviour
{
    GameObject testCube;

    private void Start()
    {
        testCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        testCube.transform.localScale = Vector3.one * 0.2f;
    }
    string worker = "worker_";
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SpatialBridge.coreGUIService.DisplayToastMessage("test");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpatialBridge.actorService.localActor.avatar.SetAvatarBody(AssetType.EmbeddedAsset, worker+"blue");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpatialBridge.actorService.localActor.avatar.SetAvatarBody(AssetType.EmbeddedAsset, worker+"green");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpatialBridge.actorService.localActor.avatar.SetAvatarBody(AssetType.EmbeddedAsset, worker+"red");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SpatialBridge.actorService.localActor.avatar.SetAvatarBody(AssetType.EmbeddedAsset, worker+"white");
        }
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    SpatialBridge.spaceContentService.SpawnPrefabObject(AssetType.EmbeddedAsset, "2", new Vector3(0f, 3f, 0f), Quaternion.identity);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    SpatialBridge.actorService.localActor.avatar.SetAvatarBody(AssetType.EmbeddedAsset, "3");
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    Transform head = SpatialBridge.actorService.localActor.avatar.GetAvatarBoneTransform(HumanBodyBones.Head);
        //    testCube.transform.position = head.position;
        //}
    }


    int healthPoints = 1000;

    public void DealDamage(int damage)
    {
        healthPoints -= damage;
        SpatialBridge.vfxService.CreateFloatingText($"{damage}!", FloatingTextAnimStyle.Bouncy, transform.position, Vector3.up, Color.red);

        if (healthPoints == 0)
            Destroy(gameObject);
    }
}
