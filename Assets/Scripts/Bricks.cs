using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {  
        if (collision.gameObject.name.Equals("Ball"))
        {   
            Destroy(gameObject);
        }
    }
}
