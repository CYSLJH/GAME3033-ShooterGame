using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFollowState : ZombieStates
{
    GameObject followTarget;
    const float stoppingDistance = 1;
    int movementZHash = Animator.StringToHash("MovementZ");

    public ZombieFollowState(GameObject _followTarget, ZombieComponent zombie, ZombieStateMachine zombieStateMachine) : base(zombie, zombieStateMachine)
    {
        followTarget = _followTarget;
        updateInterval = 2;
    }

    public override void Start()
    {
        base.Start();
        if (ownerZombie.zombieNavMesh.enabled)
            ownerZombie.zombieNavMesh.SetDestination(followTarget.transform.position);
    }

    public override void IntervalUpdate()
    {
        base.IntervalUpdate();
        if (ownerZombie.zombieNavMesh.enabled)
        {
            ownerZombie.zombieNavMesh.SetDestination(followTarget.transform.position);
        }
    }

    public override void Update()
    {
        base.Update();
        float moveZ = ownerZombie.zombieNavMesh.velocity.normalized.z != 0f ? 1f : 0f;
        ownerZombie.zombieAnimator.SetFloat(movementZHash, moveZ);

        if (followTarget.gameObject != null)
        {
            float distanceBetween = Vector3.Distance(ownerZombie.transform.position, followTarget.transform.position);
            if (distanceBetween < stoppingDistance)
            {
                stateMachine.ChangeState(ZombieStateType.Attacking);
            }
            if (followTarget == null)
            {
                stateMachine.ChangeState(ZombieStateType.Idling);
            }
        }
        
    }
}
