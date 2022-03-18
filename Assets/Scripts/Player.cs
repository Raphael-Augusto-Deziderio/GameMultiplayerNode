using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float velocity;


    // Start is called before the first frame update
    void Start()
    {
        velocity = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            transform.Translate(0, 0, (velocity*Time.deltaTime));
        }

        if (Input.GetKey("s"))
        {
            transform.Translate(0, 0, (-velocity * Time.deltaTime));
        }

        if (Input.GetKey("a"))
        {
            transform.Translate((-velocity * Time.deltaTime), 0, 0);
        }

        if (Input.GetKey("d"))
        {
            transform.Translate((velocity * Time.deltaTime),0 , 0);
        }

    }

}
