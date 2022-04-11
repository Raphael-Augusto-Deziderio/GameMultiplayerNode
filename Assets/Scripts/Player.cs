using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    float speedPos = 10f;
    float speedRot = 500f;
    public bool isLocalPlayer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
        if (isLocalPlayer)
        {
            var hor = Input.GetAxis("Horizontal") * Time.deltaTime * speedRot;
            var vert = Input.GetAxis("Vertical") * Time.deltaTime * speedPos;

            transform.Rotate(0, hor, 0);
            transform.Translate(vert, 0, 0);

            if(hor != 0 || vert != 0)
            {
                UpdateStatustoServer();
            }
        }
    }

    public void UpdateStatustoServer()
    {
        NetworkManager.instance.EmitPosAndRot(transform.position, transform.rotation);
    }

    public void UpdatePosAndRot(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Rigidbody other = collision.gameObject.GetComponent<Rigidbody>();

        if (other.name != this.gameObject.name || other.name != "ground")
        {
            other.AddForce(transform.right * 500);
        }         
    }
}


