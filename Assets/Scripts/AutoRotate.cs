using UnityEngine;
public class AutoRotate : MonoBehaviour
{
    void Update() => transform.Rotate(Vector3.up * Time.deltaTime * 10f);
}
