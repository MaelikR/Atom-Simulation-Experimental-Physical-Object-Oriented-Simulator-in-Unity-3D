// This script controls a Myst-style immersive intro followed by an automatic connection
// and a seamless transition into a second intro based on atom interaction logic

using UnityEngine;
using Fusion;
using System.Collections;

public class MystIntroController : MonoBehaviour
{
    [Header("Intro Sequence Settings")]
    public GameObject mystIntroCameraRig;
    public float mystIntroDuration = 12f;
    public GameObject fadeCanvas;

    [Header("Network Connection")]
    public NetworkRunner runner;
    public FusionBootstrap bootstrap;

    [Header("Intro 2 Trigger")]
    public GameObject intro2TriggerArea;
    public GameObject atomIntroLogicSystem;

    [Header("Player Prefab & Spawner")]
    public NetworkObject playerPrefab;
    public Transform spawnPoint;

    private bool introFinished = false;
    private bool intro2Started = false;

    void Start()
    {
        StartCoroutine(PlayMystIntro());
    }

    IEnumerator PlayMystIntro()
    {
        if (fadeCanvas) fadeCanvas.SetActive(true);

        mystIntroCameraRig.SetActive(true);
        yield return new WaitForSeconds(mystIntroDuration);

        mystIntroCameraRig.SetActive(false);
        if (fadeCanvas) fadeCanvas.SetActive(false);

        StartCoroutine(AutoConnect());
        introFinished = true;
    }

    IEnumerator AutoConnect()
    {
        if (bootstrap != null)
        {
          //  bootstrap.StartGame();
            yield return new WaitForSeconds(1f);

           // runner.Spawn(playerPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    void Update()
    {
        if (!introFinished || intro2Started) return;

        // Check if player is inside trigger to start intro 2
        if (intro2TriggerArea.activeInHierarchy && PlayerInTrigger(intro2TriggerArea))
        {
            StartIntro2();
        }
    }

    bool PlayerInTrigger(GameObject triggerArea)
    {
        Collider col = triggerArea.GetComponent<Collider>();
        if (col == null) return false;

        Collider[] hits = Physics.OverlapBox(col.bounds.center, col.bounds.extents);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player")) return true;
        }
        return false;
    }

    void StartIntro2()
    {
        intro2Started = true;
        atomIntroLogicSystem.SetActive(true);
        Debug.Log("Intro 2 (Atom Logic) Started.");
        // Additional gameplay events can be triggered here
    }
}
