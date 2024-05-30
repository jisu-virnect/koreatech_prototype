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

        go_FloorPlan = gameObject.Search(nameof(go_FloorPlan)).gameObject;
        go_Elevation = gameObject.Search(nameof(go_Elevation)).gameObject;

        OnClick_Show();
    }

    private void OnClick_FloorPlan()
    {
        UIManager.instance.OpenPopup<popup_InstallMapDetail>().SetData(new packet_mapdata("평면도", "bg_floorplan"));
    }

    private void OnClick_Elevation()
    {
        UIManager.instance.OpenPopup<popup_InstallMapDetail>().SetData(new packet_mapdata("입면도", "bg_elevation"));
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
