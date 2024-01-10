using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class MergeState : FSMState<CircleObject>
{
    public override void OnEnter()
    {
        base.OnEnter();
        sys.SetRigidBodyToKinematic();
        sys.SetRigidBodyVelocity(Vector3.zero);
        sys.SetAngularVelocity(0f);
        sys.StartCoroutine(sys.SpawnNewCircle(sys.ContactCircle.gameObject));
    }
}
