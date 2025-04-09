using UnityEngine;
using Fusion;
using System.Collections.Generic;


public class CharacterStats : NetworkBehaviour
{
    [Networked] public int Strength { get; set; }
    [Networked] public int Agility { get; set; }
    [Networked] public int Intelligence { get; set; }
    [Networked] public int Vitality { get; set; }
    [Networked] public int Spirit { get; set; }
    [Networked] public int StatPoints { get; set; } = 5;
    [Networked] public int Experience { get; set; }
    [Networked] public int ExperienceToNextLevel { get; set; }

    [Networked] public int Level { get; set; }
    [Networked] public int CurrentHP { get; set; }
    [Networked] public int MaxHP { get; set; }
    [Networked] public int Energy { get; set; }

    public string characterName = "Eldorian";
    public Texture2D profileImage;
    public List<Spell> unlockedSpells = new List<Spell>();

    public void CheckForNewSpells()
    {
        if (Intelligence >= 20 && !HasSpell("Arcane Blast"))
        {
            Spell newSpell = new Spell("Arcane Blast", "Unleash a powerful blast of arcane energy.");
            unlockedSpells.Add(newSpell);
            Debug.Log("New spell unlocked: " + newSpell.Name);
        }
    }

    private bool HasSpell(string spellName)
    {
        return unlockedSpells.Exists(spell => spell.Name == spellName);
    }

    public void GainExperience(int amount)
    {
        Experience += amount;
        if (Experience >= ExperienceToNextLevel)
        {
            LevelUp();
            CheckForNewSpells();
        }
    }
    public void LevelUp()
    {
        Level += 1;
        StatPoints += 3; // Tu peux ajuster le nombre de points gagnés par niveau
        Experience = 0;
        ExperienceToNextLevel += 100; // Augmente la difficulté

        MaxHP += 20;
        CurrentHP = MaxHP;

        Debug.Log($"Level Up! You are now level {Level}.");
    }

    public Texture2D Icon;

    [System.Serializable]
    public class Spell
    {
        public string Name;
        public string Description;
        public Texture2D Icon;

        public Spell(string name, string description, Texture2D icon = null)
        {
            Name = name;
            Description = description;
            Icon = icon;
        }
    }

}
