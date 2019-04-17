using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickTransparency : MonoBehaviour
{
    public Material[] materials;
    private Rigidbody rb;
    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        rend.sharedMaterial = materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        /*string input = Input.inputString;
        switch (input)
        {
            case "1": //pink
                rend.sharedMaterial = materials[0];
                break;
            case "2": //dark blue
                rend.sharedMaterial = materials[0];
                if (gameObject.layer == 9)
                    rend.sharedMaterial = materials[1];
                break;
            case "3": //light blue
                rend.sharedMaterial = materials[0];
                if (gameObject.layer == 10)
                    rend.sharedMaterial = materials[1];
                break;
            case "4": //metal
                rend.sharedMaterial = materials[0];
                if (gameObject.layer == 12)
                    rend.sharedMaterial = materials[1];
                break;
        }*/
    }
}
