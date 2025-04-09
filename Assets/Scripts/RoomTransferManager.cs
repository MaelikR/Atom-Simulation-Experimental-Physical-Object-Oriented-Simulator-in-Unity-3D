// RoomTransferManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;

public class RoomTransferManager : MonoBehaviour
{
    public static string NextRoomName;

    private void Start()
    {
        var runner = FindObjectOfType<NetworkRunner>();
        if (runner == null)
        {
            var newRunnerGO = new GameObject("NetworkRunner");
            runner = newRunnerGO.AddComponent<NetworkRunner>();
            runner.ProvideInput = true;

            runner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.Shared,
                SessionName = NextRoomName,
                Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
                SceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }
    }
}
