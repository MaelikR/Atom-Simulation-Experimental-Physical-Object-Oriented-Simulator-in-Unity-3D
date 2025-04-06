using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    public float dayLengthInSeconds = 120f; // Temps total pour un cycle jour+nuit
   

    [Header("Lighting")]
    public Light directionalLight;
    public Gradient lightColorOverDay;
    public AnimationCurve lightIntensityOverDay;
    public Gradient ambientColorOverDay;
    public static float CurrentTimeOfDay { get; private set; }
    [Header("Rotation")]
    public Vector3 lightRotationAxis = new Vector3(1, 0, 0);
    public float maxSunAngle = 90f;

    void Update()
    {
        CurrentTimeOfDay += Time.deltaTime / dayLengthInSeconds;
        

        if (CurrentTimeOfDay > 1f) CurrentTimeOfDay -= 1f;

        UpdateLighting();
    }
    public static float CurrentHour => CurrentTimeOfDay * 24f;
    void UpdateLighting()
    {
        if (directionalLight == null) return;

        // Rotation du soleil
        float sunAngle = Mathf.Lerp(-maxSunAngle, maxSunAngle, CurrentTimeOfDay);

        directionalLight.transform.rotation = Quaternion.Euler(sunAngle, 0, 0);

        directionalLight.color = lightColorOverDay.Evaluate(CurrentTimeOfDay);
        directionalLight.intensity = lightIntensityOverDay.Evaluate(CurrentTimeOfDay);
        RenderSettings.ambientLight = ambientColorOverDay.Evaluate(CurrentTimeOfDay);

    }
}
