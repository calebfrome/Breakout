﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubePaddleController : MonoBehaviour
{
    public GameObject[] otherPartOfPaddle;

    private void Start()
    {
    }

    void FixedUpdate()
    {
        float moveHorizontal;
        Vector3 myVector;
        //Vector3 otherVector;
        double dx;
        //float sqrt2 = Mathf.Sqrt(2.0f);

        if (gameObject.layer == 16)
        {
            moveHorizontal = Input.GetAxis("Horizontal");

            myVector = new Vector3(moveHorizontal * 25, 0.0f, 0.0f);
            //myVector = new Vector3(moveHorizontal * 13 * sqrt2, moveHorizontal * 13 * sqrt2, 0.0f);
            //otherVector = new Vector3(moveHorizontal * 13 * sqrt2, -moveHorizontal * 13 * sqrt2, 0.0f);

            dx = myVector.x * Time.deltaTime;
            if (this.transform.position.x + dx < (10.5) && this.transform.position.x + dx > (-10.5))
            {
                transform.Translate(myVector * Time.deltaTime);

                foreach (GameObject part in otherPartOfPaddle)
                {
                    part.transform.Translate(myVector * Time.deltaTime);
                }

                //otherPartOfPaddle.transform.Translate(otherVector * Time.deltaTime);
            }
        }
    }
}
