using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class SunRotatorHDRP : MonoBehaviour
{
    [Header("Sun Settings")]
    public Light sunLight;

    public float rotationSpeed = 10f;


    [HideInInspector] public bool isDaytime = true;


    void Start()
    {
      
    }

    void Update()
    {
        if (sunLight == null) return;

        sunLight.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);

        float sunAngle = Vector3.Angle(sunLight.transform.forward, Vector3.down);
        float t = Mathf.InverseLerp(0f, 180f, sunAngle);

    }
}
