using UnityEngine;

public class Gun : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip getTheGun;
    public AudioClip reloadGun;
    public AudioClip m1ClingSound;
    public AudioClip emptyJacketSound;

    public float fireRate = 15f;
    public float nextTimeToFire = 0f;

    public int ammoInJacket = 0;
    public int totalAmmo = 0;
    public bool isAllowed = false;
    public int jacketCapacity = 0;

    public Transform bulletSpawnPoint;
    public GameObject bullet;
    public bool canFire = false;

    public void takeShot()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    public void getGun()
    {
        if (audioSource != null)
            audioSource.PlayOneShot(getTheGun);
    }

    public void reload()
    {
        if (audioSource != null)
            audioSource.PlayOneShot(reloadGun);
    }

    public void fireEmptyJacketSound()
    {
        audioSource.PlayOneShot(emptyJacketSound);
    }
    
    public void spawnBullet()
    {
        GameObject _bullet = (GameObject) Instantiate(bullet, bulletSpawnPoint.transform.position, Quaternion.identity);
        _bullet.transform.rotation = bulletSpawnPoint.transform.rotation;
        takeShot();

        if (ammoInJacket == 0 && this.name.Contains("GARAND"))
        {
            audioSource.PlayOneShot(m1ClingSound);
        }
    }
    
}