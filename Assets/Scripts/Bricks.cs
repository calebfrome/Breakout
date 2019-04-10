using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : MonoBehaviour
{
    public Material[] materials;
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        if (materials.GetLength(0) > 0)
        {
            rend.sharedMaterial = materials[0];
        }
    }

    private void OnCollisionEnter(Collision collision)
    {  
        if (collision.gameObject.name.Equals("Ball"))
        {
            if (gameObject.layer == 12)
            {
                print("Hit layer 12");
                print(rend.material.name.Split(' ')[0]);
                print(materials[2].name);
                if (rend.material.Equals(materials[0]))
                {
                    print("this was the problem.");
                    rend.sharedMaterial = materials[1];
                }
                if (rend.material.name.Split(' ')[0].Equals(materials[1].name)) //(rend.material.Equals(materials[1]))
                {
                    print("In if statement");
                    rend.sharedMaterial = materials[2];
                    return;
                }
                if (rend.material.name.Split(' ')[0].Equals(materials[2].name))
                {
                    print("second if statement");
                    rend.sharedMaterial = materials[3];
                    return;
                }
                if (rend.material.name.Split(' ')[0].Equals(materials[3].name))
                {
                    Destroy(gameObject);
                }
            }
            else Destroy(gameObject);
        }
    }
}
