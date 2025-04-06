using UnityEngine;
using System.Collections;

public class SharkMemory : MonoBehaviour
{
    [Header("Target Memory Settings")]
    public float memoryDuration = 5f; // Durée pendant laquelle le requin se souvient d'une cible perdue de vue
    private GameObject lastSeenTarget;
    private float memoryTimer;

    public GameObject CurrentTarget => IsTargetRemembered() ? lastSeenTarget : null;

    public void RememberTarget(GameObject target)
    {
        lastSeenTarget = target;
        memoryTimer = memoryDuration;
    }

    void Update()
    {
        if (memoryTimer > 0)
        {
            memoryTimer -= Time.deltaTime;
            if (memoryTimer <= 0)
                lastSeenTarget = null;
        }
    }

    public bool IsTargetRemembered()
    {
        return lastSeenTarget != null && memoryTimer > 0;
    }
}