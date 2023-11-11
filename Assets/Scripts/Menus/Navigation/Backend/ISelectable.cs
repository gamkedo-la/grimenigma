using System;
using UnityEngine;

public interface ISelectable
{
    event Action<object> onChange;

    void OnSelect();
    void OnDeselect();
    void Action(object data);
    void InvokeOnChange(object data);
}