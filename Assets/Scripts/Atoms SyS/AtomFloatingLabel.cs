using UnityEngine;

public class AtomFloatingLabel : MonoBehaviour
{
    public TextMesh label;
    public Atom atom;

    void Update()
    {
        if (atom == null) return;

        label.text = atom.element;
        label.transform.rotation = Quaternion.LookRotation(label.transform.position - Camera.main.transform.position);
    }
}
