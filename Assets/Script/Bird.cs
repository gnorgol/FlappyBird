using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bird : MonoBehaviour
{
    public float upForce = 200f;
    private bool CanJump = true;
    public float baseJumpDelay = 0.5f;
    private float jumpDelay;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Game Over");
        GameManager.instance.GameOver();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pipe")
        {
            GameManager.instance.GameOver();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GameState != GameState.Playing)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            return;
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
        //if space key is pressed
        if (Input.GetKeyDown("space") || Input.GetMouseButtonDown(0) && CanJump)
        {
            jump();
        }
        if (!CanJump)
        {
            jumpDelay -= Time.deltaTime;
            if (jumpDelay <= 0)
            {
                CanJump = true;
                jumpDelay = baseJumpDelay;
            }
        }

    }
    public void DisableMovement()
    {
        CanJump = false;
    }

    private void jump()
    {
        //add force to bird
        GetComponent<Rigidbody>().velocity = Vector2.zero;
        GetComponent<Rigidbody>().AddForce(new Vector2(0, upForce));
        //set CanJump to false
        CanJump = false;
    }



}
