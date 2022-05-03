
using UnityEngine;

public class playerControl : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 1f; // for camera control
    [SerializeField] private Transform debugPoint;// can delete or use it as an extra flavor
    [SerializeField] private Transform HookshotTransform; // refer to the hook(just a 3d cute)



    // character move control 
    private CharacterController characterController;// for unity build up component characterController
    private float cameraVerticalAngle;
    private float characterVelocityY;
    public Transform groundcheck;
    public float groundDistance = 0.2f;
    public LayerMask ground;
    public float gravity = -9.81f;
    public float jumpHight = 3f;
    Vector3 apply_gravity; // add gravity to the charactercontroller
    private Vector3 hookShotPosition;
    bool isGrounded;
    private Vector3 characterMomentum;
    private float hookshotSize;


    //camera 
    private const float Nomral_fov = 60f; //the camera view for normal
    private const float Hookshot_fov = 100f;

    public Camera playerCamera;
    private cameraPOV cameraFov;

    // three different state, normal apply the gravity, 
    //HookshotThrown: when hook is flying
    //hookshotflying: when player is flying
    private State state;
    private enum State
    {
        Normal,
        hookShotThrown,
        hookShotFlying,
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = transform.Find("Camera").GetComponent<Camera>();
        cameraFov = playerCamera.GetComponent<cameraPOV>();
        Cursor.lockState = CursorLockMode.Locked;
        state = State.Normal;
        HookshotTransform.gameObject.SetActive(false);

    }

    void Update()
    {
        switch (state)
        {
            default:
            case State.Normal:
                HandleCharacterLook();
                HandleCharacterMovment();
                Hookshot();
                cameraFov.setCameraFov(Nomral_fov);
                break;

            case State.hookShotThrown:// when the hookshot is still flying
                hookShotThrow();
                HandleCharacterLook();
                HandleCharacterMovment();
                break;

            case State.hookShotFlying:// when hookshot on the object and player flying
                hookShotFlying();
                HandleCharacterLook();
                break;
        }
    }

    private void hookShotThrow()
    {
        HookshotTransform.LookAt(hookShotPosition);

        float hookshotThrowSpeed = 100f;
        hookshotSize += hookshotThrowSpeed * Time.deltaTime;
        HookshotTransform.localScale = new Vector3(1, 1, hookshotSize);
        if (hookshotSize >= Vector3.Distance(transform.position, hookShotPosition))
        {

            state = State.hookShotFlying;
            cameraFov.setCameraFov(Hookshot_fov);
        }
    }

    private void HandleCharacterLook()
    {
        float lookX = Input.GetAxisRaw("Mouse X");
        float lookY = Input.GetAxisRaw("Mouse Y");

        transform.Rotate(new Vector3(0f, lookX * mouseSensitivity, 0f), Space.Self);
        cameraVerticalAngle -= lookY * mouseSensitivity;
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -89f, 89f);
        playerCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle, 0, 0);
    }

    private void HandleCharacterMovment()

    {
        // using the checksphere a invisible sphere at the buttom of the character,feet, 
        //to check it is touch the ground
        isGrounded = Physics.CheckSphere(groundcheck.position, groundDistance, ground);
        if (isGrounded && apply_gravity.y < 0)
        {
            apply_gravity.y = -2f;
        }
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        float moveSpeed = 5f;

        Vector3 characterVelocity = transform.right * moveX * moveSpeed + transform.forward * moveZ * moveSpeed;

        //apply momentum
        characterVelocity += characterMomentum;

        // move character
        characterController.Move(characterVelocity * Time.deltaTime);

        //dump momentum
        if (characterMomentum.magnitude >= 0f)
        {
            float drag = 3f;
            characterMomentum -= characterMomentum * drag * Time.deltaTime;
            if (characterMomentum.magnitude < .0f)
            {
                characterMomentum = Vector3.zero;
            }

        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //the jump function is sqrt(h*g*-2)
            apply_gravity.y = Mathf.Sqrt(jumpHight * gravity * -2f);
        }

        apply_gravity.y += gravity * Time.deltaTime;
        // apply gravity 
        characterController.Move(apply_gravity * Time.deltaTime);
    }

    private void Hookshot()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit raycastHit))
            {
                debugPoint.position = raycastHit.point;
                hookShotPosition = raycastHit.point;

                hookshotSize = 0f;
                HookshotTransform.gameObject.SetActive(true);
                HookshotTransform.localScale = Vector3.zero;
                state = State.hookShotThrown;
            }
        }

    }
    private void hookShotFlying()
    {
        HookshotTransform.LookAt(hookShotPosition);
        Vector3 hookShotDir = (hookShotPosition - transform.position).normalized;

        //set up the speed for the flying speed
        float maxSpeed = 50f;
        float minSpeed = 15f;
        float hookShotSpeed = Vector3.Distance(transform.position, hookShotPosition);
        float speedMultiplyer = 2.5f;
        float finalSpeed = Mathf.Clamp(hookShotSpeed * speedMultiplyer, minSpeed, maxSpeed);

        characterController.Move(hookShotDir * finalSpeed * Time.deltaTime);

        float reachHookShotPosition = 2f;
        if (Vector3.Distance(transform.position, hookShotPosition) < reachHookShotPosition)
        {
            //reach hookshot positon
            return_normal();

        }
        if (cancelHookShot())
        {
            characterMomentum = hookShotDir * finalSpeed;
            return_normal();
        }
        if (Input.GetButtonDown("Jump"))
        {
            HookshotTransform.gameObject.SetActive(false);
            float momentumExtra = 3f;
            characterMomentum = hookShotDir * hookShotSpeed * momentumExtra;
            float jumpSpeed = 30f;
            characterMomentum += Vector3.up * jumpSpeed;

            return_normal();

        }
    }
    private bool cancelHookShot()
    {
        return (Input.GetKeyDown(KeyCode.E));
    }
    private void return_normal()
    {
        state = State.Normal;
        HookshotTransform.gameObject.SetActive(false);
        apply_gravity.y = -2f;
    }

}
