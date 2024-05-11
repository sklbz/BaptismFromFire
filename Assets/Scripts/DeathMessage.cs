using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathMessage : MonoBehaviour
{
    Text _text;
    [SerializeField]
    float interpolationRatio;

    int tryCount = 1;
    
    void Awake() {
        _text = GetComponent<Text>();
    }

    public void GenerateMessage() {
        string msg = $"Try n°{tryCount}";

        _text.color = Color.white;
        _text.text = msg;

        StartCoroutine(Fade());

        tryCount++;
    }

    IEnumerator Fade() {
        float alpha = 1f;
        while (alpha >= 0.1)
        {
            alpha = Mathf.Lerp(alpha, 0f, Time.unscaledDeltaTime / interpolationRatio);
            _text.color = Color.HSVToRGB(0, 0, alpha);
            yield return null;
        }
        _text.color = Color.HSVToRGB(0, 0, 0);
    }
}
