using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GroundedState : FSMState<CircleObject>
{
    // Start is called before the first frame update
    public override void OnEnter()
    {
        base.OnEnter();
        sys.SetIsSFXPlayed(true);
        sys.StartCoroutine(sys.ResetMerge());
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        //sys.ClaimPosition();
    }
}
