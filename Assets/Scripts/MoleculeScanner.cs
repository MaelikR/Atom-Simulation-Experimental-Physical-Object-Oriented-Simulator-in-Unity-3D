using UnityEngine;
using UnityEngine.UI;

public class MoleculeScanner : MonoBehaviour
{
    [Header("Camera & Layer")]
    public Camera mainCam;
    public LayerMask moleculeLayer;
    public float scanRange = 100f;

    [Header("UI")]
    public Image scanReticle;
    public GameObject scanPanelUI;
    public Text moleculeNameText;
    public Text moleculeStateText;

    private MoleculeScanUI scanUI;

    void Start()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
            if (mainCam == null)
            {
                Debug.LogError(" Aucun objet avec le tag 'MainCamera' trouvé. Assigne mainCam dans l'inspecteur.");
                enabled = false;
                return;
            }
        }

        scanUI = GetComponent<MoleculeScanUI>();
        if (scanPanelUI != null)
            scanPanelUI.SetActive(false);
    }

    void Update()
    {
        if (mainCam == null) return; // sécurité supplémentaire

        Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, scanRange, moleculeLayer))
        {
            MoleculeInfo mol = hit.collider.GetComponent<MoleculeInfo>();
            if (mol != null)
            {
                if (scanUI != null) scanUI.Show(mol);
                if (scanReticle != null) scanReticle.color = Color.cyan;
                return;
            }
        }

        if (scanUI != null) scanUI.Hide();
        if (scanReticle != null) scanReticle.color = Color.white;
    }
}
