using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    [Header("Visuals")]
    public Sprite idleSprite;
    public RuntimeAnimatorController hitAnimatorController;
    public Sprite deadSprite; // Yeni: Ölüm sprite'ı

    [Header("Scale Animation")]
    public Vector3 hitScale = new Vector3(1.2f, 1.2f, 1f);
    public Vector3 idleScale = Vector3.one;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Enemy enemy;

    private bool isHit = false;
    private float hitAnimLength = 0f;
    private float hitTimer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemy = GetComponentInParent<Enemy>();

        if (spriteRenderer != null && idleSprite != null)
            spriteRenderer.sprite = idleSprite;

        if (animator != null && hitAnimatorController != null)
        {
            animator.runtimeAnimatorController = hitAnimatorController;
            animator.enabled = false;

            AnimationClip[] clips = hitAnimatorController.animationClips;
            if (clips.Length > 0)
                hitAnimLength = clips[0].length;
        }
    }

    void Update()
    {
        if (enemy != null && enemy.isEnemyHit && !isHit)
        {
            PlayHitAnimation();
        }

        if (isHit)
        {
            hitTimer += Time.deltaTime;
            if (hitTimer >= hitAnimLength)
            {
                ResetToIdle();
            }
        }

        // Can 0 veya altına düştüyse ölüm animasyonunu otomatik oynat
        if (enemy != null && enemy.currentHealth <= 0 && spriteRenderer.sprite != deadSprite)
        {
            PlayDeadAnimation();
        }
    }

    public void PlayHitAnimation()
    {
        if (animator == null || hitAnimatorController == null)
            return;

        isHit = true;
        hitTimer = 0f;

        if (enemy != null)
            enemy.isEnemyHit = false;

        animator.enabled = true;
        animator.Play("Hit", 0, 0f);

        transform.localScale = hitScale;
    }

    public void PlayDeadAnimation()
    {
        // Ölüm animasyonu: sprite'ı deadSprite yap, scale sıfırla, animator'ı kapat
        if (animator != null)
            animator.enabled = false;

        if (spriteRenderer != null && deadSprite != null)
            spriteRenderer.sprite = deadSprite;

        transform.localScale = idleScale;
        isHit = false;
        hitTimer = 0f;
    }

    void ResetToIdle()
    {
        // Eğer düşman öldüyse idle'a dönme
        if (enemy != null && enemy.currentHealth <= 0)
            return;

        isHit = false;
        hitTimer = 0f;

        if (animator != null)
            animator.enabled = false;

        if (spriteRenderer != null && idleSprite != null)
            spriteRenderer.sprite = idleSprite;

        transform.localScale = idleScale;
    }
}
