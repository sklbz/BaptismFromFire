using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireAnim : MonoBehaviour
{
    Animator _anim;
    void Start()
    {
        _anim = transform.GetChild(0).GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log(collider);
        if (!collider.CompareTag("Player"))
            return;

        _anim.SetTrigger("Light");
    }
}
