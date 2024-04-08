using UnityEngine;

[System.Serializable]

public class DropState : FSMState<CircleObject>
{
    // Start is called before the first frame update
    public override void OnEnter()
    {
        sys.SetSpritePiority(0);
        sys.SetIsDropping(true);
        sys.DropMergeStartCoroutine();
        sys.SetRigidBodyToDynamic();
        sys.SetColliderRadius();
        sys.SetRigidBodyVelocity(new Vector3(0, -15));
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        sys.SetDropVelocity();
    }
}
