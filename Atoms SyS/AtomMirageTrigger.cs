using UnityEngine;

public class AtomMirageTrigger : MonoBehaviour
{
    [Header("Mirage Conditions")]
    public Transform playerCamera;
    public float requiredViewAngle = 20f; // Angle max entre reflet et regard
    public float energyThreshold = 80f;
    public float checkInterval = 1f;

    [Header("Mirage Settings")]
    public GameObject atomMiragePrefab;
    public Transform mirageSpawnPoint;
    public float mirageDuration = 15f;

    [Header("Environmental References")]
    public ReflectionProbe waterReflection;
    public AtomEnergyField cosmicEnergy;

    private float nextCheckTime;
    private GameObject currentMirage;

    void Update()
    {
        if (Time.time < nextCheckTime) return;
        nextCheckTime = Time.time + checkInterval;

        if (IsLookingAtReflection() && cosmicEnergy.GetCurrentEnergy() >= energyThreshold)

        {
            TriggerMirage();
        }
    }

    bool IsLookingAtReflection()
    {
        Vector3 toReflection = (waterReflection.transform.position - playerCamera.position).normalized;
        float angle = Vector3.Angle(playerCamera.forward, toReflection);
        return angle < requiredViewAngle;
    }

    void TriggerMirage()
    {
        if (currentMirage != null) return;

        currentMirage = Instantiate(atomMiragePrefab, mirageSpawnPoint.position, Quaternion.identity);
        StartCoroutine(DestroyMirageAfterTime(mirageDuration));

        // Optional: Audio & FX
        AtomMirageAudio.PlayEffect();
        AtomMirageVisuals.SpawnDistortion(currentMirage.transform.position);
    }

    System.Collections.IEnumerator DestroyMirageAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (currentMirage != null)
        {
            Destroy(currentMirage);
            currentMirage = null;
        }
    }
}
