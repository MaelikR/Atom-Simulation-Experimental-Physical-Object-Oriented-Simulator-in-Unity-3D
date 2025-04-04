using UnityEngine;
using UnityEngine.UI;

public class MoleculeScanner : MonoBehaviour
{
    [Header("Scanner")]
    public float scanRange = 100f;
    public LayerMask moleculeLayer;
    public Image scanReticle;
    public GameObject scanPanelUI;
    public Text moleculeNameText;
    public Text moleculeStateText;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        scanPanelUI.SetActive(false);
    }

    void Update()
    {
        Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, scanRange, moleculeLayer))
        {
            MoleculeInfo mol = hit.collider.GetComponent<MoleculeInfo>();
            if (mol != null)
            {
                scanPanelUI.SetActive(true);
                moleculeNameText.text = mol.moleculeName;
                moleculeStateText.text = mol.moleculeState;
                scanReticle.color = Color.cyan;
                return;
            }
        }

        // No target
        scanPanelUI.SetActive(false);
        scanReticle.color = Color.white;
    }
}
