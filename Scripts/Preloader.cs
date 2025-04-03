using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Preloader : MonoBehaviour
{
    [Header("Assets à précharger")]
    public AudioClip[] audioClips;
    public GameObject[] prefabs;
    public Texture[] textures;
    public Material[] materials;
    public GameObject playerController;
    public GameObject loadingUI;
    public float delayBeforeStart = 1f;

    private List<Object> preloadedAssets = new List<Object>();

    void Start()
    {
        if (loadingUI != null)
            loadingUI.SetActive(true);

        playerController.SetActive(false);
        StartCoroutine(PreloadAssets());
    }

    IEnumerator PreloadAssets()
    {
        yield return new WaitForSeconds(delayBeforeStart);

        // Précharge Audio
        foreach (AudioClip clip in audioClips)
        {
            AudioSource temp = gameObject.AddComponent<AudioSource>();
            temp.clip = clip;
            temp.playOnAwake = false;
            temp.Stop(); // Ne joue rien
            preloadedAssets.Add(temp);
        }

        // Précharge Prefabs (instanciation invisible puis destruction)
        foreach (GameObject prefab in prefabs)
        {
            GameObject instance = Instantiate(prefab);
            instance.SetActive(false);
            preloadedAssets.Add(instance);
            yield return null;
            Destroy(instance);
        }

        // Textures
        foreach (Texture tex in textures)
        {
            tex.GetNativeTexturePtr(); // Force chargement dans GPU
            yield return null;
        }

        // Matériaux
        foreach (Material mat in materials)
        {
            mat.SetFloat("_Glossiness", mat.GetFloat("_Glossiness")); // Force parsing
            yield return null;
        }

        yield return new WaitForSeconds(0.5f); // Petit tampon de sécurité

        if (loadingUI != null)
            loadingUI.SetActive(false);

        playerController.SetActive(true);
        Debug.Log("✅ Tous les assets critiques ont été préchargés.");
    }
}
