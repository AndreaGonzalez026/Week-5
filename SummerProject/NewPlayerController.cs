using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float stamina = 5f;
    [SerializeField] private float maxStamina = 5f;
    [SerializeField] private float staminaRecoveryRate = 1f;
    [SerializeField] private float mouseSensitivity = 2f;
    
    private CharacterController controller;
    private Vector3 moveDirection;
    private bool isJumping;
    private bool isCrouching;
    private bool isSprinting;
    private float originalHeight;
    private float crouchHeight = 0.5f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;
    }

    void Update()
    {
        MovePlayer();
        HandleCrouching();
        HandleSprinting();
        RotatePlayer();
        ApplyGravity();
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void MovePlayer()
    {
        float moveDirectionX = Input.GetAxis("Horizontal");
        float moveDirectionZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveDirectionX + transform.forward * moveDirectionZ;

        float speed = moveSpeed;
        if (isCrouching) speed = crouchSpeed;
        if (isSprinting) speed = sprintSpeed;

        moveDirection = new Vector3(move.x * speed, moveDirection.y, move.z * speed);

        if (controller.isGrounded && Input.GetButtonDown("Jump") && !isCrouching)
        {
            moveDirection.y = jumpForce;
        }
    }

    private void HandleCrouching()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            controller.height = isCrouching ? crouchHeight : originalHeight;
        }
    }

    private void HandleSprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            isSprinting = true;
            stamina -= Time.deltaTime;
        }
        else
        {
            isSprinting = false;
            if (stamina < maxStamina)
            {
                stamina += staminaRecoveryRate * Time.deltaTime;
            }
        }
    }

    private void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);
    }

    private void ApplyGravity()
    {
        if (!controller.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else if (moveDirection.y < 0)
        {
            moveDirection.y = -2f; // small value to keep grounded
        }
    }
}

