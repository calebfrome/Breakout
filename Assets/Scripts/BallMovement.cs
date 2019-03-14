using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Renderer rend;
    private float constantSpeed = 11.0f;
    public Material[] materials;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        Vector3 myVector = new Vector3(1.0f * 10, 5.0f, 0.0f);
        Physics.IgnoreLayerCollision(9, 11);
        Physics.IgnoreLayerCollision(10, 11);
        rb.velocity = myVector;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = constantSpeed * (rb.velocity.normalized);
        string input = Input.inputString;
        switch (input)
        {
            case "1":
                rend.sharedMaterial = materials[0];
                Physics.IgnoreLayerCollision(9, 11); //darkblue and ball
                Physics.IgnoreLayerCollision(10, 11); //lightblue and ball
                break;
            case "2":
                rend.sharedMaterial = materials[1];
                Physics.IgnoreLayerCollision(9, 11, false);
                break;
            case "3":
                rend.sharedMaterial = materials[2];
                Physics.IgnoreLayerCollision(10, 11, false);
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {   
        Rigidbody otherRb = collision.rigidbody;
        if (otherRb == null) return;
        
        //For some reason it was having problems when it collided with the
        //paddle, so these are some manual, hacky attempts to fix it.
        if (otherRb.gameObject.name == "Paddle")
        {
            //print("It hit the paddle.");
            //print("velocity before: " + rb.velocity.y);
            float newY = Math.Abs(rb.velocity.y);
            if (Math.Abs(newY) < .01) newY += 5.0f;
            rb.velocity = new Vector3(rb.velocity.x, newY, rb.velocity.z);
            //print(rb.velocity.y);
            //print(collision.relativeVelocity);
        }
        if(otherRb.gameObject.tag.Equals("Brick"))
        {   
            float newY = Math.Abs(rb.velocity.y);
            if (newY < .01) newY += 5.0f;
            rb.velocity = new Vector3(rb.velocity.x, -newY, rb.velocity.z);
            print("collided");
        }
    }
}
