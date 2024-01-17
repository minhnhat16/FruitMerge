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
        sys.RandomYaySFX(sys.TypeID);
    }
}
