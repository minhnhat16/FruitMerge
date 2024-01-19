using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]

public class SpawnState : FSMState<CircleObject>
{
    public override void OnEnter()
    {
        base.OnEnter();
        sys.SetColliderRadius(0f);
        sys.SetRotation(Vector3.zero);
        sys.SetIsMerge(false);
        sys.SpawnCircle(sys.TypeID);
        sys.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        //sys.SetRigidBodyToNone();
    }

}
