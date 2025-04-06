using UnityEngine;
using UnityEngine.UI;

public class AtomUIController : MonoBehaviour
{
    public GameObject panel;
    public Text elementText;
    public Slider energySlider;
    public Text stateText;

    private Atom trackedAtom;

    public void SetTarget(Atom atom)
    {
        trackedAtom = atom;
        panel.SetActive(true);
    }

    void Update()
    {
        if (trackedAtom == null)
        {
            panel.SetActive(false);
            return;
        }

        elementText.text = $"Element : {trackedAtom.element}";
        energySlider.value = trackedAtom.energy / trackedAtom.maxEnergy;
        stateText.text = trackedAtom.isUnstable ? "<color=red>Instable</color>" : "Stable";
    }
}
