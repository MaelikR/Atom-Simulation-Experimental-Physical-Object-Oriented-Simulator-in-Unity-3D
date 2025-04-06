// ThermalRay.cs
using UnityEngine;

public class ThermalRay : MonoBehaviour
{
    public float heatAmount = 10f;
    public float range = 5f;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, range))
            {
                if (hit.collider.CompareTag("Atom"))
                {
                    var atom = hit.collider.GetComponent<Atom>();
                    if (atom != null)
                    {
                        atom.AddEnergy(heatAmount);
                    }
                }
            }
        }
    }
}