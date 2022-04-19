using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieComponent : MonoBehaviour
{
    public float zombieDamage = 5;

    public NavMeshAgent zombieNavMesh;
    public Animator zombieAnimator;
    public ZombieStateMachine stateMachine;

    public GameObject followTarget;
    
    [Header("Physics Properties")]
    public bool knockedBack;
    public bool isGrounded;
    public Transform groundCheckOrigin;
    public float groundCheckRadius;
    public LayerMask groundMasks;
    public float knockbackTimer;
    public float knockbackDuration;

    [Header("Score Value")]
    public int scoreValue;

    public Rigidbody rigidBody;
    public List<AudioClip> audioClips;
    private void Awake()
    {
        zombieNavMesh = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        stateMachine = GetComponent<ZombieStateMachine>();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Initialize(followTarget);
    }

    public void Initialize(GameObject _followTarget)
    {
        followTarget = _followTarget;
        knockedBack = false;
        ZombieIdleState idleState = new ZombieIdleState(this, stateMachine);
        ZombieFollowState followState = new ZombieFollowState(followTarget, this, stateMachine);
        ZombieAttackState attackState = new ZombieAttackState(followTarget, this, stateMachine);
        ZombieDeadState deadState = new ZombieDeadState(this, stateMachine);
        stateMachine.AddState(ZombieStateType.Idling, idleState);
        stateMachine.AddState(ZombieStateType.Following, followState);
        stateMachine.AddState(ZombieStateType.Attacking, attackState);
        stateMachine.AddState(ZombieStateType.isDead, deadState);

        stateMachine.Initialize(ZombieStateType.Following);
    }
    private void Update()
    {
        if (!knockedBack)
        {
            CheckGround();
        }
        else
        {
            knockbackTimer += Time.deltaTime;
            if (knockbackTimer > knockbackDuration)
            {
                knockedBack = false;
            }
        }
    }

    void EnableNavMesh()
    {
        zombieNavMesh.enabled = true;
    }
    
    public void StunEnemy()
    {
        DisableNavMesh();

    }

    void DisableNavMesh()
    {
        Debug.Log("Disable");
        knockbackTimer = 0;
        rigidBody.mass = 3;
        zombieNavMesh.enabled = false;
        knockedBack = true;
    }

    public void Remove()
    {
        DisableNavMesh();
        Destroy(gameObject);
    }

    public void UpdateScore()
    {
        ScoreManager.Instance.InvokeScoreUpdate(scoreValue);
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheckOrigin.position, groundCheckRadius, groundMasks);

        if (isGrounded && GetComponent<ZombieHealthComponent>().CurrentHealth > 0)
        {
            EnableNavMesh();
        }
        else
        {
            DisableNavMesh();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundCheckOrigin.position, groundCheckRadius);
    }
}
