using UnityEngine;

public class HumanoidEnemyShooter : MonoBehaviour
{
    [Header("Detection & Targeting")]
    public Transform player;
    public float detectionRange = 25f;
    public float rotationSpeed = 3f;

    [Header("Gun Settings")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireRate = 1.5f;
    public float bulletSpeed = 40f;
    private float nextFireTime;

    [Header("Animation")]
    public Animator animator; // optional
    public string shootTrigger = "Shoot";

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= detectionRange)
        {
            RotateTowards(player.position);

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void RotateTowards(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        direction.y = 0; // Lock rotation on Y axis
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void Shoot()
    {
        if (animator != null)
            animator.SetTrigger(shootTrigger);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = firePoint.forward * bulletSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
