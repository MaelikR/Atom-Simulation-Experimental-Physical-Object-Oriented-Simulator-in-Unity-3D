using UnityEngine;
using Fusion;

public class SubmarineInteractor : NetworkBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 3f;
    public LayerMask submarineLayer;
    public KeyCode interactionKey = KeyCode.E;

    private Camera playerCam;
    private bool isInSubmarine = false;
    private GameObject currentSubmarine;

    public GameObject playerAvatar;
    public GameObject submarineCamera;
    public GameObject submarineController;

    public override void Spawned()
    {
        if (!Object.HasInputAuthority) return;
        playerCam = Camera.main;
    }

    void Update()
    {
        if (!Object.HasInputAuthority || isInSubmarine) return;

        if (Input.GetKeyDown(interactionKey))
        {
            TryEnterSubmarine();
        }
    }

    void TryEnterSubmarine()
    {
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, submarineLayer))
        {
            currentSubmarine = hit.collider.gameObject;
            EnterSubmarine();
        }
    }

    void EnterSubmarine()
    {
        isInSubmarine = true;

        if (playerAvatar != null)
            playerAvatar.SetActive(false);

        if (submarineController != null)
            submarineController.SetActive(true);

        if (submarineCamera != null)
            submarineCamera.SetActive(true);

        Debug.Log("Player has entered the submarine.");
    }
}
