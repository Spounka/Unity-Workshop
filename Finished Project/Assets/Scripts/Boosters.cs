using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boosters : MonoBehaviour
{
    public bool isHealth = true, isJump;
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(this.isHealth || this.isJump)
            {
                other.GetComponent<PlayerManager>().Health = other.GetComponent<PlayerManager>().MaxHealth;
                GameManager.GM.boosterTime = GameManager.GM.boosterLimit;
                GameManager.GM.Timer(ref GameManager.GM.boosterTime);
                GameManager.GM.enabledBooster.Remove(this.gameObject);
                Destroy(gameObject);
            }
        }

    }
}
