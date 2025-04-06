// -------------------------
// 🌀 SCREEN GLITCH EFFECT
// -------------------------
using UnityEngine;

public class QuantumFlightFX : MonoBehaviour
{
	public Material glitchMat;
	public float intensity = 0f;
	public float fadeSpeed = 2f;
	private bool active = false;

	void Update()
	{
		if (!active) intensity = Mathf.Lerp(intensity, 0, Time.deltaTime * fadeSpeed);
		glitchMat.SetFloat("_Intensity", intensity);
	}

	public void TriggerGlitch(float force = 1f)
	{
		active = true;
		intensity = force;
		Invoke("Stop", 0.3f);
	}

	void Stop() => active = false;
}