﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    public int attackDamage = 5;

    private void Start()
    {
        Destroy(gameObject, 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            other.gameObject.GetComponent<enemyScript>().TakeDamage(attackDamage);
            Destroy(gameObject);
        }
        else if(other.tag != "Player" && other.tag != "Interactable")
        {
            Destroy(gameObject);
        }


    }
}
