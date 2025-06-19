using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constraint : MonoBehaviour
{
    void Update()
    {
        transform.position = new(transform.position.x, 0);
        
    }
}
