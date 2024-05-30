using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class view_Step : MonoBehaviour
{
    private Image img_Idle_1;
    private Image img_Active_1;
    private Image img_Idle_2;
    private Image img_Active_2;
    private void Awake()
    {
        img_Idle_1 = gameObject.Search<Image>(nameof(img_Idle_1));
        img_Idle_2 = gameObject.Search<Image>(nameof(img_Idle_2));
        img_Active_1 = gameObject.Search<Image>(nameof(img_Active_1));
        img_Active_2 = gameObject.Search<Image>(nameof(img_Active_2));
    }


    public void SetActive(eActive active)
    {
        img_Idle_1.gameObject.SetActive(false);
        img_Idle_2.gameObject.SetActive(false);
        img_Active_1.gameObject.SetActive(false);
        img_Active_2.gameObject.SetActive(false);
        switch (active)
        {
            case eActive.idle:
                img_Idle_1.gameObject.SetActive(true);
                img_Idle_2.gameObject.SetActive(true);
                break;
            case eActive.active:
                img_Active_1.gameObject.SetActive(true);
                img_Active_2.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}
