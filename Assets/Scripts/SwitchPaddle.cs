﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPaddle : MonoBehaviour
{
    public int paddleType;
    private Renderer rend;
    private Collider myCollider;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        myCollider = GetComponent<Collider>();

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
        string input = Input.inputString;
        switch (input)
        {
            case "1": //pink
                if (paddleType == 1)
                {
                    //gameObject.SetActive(false);
                    rend.enabled = true;
                    myCollider.enabled = true;
                }
                else
                {
                    rend.enabled = false;//gameObject.SetActive(false);
                    myCollider.enabled = false;
                }
                break;
            case "2": //stone
                if (paddleType == 2)
                {
                    //gameObject.SetActive(false);
                    rend.enabled = true;
                    myCollider.enabled = true;
                }
                else
                {
                    rend.enabled = false;//gameObject.SetActive(false);
                    myCollider.enabled = false;
                }
                break;
            case "3": //light blue
                if (paddleType == 3 || paddleType == 1) //temporary
                {
                    //gameObject.SetActive(false);
                    rend.enabled = true;
                    myCollider.enabled = true;
                }
                else
                {
                    rend.enabled = false;//gameObject.SetActive(false);
                    myCollider.enabled = false;
                }
                break;
            case "4": //metal
                if (paddleType == 4)
                {
                    //gameObject.SetActive(false);
                    rend.enabled = true;
                    myCollider.enabled = true;
                }
                else
                {
                    rend.enabled = false;//gameObject.SetActive(false);
                    myCollider.enabled = false;
                }
                break;
        }
    }
}
