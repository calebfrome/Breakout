using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 myVector = new Vector3(moveHorizontal*25, 0.0f, 0.0f);

        //rb.AddForce(moveHorizontal*50, 0.0f, 0.0f);
        //rb.AddForce(moveHorizontal, 0.0f, 0.0f, ForceMode.VelocityChange);
        rb.velocity = myVector;
    }
}
