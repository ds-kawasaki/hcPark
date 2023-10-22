using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Rendering;
using UnityEngine;

public class Car : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Car")
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player)
            {
                MainGame mainGame = player.GetComponent<MainGame>();
                if (mainGame)
                {
                    mainGame.CollisionFromCar();
                }
            }            
            var sa = other.gameObject.transform.position - this.gameObject.transform.position;
            sa.y = 0.0f; //高低差無視
            sa.Normalize();
            sa *= 100.0f;
            sa.y = 200.0f;
            var orb = other.gameObject.GetComponent<Rigidbody>();
            if (orb != null)
            {
                // orb.isKinematic = false;
                orb.AddForce(sa);
            }
            sa *= -1.0f;
            sa.y = 200.0f;
            var trb = this.gameObject.GetComponent<Rigidbody>();
            if (trb != null)
            {
                // trb.isKinematic = false;
                trb.AddForce(sa);
            }
        }
        else if (other.gameObject.tag == "Item")
        {
            Destroy(other.gameObject);
        }
    }
}
