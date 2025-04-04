using UnityEngine;
using UnityEngine.UI;

public class ScanLineAnimation : MonoBehaviour
{
    public RawImage scanImage;
    public float scrollSpeed = 0.2f;

    void Update()
    {
        if (scanImage != null)
        {
            Rect uv = scanImage.uvRect;
            uv.y += scrollSpeed * Time.deltaTime;
            scanImage.uvRect = uv;
        }
    }
}
