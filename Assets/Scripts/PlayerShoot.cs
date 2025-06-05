using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Transform handPoint;           // Empty placé dans la main du joueur
    public GameObject bulletPrefab;       // Prefab de la balle
    public float bulletForce = 20f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clic gauche
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, handPoint.position, handPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(handPoint.forward * bulletForce, ForceMode.Impulse);
        }
    }
}
