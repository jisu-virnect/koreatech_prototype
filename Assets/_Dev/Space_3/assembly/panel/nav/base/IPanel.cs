using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPanel
{
    void Open(Action act = null);
    void Close(Action act = null);
    void SetData<T>(T t) where T : class;
}
