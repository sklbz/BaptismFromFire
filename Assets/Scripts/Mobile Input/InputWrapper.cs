using UnityEngine;

public class InputWrapper : MonoBehaviour
{
    new RectTransform transform;

    void Start() {
        transform = GetComponent<RectTransform>();
    }

    public float Horizontal() {
        return transform.localPosition.x / 100f;
    }
}
