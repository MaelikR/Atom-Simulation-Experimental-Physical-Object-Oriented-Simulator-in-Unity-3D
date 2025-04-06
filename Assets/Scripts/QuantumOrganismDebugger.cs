using UnityEngine;

public class QuantumOrganismDebugger : MonoBehaviour
{
    private LivingOrganism organism;
    private GUIStyle labelStyle;

    void Start()
    {
        organism = GetComponent<LivingOrganism>();

        labelStyle = new GUIStyle();
        labelStyle.fontSize = 14;
        labelStyle.normal.textColor = Color.cyan;
    }
}
