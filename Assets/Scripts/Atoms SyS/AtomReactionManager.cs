using UnityEngine;
using System.Collections.Generic;

public class AtomReactionManager : MonoBehaviour
{
    public float reactionRadius = 1.5f;

    void Update()
    {
        Atom[] atoms = FindObjectsOfType<Atom>();

        foreach (Atom atom in atoms)
        {
            Collider[] nearby = Physics.OverlapSphere(atom.transform.position, reactionRadius);

            List<Atom> neighbors = new List<Atom>();

            foreach (Collider col in nearby)
            {
                Atom other = col.GetComponent<Atom>();
                if (other != null && other != atom)
                {
                    neighbors.Add(other);
                }
            }
            FindObjectOfType<ReactionFeedUI>().ShowReaction("2H + O ? H?O");

            TryReact(atom, neighbors);
        }
    }

    void TryReact(Atom atom, List<Atom> neighbors)
    {
        // Exemple : H + H + O = H2O
        int hCount = 0;
        bool hasO = false;

        foreach (Atom a in neighbors)
        {
            if (a.element == "H") hCount++;
            if (a.element == "O") hasO = true;
        }

        if (atom.element == "O" && hCount >= 2 && hasO == false)
        {
            CreateMolecule("H2O", atom.transform.position);
            Destroy(atom.gameObject);
            foreach (Atom a in neighbors)
            {
                if (a.element == "H")
                {
                    Destroy(a.gameObject);
                    hCount--;
                    if (hCount <= 0) break;
                }
            }
        }
    }

    void CreateMolecule(string moleculeName, Vector3 pos)
    {
        Debug.Log($"Création de la molécule : {moleculeName} à {pos}");
        // FX, VFX, prefab, ou autre
    }
}
