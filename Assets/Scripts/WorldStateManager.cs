using UnityEngine;
public enum WorldMood { Harmonious, Neutral, Corrupted }

public class WorldStateManager : MonoBehaviour
{
    public static WorldMood CurrentMood = WorldMood.Neutral;
    public static float harmonyLevel = 0f; // < 0 = destruction, > 0 = aide

    public void UpdateMood(float playerImpact)
    {
        harmonyLevel += playerImpact;

        if (harmonyLevel > 10f) CurrentMood = WorldMood.Harmonious;
        else if (harmonyLevel < -10f) CurrentMood = WorldMood.Corrupted;
        else CurrentMood = WorldMood.Neutral;
    }
}
