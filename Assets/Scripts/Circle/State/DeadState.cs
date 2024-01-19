using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeadState : FSMState<CircleObject>
{
    public override void OnEnter()
    {
        base.OnEnter();
        sys.SetRigidBodyToFreeze();
    }
}
