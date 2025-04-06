using UnityEngine;
using Fusion;

public class DisableRemoteCamera : NetworkBehaviour
{
    private void Start()
    {
        if (!Object.HasInputAuthority)
        {
            Camera cam = GetComponentInChildren<Camera>(true);
            if (cam != null)
                cam.gameObject.SetActive(false);
        }
    }
}
