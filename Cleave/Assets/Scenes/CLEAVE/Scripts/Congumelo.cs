using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Congumelo : MonoBehaviour
{
    public float jumpForce = 10f;

    private Vector3 baseScale;

    private void Start()
    {
        baseScale = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = other.gameObject.GetComponent<Rigidbody2D>();
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0f);
            playerRigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            MakeBounce();
        }
    }

    private void MakeBounce()
    {
        Invoke("JumpStart", 0f);
        Invoke("Jumping", 0.15f);
        Invoke("JumpEnd", 0.3f);
    }

    private void JumpStart()
    {
        transform.localScale = baseScale * 0.8f;
    }

    private void Jumping()
    {
        transform.localScale = baseScale * 1.2f;
    }

    private void JumpEnd()
    {
        transform.localScale = baseScale;
    }
}
