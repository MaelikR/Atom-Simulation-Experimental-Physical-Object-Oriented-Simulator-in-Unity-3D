using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    public QuestManager questManager;
    public string questToComplete;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            questManager.CompleteQuest(questToComplete);
        }
    }
}
