using UnityEngine;
using UnityEngine.UI;

public class ReactionFeedUI : MonoBehaviour
{
    public Text reactionText;

    public void ShowReaction(string formula)
    {
        reactionText.text = $"<b>Réaction :</b> {formula}";
        CancelInvoke();
        Invoke(nameof(Clear), 5f);
    }

    void Clear()
    {
        reactionText.text = "";
    }
}
