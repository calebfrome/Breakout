using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {  
        if (collision.gameObject.name == "Ball")
        {   
            Color ballColor = collision.gameObject.GetComponent<Renderer>().material.color;
            Color selfColor = GetComponent<Renderer>().material.color;
            print(selfColor + " vs. " + ballColor);
            if (ballColor == selfColor)
            {
                Destroy(gameObject);
            }
            //else
            //{
            //    Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            //}
        }
    }
}
