using UnityEngine;

public class FPSMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;

    [Header("Mouse Settings")]
    public Transform playerCamera;
    public float mouseSensitivity = 100f;
    public bool invertY = false;

    [Header("UI Panels")]
    public GameObject winScreen;
    public GameObject loseScreen;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        LockCursor(true); // Oyun başında cursor kilitli ve gizli
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();

        // Win/Lose ekranı açıksa cursor göster
        if ((winScreen != null && winScreen.activeSelf) || (loseScreen != null && loseScreen.activeSelf))
        {
            LockCursor(false);
            return;
        }

        // Panel açma/kapatma: Escape ile cursor göster/gizle
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool shouldLock = Cursor.lockState != CursorLockMode.Locked;
            LockCursor(shouldLock);
        }
    }

    void LockCursor(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f; // Yere yapışmasını sağlamak için

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // Koşma kontrolü
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Zıplama
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Yer çekimi
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Invert ayarı
        mouseY *= invertY ? 1 : -1;

        xRotation += mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Kamera dönme sınırı

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    // Bu fonksiyonu PauseMenu veya başka bir yerden çağırabilirsiniz
    public void Resume()
    {
        LockCursor(true); // Resume olduğunda cursor gizlensin ve kilitlensin
    }
    public void Pause()
    {
        LockCursor(false); // Pause olduğunda cursor gösterilsin
    }
}