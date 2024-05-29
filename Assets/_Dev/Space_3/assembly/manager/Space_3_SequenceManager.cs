using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class Space_3_SequenceManager : MonoBehaviour
{
    public static Space_3_SequenceManager instance;
    public TextAsset db;

    public panel_Base[] panel_Bases { get; private set; }
    public popup_Base[] popup_Bases { get; private set; }
    public List<Sequence> sequenceList { get; private set; }

    private void Awake()
    {
        instance = this;
        InitUI();
        InitDB();
    }

    private void InitUI()
    {
        panel_Bases = (panel_Base[])FindObjectsOfType(typeof(panel_Base), true);
        popup_Bases = (popup_Base[])FindObjectsOfType(typeof(popup_Base), true);
    }

    private void InitDB()
    {
        sequenceList = Util.FromJsonList<Sequence>(db.text);
    }

    public void OpenPanel<T>() where T : panel_Base
    {
        for (int i = 0; i < panel_Bases.Length; i++)
        {
            panel_Base panel_Base = panel_Bases[i];
            if (panel_Base as T)
            {
                panel_Base.Open();
                break;
            }
        }
    }
    public void ClosePanel<T>() where T : panel_Base
    {
        for (int i = 0; i < panel_Bases.Length; i++)
        {
            panel_Base panel_Base = panel_Bases[i];
            if (panel_Base as T)
            {
                panel_Base.Close();
                break;
            }
        }
    }

    public popup_Base OpenPopup<T>() where T : popup_Base
    {
        popup_Base popup_Base = default;
        for (int i = 0; i < popup_Bases.Length; i++)
        {
            popup_Base = popup_Bases [i];
            if (popup_Base as T)
            {
                break;                
            }
        }
        popup_Base.Open();
        return popup_Base;
    }
    public popup_Base ClosePopup<T>() where T : popup_Base
    {
        popup_Base popup_Base = default;
        for (int i = 0; i < popup_Bases.Length; i++)
        {
            popup_Base = popup_Bases[i];
            if (popup_Base as T)
            {
                break;
            }
        }
        popup_Base.Close();
        return popup_Base;
    }
}
