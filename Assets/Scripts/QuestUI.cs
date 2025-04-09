// 🔮 Extension du GUI avec SpellBookUI + EquipmentSlots
using UnityEngine;
using System.Collections.Generic;

public class CharacterSheetUI : MonoBehaviour
{
    public QuestManager questManager;
    public CharacterStats characterStats;
    public Texture2D backgroundOverlay;
    public Texture2D guiBackgroundTexture;
    public Texture2D[] equipmentIcons; // Casque, armure, arme, artefact...
    public GUIStyle spellTooltipStyle;
    public FirstPersonCamera player;
    public GUIStyle infoStyle;
    private bool showSheet = false;
    private string hoveredSpellDescription = "";

    private Rect[] equipmentSlots = new Rect[4];
    private string[] equipmentLabels = { "Helmet", "Armor", "Weapon", "Artifact" };

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            showSheet = !showSheet;
    }

    void OnGUI()
    {
        if (!showSheet || characterStats == null) return;

        float padding = 10f;
        float boxWidth = 880f;
        float boxHeight = 620f;
        float posX = (Screen.width - boxWidth) / 2f;
        float posY = (Screen.height - boxHeight) / 2f;

        if (guiBackgroundTexture != null)
            GUI.DrawTexture(new Rect(posX, posY, boxWidth, boxHeight), guiBackgroundTexture, ScaleMode.StretchToFill);
        else if (backgroundOverlay != null)
            GUI.DrawTexture(new Rect(posX, posY, boxWidth, boxHeight), backgroundOverlay, ScaleMode.StretchToFill);

        GUI.Box(new Rect(posX, posY, boxWidth, boxHeight), "", GUI.skin.window);
        GUI.Label(new Rect(posX + 20, posY + 10, boxWidth, 40), "✦ Eldryana, the Red-Spear ✦");

        // --- Portrait ---
        GUI.DrawTexture(new Rect(posX + 30, posY + 60, 180, 240), characterStats.profileImage, ScaleMode.ScaleToFit);

        float infoX = posX + 230;
        float infoY = posY + 60;

        GUI.Label(new Rect(infoX, infoY, 260, 25), $"Level: {characterStats.Level}");
        GUI.Label(new Rect(infoX, infoY + 30, 260, 25), $"HP: {characterStats.CurrentHP} / {characterStats.MaxHP}");
        GUI.Label(new Rect(infoX, infoY + 60, 260, 25), $"Energy: {characterStats.Energy}");
        GUI.Label(new Rect(infoX, infoY + 90, 260, 25), $"Stat Points: {characterStats.StatPoints}");

        GUI.Box(new Rect(infoX, infoY + 130, 250, 170), "Core Attributes");
        float statX = infoX + 15;
        float statY = infoY + 155;
        DrawStatRow("STR", statX, statY, 0);
        DrawStatRow("AGI", statX, statY, 1);
        DrawStatRow("INT", statX, statY, 2);
        DrawStatRow("VIT", statX, statY, 3);
        DrawStatRow("SPR", statX, statY, 4);

        // --- Spells ---
        GUI.Box(new Rect(posX + 520, posY + 60, 320, 200), "Spells Unlocked");
        int skillYOffset = 0;
        for (int i = 0; i < characterStats.unlockedSpells.Count; i++)
        {
            var spell = characterStats.unlockedSpells[i];
            Rect spellRect = new Rect(posX + 535, posY + 85 + skillYOffset, 180, 20);
            GUI.Label(spellRect, $"• {spell.Name}");

            if (GUI.Button(new Rect(spellRect.x + 190, spellRect.y, 60, 20), "Cast"))
            {
                player?.ExecuteSpell(spell.Name);
            }

            if (spellRect.Contains(Event.current.mousePosition))
            {
                hoveredSpellDescription = spell.Description;
                GUI.Box(new Rect(Event.current.mousePosition.x + 15, Event.current.mousePosition.y + 15, 250, 60), hoveredSpellDescription, spellTooltipStyle);
            }

            skillYOffset += 30;
        }


        // --- Equipment Slots ---
        GUI.Box(new Rect(posX + 530, posY + 270, 300, 120), "Equipment");
        for (int i = 0; i < 4; i++)
        {
            float slotX = posX + 545 + i * 70;
            float slotY = posY + 300;
            equipmentSlots[i] = new Rect(slotX, slotY, 64, 64);
            GUI.Box(equipmentSlots[i], equipmentIcons.Length > i && equipmentIcons[i] != null ? equipmentIcons[i] : Texture2D.grayTexture);
            GUI.Label(new Rect(slotX, slotY + 68, 64, 20), equipmentLabels[i], GUI.skin.label);
        }

        // --- Quests ---
        if (questManager != null)
        {
            float questBoxWidth = 400f;
            float questBoxHeight = 160f;
            float questPosX = posX + 30;
            float questPosY = posY + boxHeight - questBoxHeight - 20f;

            GUI.Box(new Rect(questPosX, questPosY, questBoxWidth, questBoxHeight), "Active Quests");
            int yOffset = 25;
            foreach (var quest in questManager.activeQuests)
            {
                string status = quest.isComplete ? "[✔]" : "[ ]";
                GUI.Label(new Rect(questPosX + 10, questPosY + yOffset, questBoxWidth - 20, 20), $"{status} {quest.title}");
                GUI.Label(new Rect(questPosX + 20, questPosY + yOffset + 18, questBoxWidth - 30, 40), quest.description);
                yOffset += 60;
            }
        }
        float infoBoxX = 30f;
        float infoBoxY = posY + boxHeight + 10f;

        if (player == null || !player.Object.HasInputAuthority) return;
        GUI.Label(new Rect(infoBoxX, infoBoxY, 300f, 25f), $"HP: {player.MaxHealth} / {player.CurrentHealth}", infoStyle);

        GUI.Label(new Rect(infoBoxX, infoBoxY + 25f, 300f, 25f), $"Mode: {(player.IsFlying ? "Flying" : player.IsSwimming ? "Swimming" : "Ground")}", infoStyle);
        GUI.Label(new Rect(infoBoxX, infoBoxY + 50f, 300f, 25f), $"AutoRun: {(player.IsAutoRunning ? "Yes" : "No")}", infoStyle);

    }

    void DrawStatRow(string label, float x, float y, int index)
    {
        float offsetY = y + index * 25f;
        int value = label switch
        {
            "STR" => characterStats.Strength,
            "AGI" => characterStats.Agility,
            "INT" => characterStats.Intelligence,
            "VIT" => characterStats.Vitality,
            "SPR" => characterStats.Spirit,
            _ => 0
        };

        GUI.Label(new Rect(x, offsetY, 100, 20), $"{label}: {value}");
        if (characterStats.StatPoints > 0 && GUI.Button(new Rect(x + 105, offsetY, 25, 20), "+"))
        {
            switch (label)
            {
                case "STR": characterStats.Strength += 1; break;
                case "AGI": characterStats.Agility += 1; break;
                case "INT": characterStats.Intelligence += 1; break;
                case "VIT": characterStats.Vitality += 1; break;
                case "SPR": characterStats.Spirit += 1; break;
            }
            characterStats.StatPoints -= 1;
        }
    }
}