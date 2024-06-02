using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class panel_PlanMap : panel_Base
{
    private GameObject go_FloorPlan;
    private GameObject go_Elevation;

    private TMP_Text tmp_Title;
    private TMP_Text tmp_FloorPlan;
    private TMP_Text tmp_Elevation;
    private Image img_FloorPlan;
    private Image img_Elevation;

    private Button btn_FloorPlan;
    private Button btn_Elevation;
    private Button btn_Show;
    private Button btn_Hide;
    protected override void Awake()
    {
        base.Awake();
        btn_FloorPlan = gameObject.Search<Button>(nameof(btn_FloorPlan));
        btn_FloorPlan.onClick.AddListener(OnClick_FloorPlan);

        btn_Elevation = gameObject.Search<Button>(nameof(btn_Elevation));
        btn_Elevation.onClick.AddListener(OnClick_Elevation);

        btn_Show = gameObject.Search<Button>(nameof(btn_Show));
        btn_Show.onClick.AddListener(OnClick_Show);

        btn_Hide = gameObject.Search<Button>(nameof(btn_Hide));
        btn_Hide.onClick.AddListener(OnClick_Hide);

        go_FloorPlan = gameObject.SearchGameObject(nameof(go_FloorPlan));
        go_Elevation = gameObject.SearchGameObject(nameof(go_Elevation));

        tmp_FloorPlan = gameObject.Search<TMP_Text>(nameof(tmp_FloorPlan));
        tmp_Elevation = gameObject.Search<TMP_Text>(nameof(tmp_Elevation));
        img_FloorPlan = gameObject.Search<Image>(nameof(img_FloorPlan));
        img_Elevation = gameObject.Search<Image>(nameof(img_Elevation));

        OnClick_Show();
    }
    packet_mapdata[] packet_Mapdatas;
    public override void SetData<T>(T t)
    {
        base.SetData(t);
        packet_Mapdatas = t as packet_mapdata[];

        tmp_FloorPlan.text = packet_Mapdatas[0].mapdata;
        img_FloorPlan.sprite = ResourceManager.instance.LoadDataSprite(packet_Mapdatas[0].sprite);
        tmp_Elevation.text = packet_Mapdatas[1].mapdata;
        img_Elevation.sprite = ResourceManager.instance.LoadDataSprite(packet_Mapdatas[1].sprite);
    }

    private void OnClick_FloorPlan()
    {
        UIManager.instance.OpenPopup<popup_InstallMapDetail>().SetData(packet_Mapdatas[0]);
    }

    private void OnClick_Elevation()
    {
        UIManager.instance.OpenPopup<popup_InstallMapDetail>().SetData(packet_Mapdatas[1]);
    }

    private void OnClick_Show()
    {
        btn_Show.gameObject.SetActive(false);
        btn_Hide.gameObject.SetActive(true);
        go_FloorPlan.SetActive(true);
        go_Elevation.SetActive(true);
    }
    private void OnClick_Hide()
    {
        btn_Show.gameObject.SetActive(true);
        btn_Hide.gameObject.SetActive(false);
        go_FloorPlan.SetActive(false);
        go_Elevation.SetActive(false);
    }
}
