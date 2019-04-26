using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Renderer rend;
    private float constantSpeed = 15.0f;
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
        Physics.IgnoreLayerCollision(12, 11);
        rb.velocity = myVector;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = constantSpeed * (rb.velocity.normalized);
        string input = Input.inputString;
        switch (input)
        {
            case "1": //pink
                rend.sharedMaterial = materials[0];
                Physics.IgnoreLayerCollision(9, 11); //darkblue and ball
                Physics.IgnoreLayerCollision(10, 11); //lightblue and ball
                Physics.IgnoreLayerCollision(12, 11); //goldmetal and ball
                Physics.IgnoreLayerCollision(18, 11, false); //pink hexagon and ball
                break;
            case "2": //stone
                rend.sharedMaterial = materials[1];
                Physics.IgnoreLayerCollision(9, 11, false);
                Physics.IgnoreLayerCollision(10, 11);
                Physics.IgnoreLayerCollision(12, 11);
                Physics.IgnoreLayerCollision(18, 11);
                break;
            case "3": //lightblue
                rend.sharedMaterial = materials[2];
                Physics.IgnoreLayerCollision(9, 11);
                Physics.IgnoreLayerCollision(10, 11, false);
                Physics.IgnoreLayerCollision(12, 11);
                Physics.IgnoreLayerCollision(18, 11);
                break;
            case "4": //gold metal
                rend.sharedMaterial = materials[3];
                Physics.IgnoreLayerCollision(9, 11);
                Physics.IgnoreLayerCollision(10, 11);
                Physics.IgnoreLayerCollision(12, 11, false);
                Physics.IgnoreLayerCollision(18, 11);
                break;
        }
    }
}
