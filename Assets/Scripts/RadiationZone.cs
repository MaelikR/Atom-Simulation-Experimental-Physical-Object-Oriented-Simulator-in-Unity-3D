
// Attach to UI Canvas:
// - RawImage as minimap
// - RectTransform icon (UI dot)

// -------------------------
// 🔴 RADIATION ZONE BLIP
// -------------------------
using UnityEngine;

public class RadiationZone : MonoBehaviour
{
	public float radius = 5f;
	public Color color = Color.red;
	void OnDrawGizmosSelected()
	{
		Gizmos.color = color;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}