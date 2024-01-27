using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLightDisableOnPlay : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }
}
