using UnityEngine;

public class PortalActivator : MonoBehaviour
{
    public QuestManager questManager;
    public GameObject portalToActivate;

    void Update()
    {
        if (questManager != null)
        {
            bool isMegalodonKilled = questManager.activeQuests.Exists(q => q.title == "Kill a Megalodon." && q.isComplete);

            if (isMegalodonKilled && portalToActivate != null && !portalToActivate.activeSelf)
            {
                portalToActivate.SetActive(true);
                Debug.Log("Portal activated!");
            }
        }
    }
}
