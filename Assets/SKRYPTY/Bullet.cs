using System;
using Mirror;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public float moveSpeed = 100f;
    private GameObject otherPlayer;
    private playerMulti p;
    public int damageAmount = 10;

    void Update()
    {
        this.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Wall"))
        {
            Destroy(this.gameObject);        
        }

        if (other.tag == "Opponent")
        {
            otherPlayer = other.gameObject;
            DealDamage(otherPlayer);
        }
        
        if (other.tag == "Player")
        {
            otherPlayer = other.gameObject;
            DealDamage(otherPlayer);
        }
        
        if (other.tag == "playerMulti")
        {
            otherPlayer = other.gameObject;
            DealDamage(otherPlayer);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag.Equals("Wall"))
        {
            Destroy(this.gameObject);        
        }

        if (other.transform.tag.Equals("Player"))
        {
            otherPlayer = other.gameObject;
            DealDamage(otherPlayer);
        }
        
        if (other.transform.tag.Equals("Opponent"))
        {
            otherPlayer = other.gameObject;
            DealDamage(otherPlayer);
        }
        
        if (other.transform.tag.Equals("playerMulti"))
        {
            otherPlayer = other.gameObject;
            DealDamage(otherPlayer);
        }
    }

    private void DealDamage(GameObject otherPlayer)
    {
        HeroAbstract p = otherPlayer.GetComponent<HeroAbstract>();
        p.getHit(damageAmount);
        Destroy(this.gameObject);
    }

}
