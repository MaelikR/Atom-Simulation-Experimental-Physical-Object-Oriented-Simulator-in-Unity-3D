using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public List<Quest> activeQuests = new List<Quest>();

    void Start()
    {
        // Exemple d'ajout de quête
        AddQuest(new Quest("Enter the Atom Ruins", "Find the red portal near the lion statues."));
    }

    public void AddQuest(Quest quest)
    {
        activeQuests.Add(quest);
    }

    public void CompleteQuest(string questTitle)
    {
        Quest quest = activeQuests.Find(q => q.title == questTitle);
        if (quest != null) quest.isComplete = true;
    }
}
