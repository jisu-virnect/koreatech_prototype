using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class popup_InstallMapDetail : popup_Base
{
    private TMP_Text tmp_Content;
    private Image img_Map;
    protected override void Awake()
    {
        base.Awake();
        tmp_Content = gameObject.Search<TMP_Text>(nameof(tmp_Content));
        img_Map = gameObject.Search<Image>(nameof(img_Map));
    }
    public override void SetData<T>(T t)
    {
        base.SetData(t);
        packet_mapdata packet_Mapdata = t as packet_mapdata;
        tmp_Content.text = packet_Mapdata.mapdata;
        Texture2D texture = ResourceManager.instance.LoadData<Texture2D>(packet_Mapdata.sprite);
        Sprite sprite = Util.Tex2Sprite(texture);
        img_Map.sprite = sprite;


    }
}

public class packet_mapdata
{
    public string mapdata;
    public string sprite;

    public packet_mapdata(string mapdata, string sprite)
    {
        this.mapdata = mapdata;
        this.sprite = sprite;
    }
}
