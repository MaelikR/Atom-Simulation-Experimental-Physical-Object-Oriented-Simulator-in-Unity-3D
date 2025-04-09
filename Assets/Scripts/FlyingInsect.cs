using UnityEngine;

public class FlyingInsect : MonoBehaviour
{
    public float speed = 1.5f;
    public float swayAmount = 2f;
    public float heightVariation = 1.5f;
    public float changeDirectionInterval = 2f;

    private Vector3 targetDirection;
    private float timer;

    void Start()
    {
        ChangeDirection();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > changeDirectionInterval)
        {
            timer = 0f;
            ChangeDirection();
        }

        // Mouvement sinusoïdal
        Vector3 sway = new Vector3(
            Mathf.Sin(Time.time * swayAmount) * 0.2f,
            Mathf.Sin(Time.time * heightVariation) * 0.3f,
            Mathf.Cos(Time.time * swayAmount) * 0.2f
        );

        transform.position += (targetDirection + sway) * Time.deltaTime * speed;
        transform.Rotate(Vector3.up * Time.deltaTime * 90f);
    }

    void ChangeDirection()
    {
        Vector3 randomDir = Random.insideUnitSphere;
        randomDir.y = Mathf.Abs(randomDir.y); // Toujours voler vers le haut
        targetDirection = randomDir.normalized;
    }
}
