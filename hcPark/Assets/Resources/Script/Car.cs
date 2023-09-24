using System.Collections;
using System.Collections.Generic;
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
        }
    }
}
