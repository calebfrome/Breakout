using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : MonoBehaviour
{
    public Material[] materials;
    private Renderer rend;
    private int materialType;
    //private bool delete;  //This makes sure that the ball bounces off
    //of the bricks it destroys
    private int delete;
    // Start is called before the first frame update
    void Start()
    {
        materialType = 1;
        rend = GetComponent<Renderer>();
        if (materials.GetLength(0) > 0)
        {
            rend.sharedMaterial = materials[0];
        }
        //delete = false;
        delete = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {  
        if (collision.gameObject.name.Equals("Ball"))
        {
            if (gameObject.layer == 12) //Metallic bricks
            {
                print(rend.material.name.Split(' ')[0]);
                print(materials[2].name);
                if (rend.material.Equals(materials[0]))
                {
                    rend.sharedMaterial = materials[1];
                }
                if (rend.material.name.Split(' ')[0].Equals(materials[1].name))
                {
                    rend.sharedMaterial = materials[2];
                    materialType = 2;
                    return;
                }
                if (rend.material.name.Split(' ')[0].Equals(materials[2].name))
                {
                    rend.sharedMaterial = materials[3];
                    materialType = 3;
                    return;
                }
                if (rend.material.name.Split(' ')[0].Equals(materials[3].name))
                {
                    //delete = true;
                    delete += 1;
                }
            }
            if (gameObject.layer == 9) //Stone bricks
            {
                if (rend.material.Equals(materials[0]))
                {
                    rend.sharedMaterial = materials[1];
                }
                if (rend.material.name.Split(' ')[0].Equals(materials[1].name))
                {
                    rend.sharedMaterial = materials[2];
                    materialType = 2;
                    return;
                }
                if (rend.material.name.Split(' ')[0].Equals(materials[2].name))
                {
                    //delete = true;
                    delete += 1;
                }
            }
            else delete += 1;//delete = true;
        }
    }


    //new
    // Update is called once per frame
    void Update()
    {
        if(delete > 0)
        {
            if(delete > 1) Destroy(gameObject);
            delete += 1;
        }
        string input = Input.inputString;
        switch (input)
        {
            case "1": //pink
                rend.sharedMaterial = materials[0];
                break;
            case "2": //stone
                rend.sharedMaterial = materials[0];
                if (gameObject.layer == 9)
                    rend.sharedMaterial = materials[materialType];
                break;
            case "3": //light blue
                rend.sharedMaterial = materials[0];
                if (gameObject.layer == 10)
                    rend.sharedMaterial = materials[1];
                break;
            case "4": //metal
                rend.sharedMaterial = materials[0];
                if (gameObject.layer == 12)
                    rend.sharedMaterial = materials[materialType];
                break;
        }
    }
}
