// AtomMusicSynth.cs
using UnityEngine;
using Fusion;
using System.Collections.Generic;

public class AtomMusicSynth : NetworkBehaviour
{
    public AudioClip baseNote;
    public AudioSource audioPrefab;
    public float minSpeed = 1f;
    public float timeBetweenNotes = 0.2f;

    private float timeSinceLastNote = 0f;

    [Networked] private TickTimer noteCooldown { get; set; }

    void Update()
    {
        if (!HasStateAuthority || noteCooldown.ExpiredOrNotRunning(Runner) == false)
            return;

        var atoms = FindObjectsOfType<Atom>();
        foreach (var atom in atoms)
        {
            if (atom == null) continue;

            float speed = atom.GetComponent<Rigidbody>().velocity.magnitude;
            if (speed > minSpeed)
            {
                RPC_PlayNote(atom.transform.position, speed);
                noteCooldown = TickTimer.CreateFromSeconds(Runner, timeBetweenNotes);
                break;
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_PlayNote(Vector3 position, float speed)
    {
        AudioSource a = Instantiate(audioPrefab, position, Quaternion.identity);
        a.clip = baseNote;
        a.pitch = 1f + speed * 0.1f;
        a.volume = Mathf.Clamp01(speed / 10f);
        a.spatialBlend = 1f;
        a.Play();
        Destroy(a.gameObject, baseNote.length);
    }
}
