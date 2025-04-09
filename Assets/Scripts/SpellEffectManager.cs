using UnityEngine;

public class SpellEffectManager : MonoBehaviour
{
    public GameObject lightSpellEffectPrefab;
    public GameObject arcaneBlastPrefab;
    public GameObject stoneShieldEffectPrefab;

    public void PlayEffect(string spellName, Transform castPoint)
    {
        GameObject effect = null;

        switch (spellName)
        {
            case "Spell of Light":
                effect = lightSpellEffectPrefab;
                break;
            case "Arcane Blast":
                effect = arcaneBlastPrefab;
                break;
            case "Stone Shield":
                effect = stoneShieldEffectPrefab;
                break;
        }

        if (effect != null && castPoint != null)
        {
            Instantiate(effect, castPoint.position, castPoint.rotation);
        }
    }
}
