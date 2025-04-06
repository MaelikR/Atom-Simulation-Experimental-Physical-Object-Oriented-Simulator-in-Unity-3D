using UnityEngine;

public static class AtomMirageAudio
{
    private static AudioSource source;

    public static void PlayEffect()
    {
        if (source == null)
        {
            GameObject audioObj = new GameObject("AtomMirageAudio");
            source = audioObj.AddComponent<AudioSource>();
            source.spatialBlend = 0f;
            source.volume = 0.8f;
        }

        // Remplace par ton clip dans Resources
        AudioClip clip = Resources.Load<AudioClip>("Audio/MirageAppear");
        if (clip != null)
        {
            source.PlayOneShot(clip);
        }
    }
}
