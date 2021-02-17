using UnityEngine;

public class M16_script : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip shoot;
    public AudioClip getTheGun;
    public AudioClip reloadGun;
    public StrzalPistolet strzalPistolet;


    private void Start()
    {
        strzalPistolet = GetComponent<StrzalPistolet>();
    }

    public void takeShoot()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(shoot);
            strzalPistolet.strzal();
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
}
