using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeScaleManager : MonoBehaviour
{
    [SerializeField]
    float timeScaler;
    void Update()
    {
        Time.timeScale = timeScaler;
    }
}
