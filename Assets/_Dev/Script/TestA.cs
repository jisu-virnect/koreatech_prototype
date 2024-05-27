using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestA : MonoBehaviour
{
    public TextAsset text;
    // Start is called before the first frame update
    void Start()
    {
        var v= Util.FromJsonArray<Sequence>(text.text);
        Debug.Log(v);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
