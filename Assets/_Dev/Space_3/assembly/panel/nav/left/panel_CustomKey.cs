using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panel_CustomKey : panel_Base
{
    private Button btn_1;
    private Button btn_2;
    private Button btn_3;
    private Button btn_4;

    protected override void Awake()
    {
        base.Awake();
        btn_1 = gameObject.Search<Button>(nameof(btn_1));
        btn_1.onClick.AddListener(() => SpatialBridge.actorService.localActor.avatar.SetAvatarBody(AssetType.EmbeddedAsset, "0"));
        btn_2 = gameObject.Search<Button>(nameof(btn_2));
        btn_2.onClick.AddListener(() => SpatialBridge.actorService.localActor.avatar.SetAvatarBody(AssetType.EmbeddedAsset, "1"));
        btn_3 = gameObject.Search<Button>(nameof(btn_3));
        btn_3.onClick.AddListener(() => SpatialBridge.actorService.localActor.avatar.SetAvatarBody(AssetType.EmbeddedAsset, "2"));
        btn_4 = gameObject.Search<Button>(nameof(btn_4));
        btn_4.onClick.AddListener(() => SpatialBridge.actorService.localActor.avatar.SetAvatarBody(AssetType.EmbeddedAsset, "3"));
    }
}
