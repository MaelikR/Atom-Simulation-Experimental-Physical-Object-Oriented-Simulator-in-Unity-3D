// =========================
// FusionBootstrapDebugGUI.cs — Clean MonoBehaviour Version
// =========================
using UnityEngine;
using Fusion;

public class FusionBootstrapDebugGUI : MonoBehaviour
{
    private FusionBootstrap _networkDebugStart;
    private bool isClientConnected = false;

    private void Awake()
    {
        _networkDebugStart = FindObjectOfType<FusionBootstrap>();
        if (_networkDebugStart == null)
        {
            Debug.LogError("[FusionBootstrapDebugGUI] No FusionBootstrap found in scene.");
        }
    }

    private void OnGUI()
    {
        if (isClientConnected) return;

        float buttonWidth = 300f;
        float buttonHeight = 50f;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float buttonX = (screenWidth - buttonWidth) / 2;
        float buttonY = screenHeight / 3 - buttonHeight / 2;

        if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "Enter World"))
        {
            _networkDebugStart?.StartSharedClient();
            isClientConnected = true;
        }

        if (GUI.Button(new Rect(buttonX, buttonY + buttonHeight + 20f, buttonWidth, buttonHeight), "Quit Game"))
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
