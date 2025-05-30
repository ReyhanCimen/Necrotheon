using UnityEngine;

public class Weapon2DController : MonoBehaviour
{
    [Header("Camera Follow")]
    public Transform cameraTransform;
    public Vector3 positionOffset = new Vector3(0.3f, -0.2f, 0.5f);
    public Vector3 rotationOffset = Vector3.zero;

    [Header("Visuals")]
    public Sprite idleSprite;
    public RuntimeAnimatorController shotAnimatorController;

    [Header("References")]
    public Gun gun; // Gun scriptine referans

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool isShooting = false;
    private float shotAnimLength = 0f;
    private float shotTimer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (spriteRenderer != null && idleSprite != null)
            spriteRenderer.sprite = idleSprite;

        if (animator != null && shotAnimatorController != null)
        {
            animator.runtimeAnimatorController = shotAnimatorController;
            animator.enabled = false;

            AnimationClip[] clips = shotAnimatorController.animationClips;
            if (clips.Length > 0)
                shotAnimLength = clips[0].length;
        }
    }

    void LateUpdate()
    {
        if (cameraTransform != null)
        {
            transform.position = cameraTransform.position + cameraTransform.TransformDirection(positionOffset);
            transform.rotation = cameraTransform.rotation * Quaternion.Euler(rotationOffset);
        }
    }

    // void Update()
    // {
    //     if (Input.GetButtonDown("Fire1"))
    //     {
    //         // Eğer mermi yoksa animasyon oynama
    //         if (gun != null && gun.currentAmmo <= 0)
    //             return;

    //         PlayShotAnimation();
    //     }

    //     if (isShooting)
    //     {
    //         shotTimer += Time.deltaTime;
    //         if (shotTimer >= shotAnimLength)
    //         {
    //             ResetToIdle();
    //         }
    //     }
    // }

    public void  PlayShotAnimation()
    {
        if (animator == null || shotAnimatorController == null)
            return;

        isShooting = true;
        shotTimer = 0f;

        animator.enabled = true;
        animator.Play("Shot", 0, 0f);
    }

    void ResetToIdle()
    {
        isShooting = false;
        shotTimer = 0f;

        if (animator != null)
            animator.enabled = false;

        if (spriteRenderer != null && idleSprite != null)
            spriteRenderer.sprite = idleSprite;
    }
}
