using UnityEngine;

public class SwordController_Mono : MonoBehaviour
{
    [Header("Attack Settings")]
    public Animator animator;
    public Transform swordTransform;
    public LayerMask enemyLayer;
    public float attackRange = 1.5f;
    public int baseDamage = 10;
    public float attackCooldown = 1f;
    private float nextAttackTime = 0f;

    public int heavyAttackDamage = 25;
    public float heavyAttackCooldown = 3f;
    private float nextHeavyAttackTime = 0f;

    [Header("Animation Settings")]
    public string[] attackAnimations;
    public string heavyAttackTrigger = "HeavyAttack";

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip swordSwingSound;
    public AudioClip heavyAttackSound;
    public AudioClip criticalHitSound;

    private bool isAttacking = false;
    private float attackDuration = 0.6f; // Temps pendant lequel l’attaque est active

    void Update()
    {
        if (isAttacking) return;

        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            StartCoroutine(PerformAttack(false));
        }

        if (Input.GetMouseButtonDown(1) && Time.time >= nextHeavyAttackTime)
        {
            nextHeavyAttackTime = Time.time + heavyAttackCooldown;
            StartCoroutine(PerformAttack(true));
        }
    }

    System.Collections.IEnumerator PerformAttack(bool isHeavy)
    {
        isAttacking = true;

        // Jouer animation
        if (animator)
        {
            if (isHeavy)
                animator.SetTrigger(heavyAttackTrigger);
            else if (attackAnimations.Length > 0)
                animator.SetTrigger(attackAnimations[Random.Range(0, attackAnimations.Length)]);
        }

        // Jouer son
        if (audioSource)
        {
            AudioClip clip = isHeavy ? heavyAttackSound : swordSwingSound;
            if (clip) audioSource.PlayOneShot(clip);
        }

        // Détection de cibles
        yield return new WaitForSeconds(0.1f); // léger délai avant le hit

        Collider[] hits = Physics.OverlapSphere(swordTransform.position, attackRange, enemyLayer);
        foreach (Collider hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                float damage = isHeavy ? heavyAttackDamage : baseDamage;
                float finalDamage = CalculateCritical(damage);
                damageable.TakeDamage(finalDamage, gameObject);
            }
        }

        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }

    float CalculateCritical(float damage)
    {
        if (Random.value > 0.9f)
        {
            if (audioSource && criticalHitSound)
                audioSource.PlayOneShot(criticalHitSound);

            return damage * 2f;
        }
        return damage;
    }

    void OnDrawGizmosSelected()
    {
        if (swordTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(swordTransform.position, attackRange);
        }
    }
}
