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

    public Space_3_SequenceDetail sequenceDetail;

    public TextAsset db;
    public GameObject prefab_go_Title;
    public GameObject prefab_go_Sequence;

    public GameObject go_SequencePreviewRoot;

    private int sequenceIndex;

    private List<GameObject> go_Sequence = new List<GameObject>();
    private List<Sequence> sequenceList = new List<Sequence>();
    private GameObject go_prevSequence = null;

    public scaffold01_1 scaffold01_1;
    private void Awake()
    {
        instance = this;
        sequenceDetail.GetComponent();
        sequenceDetail.gameObject.SetActive(false);
    }

    private void Start()
    {
        go_SequencePreviewRoot = gameObject.Search(nameof(go_SequencePreviewRoot)).gameObject;
        //JSONArray item = GetDB();
        //InitSequenceData(item);
        InitSequenceData();
        ResetSequence();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            scaffold01_1.Action_scaffold01_1((eBuildScaffold)sequenceIndex);
            NextSequence();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            scaffold01_1.Action_ResetObjs();
            ResetSequence();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
        }
    }

    /// <summary>
    /// DB 가져오기
    /// </summary>
    /// <returns></returns>
    //private JSONArray GetDB()
    //{
    //    JSONNode root = JSON.Parse(db.text);
    //    JSONArray item = (JSONArray)root;
    //    return item;
    //}

    /// <summary>
    /// 시퀀스데이터 초기화
    /// </summary>
    /// <param name="item"></param>
    private void InitSequenceData(/*JSONArray item*/)
    {
        sequenceList = new List<Sequence>(Util.FromJsonArray<Sequence>(db.text));
        
        //for (int i = 0; i < item.Count; i++)
        //{
        //    JSONNode itemdata = item[i];
        //    int index = itemdata["index"].AsInt;
        //    string title = itemdata["title"].Value;
        //    string summary = itemdata["summary"].Value;
        //    Sequence temp_Sequence = new Sequence() { index = index, title = title, summary = summary };
        //    sequenceList.Add(temp_Sequence);
        //}
    }

    //private void InitSequenceData(JSONArray item)
    //{
    //    for (int i = 0; i < item.Count; i++)
    //    {
    //        JSONNode itemdata = item[i];
    //        int index = itemdata["index"].AsInt;
    //        string title = itemdata["title"].Value;
    //        string summary = itemdata["summary"].Value;
    //        Sequence temp_Sequence = new Sequence() { index = index, title = title, summary = summary };
    //        sequenceList.Add(temp_Sequence);
    //    }
    //}

    /// <summary>
    /// 시퀀스 삭제
    /// </summary>
    private void DestroySequence()
    {
        for (int i = go_SequencePreviewRoot.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(go_SequencePreviewRoot.transform.GetChild(i).gameObject);
        }
        go_Sequence.Clear();
    }

    public void SetSequence()
    {
        for (int i = 0; i < sequenceList.Count; i++)
        {
            Space_3_Sequence space_3_Sequence = Instantiate(prefab_go_Sequence, go_SequencePreviewRoot.transform).GetComponent<Space_3_Sequence>();
            go_Sequence.Add(space_3_Sequence.gameObject);
            space_3_Sequence.SetData(sequenceList[i]);
        }
    }

    public void ResetSequence()
    {
        sequenceIndex = -1;
        DestroySequence();
        SetSequence();
        NextSequence();
    }

    public void NextSequence()
    {
        if (go_prevSequence != null)
        {
            go_prevSequence.GetComponent<Space_3_Sequence>().SetSequenceState(SEQUENCE_STATE.AFTER);
        }
        if (go_Sequence.Count > sequenceIndex + 1)
        {
            sequenceIndex++;
            GameObject go_currentSequence = go_Sequence[sequenceIndex];
            go_currentSequence.GetComponent<Space_3_Sequence>().SetSequenceState(SEQUENCE_STATE.FOCUE);
            go_prevSequence = go_currentSequence;
        }
    }
}
