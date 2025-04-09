using UnityEngine;
using System.Collections.Generic;

public class SpellHotbarUI : MonoBehaviour
{
    public CharacterStats characterStats;
    public FirstPersonCamera player;
    public GUIStyle hotbarStyle;
    public GUIStyle keyStyle;

    private KeyCode[] hotkeys = {
        KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3,
        KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6
    };

    void Update()
    {
        if (player == null || characterStats == null || !player.Object.HasInputAuthority) return;

        for (int i = 0; i < characterStats.unlockedSpells.Count && i < hotkeys.Length; i++)
        {
            if (Input.GetKeyDown(hotkeys[i]))
            {
                string spellName = characterStats.unlockedSpells[i].Name;
                player.CastSpell(spellName);
            }
        }
    }

    void OnGUI()
    {
        if (characterStats == null || characterStats.unlockedSpells.Count == 0) return;

        float barWidth = 64 * hotkeys.Length + 20;
        float posX = (Screen.width - barWidth) / 2f;
        float posY = Screen.height - 80f;

        for (int i = 0; i < hotkeys.Length; i++)
        {
            Rect boxRect = new Rect(posX + i * 64, posY, 60, 60);

            if (i < characterStats.unlockedSpells.Count)
            {
                var spell = characterStats.unlockedSpells[i];
                GUI.Box(boxRect, spell.Name, hotbarStyle);
                GUI.Label(new Rect(boxRect.x + 4, boxRect.y + 42, 50, 20), hotkeys[i].ToString().Replace("Alpha", ""), keyStyle);
            }
            else
            {
                GUI.Box(boxRect, "", hotbarStyle);
            }
        }
    }
}
