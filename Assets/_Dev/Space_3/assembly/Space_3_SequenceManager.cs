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

    private List<Space_3_Sequence> sequences = new List<Space_3_Sequence>();
    private Space_3_Sequence prevSequence = null;

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
        InitDB();
        ResetSequence();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PrevSequence();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            NextSequence();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ResetSequence();
        }
    }



    List<Sequence> sequenceList;

    private void InitDB()
    {
        sequenceList = Util.FromJsonList<Sequence>(db.text);
    }
    /// <summary>
    /// 시퀀스데이터 초기화
    /// </summary>
    /// <param name="item"></param>
    private void CreateSequenceUI()
    {
        for (int i = 0; i < sequenceList.Count; i++)
        {
            Sequence sequence = sequenceList[i];
            Space_3_Sequence space_3_Sequence = Instantiate(prefab_go_Sequence, go_SequencePreviewRoot.transform).GetComponent<Space_3_Sequence>();
            sequences.Add(space_3_Sequence);
            space_3_Sequence.SetData(sequence);
        }
    }

    /// <summary>
    /// 시퀀스 삭제
    /// </summary>
    private void DestroySequenceUI()
    {
        for (int i = go_SequencePreviewRoot.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(go_SequencePreviewRoot.transform.GetChild(i).gameObject);
        }
        sequences.Clear();
    }


    public void ResetSequence()
    {
        sequenceIndex = -1;
        prevSequence = null;
        scaffold01_1.Action_ResetObjects();
        DestroySequenceUI();
        CreateSequenceUI();
        NextSequence();
    }

    public void NextSequence()
    {
        if (prevSequence != null)
        {
            prevSequence.SetSequenceState(SEQUENCE_STATE.AFTER);
            scaffold01_1.Action_scaffold_RenderMode((eBuildScaffold)sequenceIndex, BlendMode.Opaque, prevSequence.sequence);
            scaffold01_1.Action_scaffold_Position((eBuildScaffold)sequenceIndex, BlendMode.Opaque, prevSequence.sequence);
        }
        if (sequences.Count > sequenceIndex + 1)
        {
            sequenceIndex++;
            prevSequence = sequences[sequenceIndex];

            prevSequence.SetSequenceState(SEQUENCE_STATE.FOCUE);
            scaffold01_1.Action_scaffold_RenderMode((eBuildScaffold)sequenceIndex, BlendMode.Transparent, prevSequence.sequence);
            scaffold01_1.Action_scaffold_Position((eBuildScaffold)sequenceIndex, BlendMode.Transparent, prevSequence.sequence);
            scaffold01_1.Action_scaffold_Active((eBuildScaffold)sequenceIndex, true);

        }
    }

    public void PrevSequence()
    {
        if (sequenceIndex > 0)
        {
            prevSequence.SetSequenceState(SEQUENCE_STATE.BEFORE);
            scaffold01_1.Action_scaffold_RenderMode((eBuildScaffold)sequenceIndex, BlendMode.Opaque, prevSequence.sequence);
            scaffold01_1.Action_scaffold_Position((eBuildScaffold)sequenceIndex, BlendMode.Opaque, prevSequence.sequence);
            scaffold01_1.Action_scaffold_Active((eBuildScaffold)sequenceIndex, false);
            sequenceIndex--;

            prevSequence = sequences[sequenceIndex];
            prevSequence.SetSequenceState(SEQUENCE_STATE.FOCUE);
            scaffold01_1.Action_scaffold_RenderMode((eBuildScaffold)sequenceIndex, BlendMode.Transparent, prevSequence.sequence);
            scaffold01_1.Action_scaffold_Position((eBuildScaffold)sequenceIndex, BlendMode.Transparent, prevSequence.sequence);
            scaffold01_1.Action_scaffold_Active((eBuildScaffold)sequenceIndex, true);
        }
    }
}
