using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityConstraints : MonoBehaviour
{
    Rigidbody2D _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -10f, 50f));
    }
}
