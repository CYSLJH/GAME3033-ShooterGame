using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float walkjumpForce = 5;
    [SerializeField] private float runjumpForce = 2;
    [SerializeField] private float currentJumpForce;
    [SerializeField] private float currentSpeed;

    private PlayerController playerController;
    private Rigidbody rb;
    private Animator animController;

    // references
    Vector2 inputVector = Vector2.zero;
    Vector3 moveDirection = Vector3.zero;
    Vector2 lookInput = Vector2.zero;

    // Aiming Sensitivity and looking variables
    public float aimSensitivity = 1;
    [SerializeField] private GameObject aimingTarget;
    public GameObject followTarget;

    [SerializeField] private float bottomClamp = -30.0f;
    [SerializeField] private float topClamp = 70.0f;
    [SerializeField] private float aimingThreshold = 1.0f/60.0f;
    [SerializeField] private float cameraAngleOverride = 0.0f;
    [SerializeField] private float horizontalCameraAngleOverride = 0.0f;
    [SerializeField] private LayerMask aimColliderMask;
    [SerializeField] private Transform aimLocation;

    float _cinemachineTargetYaw;
    float _cinemachineTargetPitch;

    // OTS Camera
    [SerializeField] private CinemachineVirtualCamera cinemachineAimCamera;

    // Inventory Canvas
    [SerializeField] private GameUIController uiController;
    private bool isInventoryOn;

    // Audio Source
    AudioSource audioSource;
    public List<AudioClip> audioClips;

    // Animator hashes
    public readonly int movementXHash = Animator.StringToHash("MovementX");
    public readonly int movementYHash = Animator.StringToHash("MovementY");
    public readonly int isJumpingHash = Animator.StringToHash("isJumping");
    public readonly int isRunningHash = Animator.StringToHash("isRunning");
    public readonly int isFiringHash = Animator.StringToHash("isFiring");
    public readonly int isReloadingHash = Animator.StringToHash("isReloading");
    //public readonly int AimVerticalHash = Animator.StringToHash("AimVertical");

    // Start is called before the first frame update
    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        animController = GetComponent<Animator>();
        cinemachineAimCamera.gameObject.SetActive(false);
        aimingTarget.transform.rotation = Quaternion.identity;
        isInventoryOn = false;
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        if (!GameManager.Instance.cursorActive)
        {
            AppEvents.InvokeOnMouseCursorEnable(false);
        }
        AppEvents.InvokeOnPauseEvent(false);
        GameManager.Instance.GetComponent<AudioSource>().Play();
    }

    private void OldCameraRotation()
    {
        // horizontal rotation
        followTarget.transform.rotation *= Quaternion.AngleAxis(lookInput.x * aimSensitivity, Vector3.up);
        // vertical rotation
        followTarget.transform.rotation *= Quaternion.AngleAxis(lookInput.y * aimSensitivity, Vector3.left);

        var angles = followTarget.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTarget.transform.localEulerAngles.x;

        float min = -60;
        float max = 70.0f;
        float range = max - min;
        float offsetToZero = 0 - min;
        float aimAngle = followTarget.transform.localEulerAngles.x;
        aimAngle = (aimAngle > 180) ? aimAngle - 360 : aimAngle;
        float val = (aimAngle + offsetToZero) / (range);
        //animController.SetFloat(AimVerticalHash, val);


        if (angle > 180 && angle < 300)
        {
            angles.x = 300;
        }
        if (angle < 180 && angle > 70)
        {
            angles.x = 70;
        }


        // if we aim up, adjust animations to have a mask that will let us properly animate the aim
        followTarget.transform.localEulerAngles = angles;

        // rotate the player rotation based on the look transform
        transform.rotation = Quaternion.Euler(0, followTarget.transform.rotation.eulerAngles.y, 0);
        followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // movement
        if (playerController.isJumping) return;
        if (!(inputVector.magnitude >= 0.0f))
        {
            moveDirection = Vector3.zero;
            
        } else
        {
            audioSource.Stop();
        }

        moveDirection = transform.forward * inputVector.y + transform.right * inputVector.x;
        currentSpeed = playerController.isRunning ? runSpeed : walkSpeed;
        currentJumpForce = playerController.isRunning ? runjumpForce : walkjumpForce;

        Vector3 movementDirection = moveDirection * (currentSpeed * Time.deltaTime);
        transform.position += movementDirection;


        
    }

    private void LateUpdate()
    {
        // Aiming/looking

        //OldCameraRotation();
        CameraRotation();
        AimUpdate();
    }
    public void OnMovement(InputValue value)
    {
        inputVector = value.Get<Vector2>();
        animController.SetFloat(movementXHash, inputVector.x);
        animController.SetFloat(movementYHash, inputVector.y);
        audioSource.clip = audioClips[0];
        audioSource.loop = true;
        audioSource.Play();
    }

    public void OnRun(InputValue value)
    {
        playerController.isRunning = value.isPressed;
        animController.SetBool(isRunningHash, playerController.isRunning);
    }

    public void OnJump(InputValue value)
    {
        if (!playerController.isJumping)
        {
            playerController.isJumping = value.isPressed;
            if (!playerController.isRunning)
            {
                currentSpeed /= 5;
            } else
            {
                currentSpeed /= 3;
            }
            audioSource.clip = audioClips[1];
            audioSource.loop = false;
            audioSource.Play();

            rb.AddForce((transform.up + (moveDirection * currentSpeed)) * currentJumpForce, ForceMode.Impulse);
            animController.SetBool(isJumpingHash, playerController.isJumping);
        }
    }

    public void OnAim(InputValue value)
    {
        playerController.isAiming = value.isPressed;
        AimCamera(value.isPressed);
    }

    private void CameraRotation()
    {
        if (lookInput.sqrMagnitude >= aimingThreshold)
        {
            _cinemachineTargetYaw += lookInput.x * Time.deltaTime * aimSensitivity;
            _cinemachineTargetPitch -= lookInput.y * Time.deltaTime * aimSensitivity;
        }

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

        aimingTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + cameraAngleOverride, _cinemachineTargetYaw, 0.0f);
        transform.rotation = Quaternion.Euler(0, aimingTarget.transform.rotation.eulerAngles.y, 0);


    }
    private void AimUpdate()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, aimColliderMask))
        {
            aimLocation.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }

        Vector3 worldAimTarget = mouseWorldPosition;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime);
        //// rotate the player rotation based on the look transform
        //if (playerMovement.firing)
        //{
        //    if (hitTransform != null)
        //    {
        //        Debug.Log("Hit");
        //    }
        //}
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void AimCamera(bool isAimPressed)
    {
        if (isAimPressed)
        {
            cinemachineAimCamera.gameObject.SetActive(true);
            walkSpeed = 2.5f;
            runSpeed = 5.0f;
            
        } else
        {
            cinemachineAimCamera.gameObject.SetActive(false);
            walkSpeed = 5.0f;
            runSpeed = 10.0f;

        }
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
        // aim up, down, adjust animations to have a mask that will let us properly animate aim
    }

    public void OnPause(InputValue value)
    {
        if (AppEvents.currentPause)
        {
            AppEvents.InvokeOnPauseEvent(false);
            AppEvents.InvokeOnMouseCursorEnable(false);
        }
        else
        {
            AppEvents.InvokeOnPauseEvent(true);
            AppEvents.InvokeOnMouseCursorEnable(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !playerController.isJumping)
        {

            return;
            
        }

        playerController.isJumping = false;
        animController.SetBool(isJumpingHash, false);
        rb.velocity = Vector3.zero;
    }
}
