using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    private void Start()
    {
    }

    void FixedUpdate()
    {
        float moveHorizontal;
        Vector3 myVector;
        double dx;

        moveHorizontal = Input.GetAxis("Horizontal");

        myVector = new Vector3(moveHorizontal*25, 0.0f, 0.0f);

        dx = myVector.x * Time.deltaTime;
        if(this.transform.position.x + dx < 12.5 && this.transform.position.x + dx > -12.5)
        {
            transform.Translate(myVector * Time.deltaTime);
        }
    }
}
