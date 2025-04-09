using UnityEngine;

public class DistortionShaderController : MonoBehaviour
{
    public Material material;
    public Transform player;
    public float strength = 0.2f;

    void Update()
    {
        if (material != null)
        {
            material.SetVector("_DistortionCenter", player.position);
            material.SetFloat("_DistortionStrength", strength);
        }
    }
}
