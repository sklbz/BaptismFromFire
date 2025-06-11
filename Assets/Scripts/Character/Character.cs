using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    LifeSign[] lifeSigns;
    void Start() {
        lifeSigns = GetComponentsInChildren<LifeSign>(true);
    }

    void Update() {
        
    }

    public void Die() {
        foreach(LifeSign sign in lifeSigns)
        {
            sign.gameObject.SetActive(false);
        }

    }

    public void Resurect() {
        foreach (LifeSign sign in lifeSigns)
        {
            sign.gameObject.SetActive(true);
        }
    }
}
