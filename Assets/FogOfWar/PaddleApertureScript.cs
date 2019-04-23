using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleApertureScript : MonoBehaviour
{
    public int paddleType;
    private Renderer rend;
    private Collider myCollider;

    // Start is called before the first frame update
    void Start()
    {
        if (paddleType == 1)
        {
            //gameObject.SetActive(true);
            rend.enabled = true;
            myCollider.enabled = true;
        }
        else
        {
            rend.enabled = false;//gameObject.SetActive(false);
            myCollider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
