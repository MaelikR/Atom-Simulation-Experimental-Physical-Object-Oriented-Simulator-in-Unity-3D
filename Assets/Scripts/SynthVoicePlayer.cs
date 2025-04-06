// Apply glitchMat as screen overlay with post-processing / RenderFeature

// -------------------------
//  SYNTHETIC VOICE UI
// -------------------------
using UnityEngine;

public class SynthVoicePlayer : MonoBehaviour
{
	public AudioSource audioSource;
	public AudioClip[] clips;
	public Vector2 pitchRange = new Vector2(0.85f, 1.2f);

	public void PlayRandom()
	{
		if (clips.Length == 0 || audioSource == null) return;
		AudioClip clip = clips[Random.Range(0, clips.Length)];
		audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
		audioSource.PlayOneShot(clip);
	}

	public void PlayClip(int index)
	{
		if (index >= 0 && index < clips.Length)
		{
			audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
			audioSource.PlayOneShot(clips[index]);
		}
	}
}
