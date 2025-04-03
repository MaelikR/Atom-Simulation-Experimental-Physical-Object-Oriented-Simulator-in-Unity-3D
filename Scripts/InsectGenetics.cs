using UnityEngine;

public class InsectGenetics : MonoBehaviour
{
    public InsectDNA dna;

    void Start()
    {
        if (dna == null)
            dna = InsectDNA.GenerateRandom();
    }
}
