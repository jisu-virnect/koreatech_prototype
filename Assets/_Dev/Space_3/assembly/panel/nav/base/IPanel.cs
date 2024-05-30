using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPanel
{
    void Open();
    void Close();
    void SetData<T>(T t) where T : class;
}
