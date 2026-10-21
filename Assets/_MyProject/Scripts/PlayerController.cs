using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private bool faceMoveDirection;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.6f;
    [SerializeField] private float jumpForce = 5f;

    private Animator animator;
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool isSprinting;
    
    private GameUI gameUI;
    void Awake()
    {
        gameUI = FindAnyObjectByType<GameUI>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
       
        isSprinting = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Attack();
        }
         UpdateAnimator();
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if (animator != null)
            animator.SetTrigger("Jump");
        }
    }
    public void Attack()
    {
        if (animator != null)
            animator.SetTrigger("Attack");
    }

    void FixedUpdate()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        Vector3 movement = forward * moveInput.y + right * moveInput.x;
        float currentMoveSpeed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
        Vector3 velocity = rb.linearVelocity;
        velocity.x = movement.x * currentMoveSpeed;
        velocity.z = movement.z * currentMoveSpeed;
        rb.linearVelocity = velocity;
        if (faceMoveDirection && movement.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));
        }
    }
    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

         float inputMagnitude = new Vector3(moveInput.x, 0f, moveInput.y).magnitude;

         float speedValue = 0f;
        if (inputMagnitude > 0.01f)
        {
            speedValue = isSprinting ? 2.0f : 1.0f;
        }
        animator.SetFloat("Speed", speedValue);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            Coin coin = collision.gameObject.GetComponent<Coin>();

            if (coin != null)
            {
                if (gameUI != null)
                {
                    gameUI.AddScore(coin.GetScoreValue());
                }
                Destroy(coin.gameObject);
            }
        }
    }

}
