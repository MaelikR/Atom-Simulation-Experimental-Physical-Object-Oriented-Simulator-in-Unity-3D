using UnityEngine;

[System.Serializable]
public class InsectDNA
{
    public float moveSpeed;
    public float turnSpeed;
    public float curiosity;
    public float fleeDistance;

    public static InsectDNA GenerateRandom()
    {
        return new InsectDNA
        {
            moveSpeed = Random.Range(0.5f, 2.0f),
            turnSpeed = Random.Range(1f, 5f),
            curiosity = Random.Range(2f, 6f),
            fleeDistance = Random.Range(1f, 3f)
        };
    }
}
