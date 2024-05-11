using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField]
    float _time;

    void Start()
    {
        StartCoroutine(Destruction());
    }

    IEnumerator Destruction() {
        yield return new WaitForSecondsRealtime(_time);

        Destroy(gameObject);
    }
}
