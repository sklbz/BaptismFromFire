using UnityEngine;

public class InputWrapper : MonoBehaviour
{
    RectTransform transform;
    float value;

    void Start() {
        transform = GetComponent<RectTransform>();
    }

    public float Horizontal() {
        return transform.localPosition.x / 50f;
    }

    void Update() {
        value = Horizontal();
    }
}
