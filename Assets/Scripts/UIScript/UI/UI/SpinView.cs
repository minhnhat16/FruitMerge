using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinView : BaseView
{
    SpinCircle spiner;
    public override void OnStartShowView()
    {
        base.OnStartShowView();
        spiner = GetComponent<SpinCircle>();
    }
    public void SpinButton()
    {
        spiner.SpinningCircle();
    }
}

