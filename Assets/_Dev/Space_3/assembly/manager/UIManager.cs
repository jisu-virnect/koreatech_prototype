using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Dictionary<string, List<panel_Base>> stackedPanel = new Dictionary<string, List<panel_Base>>();
    public panel_Base[] panel_Bases { get; private set; }
    public popup_Base[] popup_Bases { get; private set; }
    public toast_Base[] toast_Bases { get; private set; }

    private void Awake()
    {
        instance = this;
        InitUI();
    }

    private void InitUI()
    {
        panel_Bases = FindObjectsOfType<panel_Base>(true);
        popup_Bases = FindObjectsOfType<popup_Base>(true);
        toast_Bases = FindObjectsOfType<toast_Base>(true);
    }

    public panel_Base OpenPanel<T>(string stackName = "") where T : panel_Base
    {
        panel_Base panel_Base = default;
        for (int i = 0; i < panel_Bases.Length; i++)
        {
            panel_Base = panel_Bases[i];
            if (panel_Base as T)
            {
                panel_Base.Open();
                if (stackName != "")
                {
                    Util.DicList(ref stackedPanel, stackName, panel_Base);
                }
                break;
            }
        }
        return panel_Base;
    }
    public panel_Base ClosePanel<T>() where T : panel_Base
    {
        panel_Base panel_Base = default;
        for (int i = 0; i < panel_Bases.Length; i++)
        {
            panel_Base = panel_Bases[i];
            if (panel_Base as T)
            {
                panel_Base.Close();
                break;
            }
        }
        return panel_Base;
    }
    public void ClosePanels(string stackName)
    {
        for (int i = 0; i < stackedPanel.Count; i++)
        {
            if (stackedPanel.ContainsKey(stackName))
            {
                List<panel_Base> temp_panel_Bases = stackedPanel[stackName];
                for (int j = 0; j < temp_panel_Bases.Count; j++)
                {
                    panel_Base temp_panel_Base = temp_panel_Bases[j];
                    temp_panel_Base.Close();
                }
                stackedPanel.Remove(stackName);
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

    public toast_Base ShowToast<T>(string message, float duration = 0f) where T : toast_Base
    {
        toast_Base toast_Base = default;
        for (int i = 0; i < popup_Bases.Length; i++)
        {
            toast_Base = toast_Bases[i];
            if (toast_Base as T)
            {
                break;
            }
        }
        toast_Base.Show(message, duration);
        return toast_Base;
    }
}
