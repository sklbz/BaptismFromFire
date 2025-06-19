using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerTeleportation : MonoBehaviour
{
    [SerializeField]
    float _offset;

    float _timer = 0f, _cooldown = 5f;

    void Update()
    {
        _timer += Time.deltaTime;
        if (!Input.GetButtonDown("Teleport") || _timer < _cooldown * Time.timeScale)
            return;

        _timer = 0f;

        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal") * _offset, Input.GetAxisRaw("Vertical") * _offset, 0f);

        transform.position += movement;
    }
}
