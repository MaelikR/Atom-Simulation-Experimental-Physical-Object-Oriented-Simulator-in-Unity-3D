using UnityEngine;
using Fusion;

public class InGameMenuGUI : NetworkBehaviour
{
   
    private float sensitivity = 2f;
    private float volume = 1f;
    private bool showMenu = false;
    private bool showOptions = false; //  Ajouté ici
    private int menuIndex = 0;

    public override void Spawned()
    {
        if (!Object.HasInputAuthority)
        {
            enabled = false;
            return;
        }

        sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2f);
        volume = PlayerPrefs.GetFloat("Volume", 1f);
        AudioListener.volume = volume;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        showMenu = !showMenu;
        Debug.Log("Menu toggle: " + showMenu);
        Cursor.lockState = showMenu ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = showMenu;
        Time.timeScale = showMenu ? 0.1f : 1f;
    }

    void SaveSettings()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity);
        PlayerPrefs.SetFloat("Volume", volume);
        AudioListener.volume = volume;
        PlayerPrefs.Save();
    }

    void OnGUI()
    {
        if (!Object.HasInputAuthority) return;
        if (!showMenu) return; //  Corrige le fait que le menu s’affiche toujours

        float menuWidth = 300;
        float menuHeight = showOptions ? 230 : 150;
        float x = (Screen.width - menuWidth) / 2f;
        float y = (Screen.height - menuHeight) / 2f;

        GUI.Box(new Rect(x, y, menuWidth, menuHeight), showOptions ? "Options" : "Pause Menu");

        GUILayout.BeginArea(new Rect(x + 10, y + 30, menuWidth - 20, menuHeight - 40));

        if (showOptions)
        {
            GUILayout.Label("Sensibilité : " + sensitivity.ToString("F2"));
            sensitivity = GUILayout.HorizontalSlider(sensitivity, 0.5f, 5f);
            GUILayout.Space(10);
            GUILayout.Label("Volume : " + (volume * 100).ToString("F0") + "%");
            volume = GUILayout.HorizontalSlider(volume, 0f, 1f);

            GUILayout.Space(20);
            if (GUILayout.Button("Retour"))
            {
                showOptions = false;
                SaveSettings();
            }
        }
        else
        {
            if (GUILayout.Button("Reprendre")) ToggleMenu();
            if (GUILayout.Button("Options")) showOptions = true;
            if (GUILayout.Button("Quitter")) Application.Quit();
        }

        GUILayout.EndArea();
    }

    public float GetSensitivity() => sensitivity;

}
