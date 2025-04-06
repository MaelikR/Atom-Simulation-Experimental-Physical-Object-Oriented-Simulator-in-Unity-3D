// -------------------------
//  MINI-MAP RAD ZONE SYSTEM
// -------------------------
using UnityEngine;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour
{
	public RectTransform playerIcon;
	public Transform player;
	public Vector2 mapSize = new Vector2(200f, 200f);
	public float worldSize = 100f;

	void Update()
	{
		Vector3 pos = player.position;
		float x = Mathf.Clamp((pos.x / worldSize) * mapSize.x, -mapSize.x / 2, mapSize.x / 2);
		float y = Mathf.Clamp((pos.z / worldSize) * mapSize.y, -mapSize.y / 2, mapSize.y / 2);
		playerIcon.anchoredPosition = new Vector2(x, y);
	}
}