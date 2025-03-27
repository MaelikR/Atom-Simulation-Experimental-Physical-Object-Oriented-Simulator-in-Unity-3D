// AtomMusicSynth.cs
using UnityEngine;
using System.Collections.Generic;

public class AtomMusicSynth : MonoBehaviour
{
    public List<Atom> atoms;
    public AudioClip baseNote;
    public AudioSource audioPrefab;
    public float minSpeed = 1f;
    public float timeBetweenNotes = 0.2f;

    private float timeSinceLastNote = 0f;

    void Update()
    {
        timeSinceLastNote += Time.deltaTime;

        if (timeSinceLastNote < timeBetweenNotes)
            return;

        timeSinceLastNote = 0f;

        foreach (var atom in atoms)
        {
            if (atom == null) continue;

            float speed = atom.GetComponent<Rigidbody>().velocity.magnitude;
            if (speed > minSpeed)
            {
                AudioSource a = Instantiate(audioPrefab, atom.transform.position, Quaternion.identity);
                a.clip = baseNote;
                a.pitch = 1f + speed * 0.1f;
                a.volume = Mathf.Clamp01(speed / 10f);
                a.spatialBlend = 1f; // 3D sound
                a.Play();
                Destroy(a.gameObject, baseNote.length);
            }
        }
    }
}
