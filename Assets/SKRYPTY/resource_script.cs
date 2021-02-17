using System;
using System.Security.Cryptography;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class resource_script : NetworkBehaviour
{
    private GameObject otherPlayer;
    private playerMulti p;
    public int ammoAmount = 10;
    public int aidKit = 1;
    
    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.transform.tag.Equals("Player"))
    //     {
    //         PlayerControllerTransform p = other.transform.GetComponent<PlayerControllerTransform>();
    //         PlayerControllerTransform.amountOfApteczkaInt++;
    //         float amountOfGatheredAmmo = Random.Range(10, 30);
    //         p.gun.totalAmmo += (int) amountOfGatheredAmmo;
    //         p.weaponHolder.selectedGun.totalAmmo += (int) amountOfGatheredAmmo;
    //         
    //         Destroy(this.gameObject);
    //
    //     }
    //     
    //     if (other.transform.tag.Equals("playerMulti"))
    //     {
    //         otherPlayer = other.gameObject;
    //         AddResources(otherPlayer);
    //         Destroy(this.gameObject);
    //     }
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            PlayerControllerTransform p = other.GetComponent<PlayerControllerTransform>();
            PlayerControllerTransform.amountOfApteczkaInt++;
            float amountOfGatheredAmmo = Random.Range(10, 30);
            p.gun.totalAmmo += (int) amountOfGatheredAmmo;
            p.weaponHolder.selectedGun.totalAmmo += (int) amountOfGatheredAmmo;
            
            Destroy(this.gameObject);

        }
        
        if (other.tag.Equals("playerMulti"))
        {
            otherPlayer = other.gameObject;
            AddResources(otherPlayer);
            Destroy(this.gameObject);
        }
    }

    void AddResources(GameObject o)
    {
        playerMulti pl = o.GetComponent<playerMulti>();
        ammoAmount = Random.Range(10, 30);
        pl.GiveResources(aidKit, ammoAmount);
    }
    
    
}
