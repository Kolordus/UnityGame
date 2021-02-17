using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerControllerTransform : MonoBehaviour, HeroAbstract
{
    private string MoveInputAxis = "Vertical";
    private string TurnInputAxis = "Horizontal";
    public float horizontalSpeed = 3.0F;
    public float verticalSpeed = 3.0F;
    public bool inWater = false;

    public float moveSpeed = 0f;
    public float leftOrRight = 0f;

    private Rigidbody rb;
    public Animator anim;
    public CapsuleCollider boxCollider;
    public bool weapon_out;
    public AimScript aimScript;

    public Text acctualWeaponJacketAmmoText;
    public Text acctualWeaponTotalAmmoText;
    public Text textHealthUI;
    public Image fillageOfHpImage;
    public Text amountOfAidKitsText;
    public static int healthPoints = 100;
    public static int amountOfApteczkaInt = 3;

    public Gun gun;
    public WeaponHolder weaponHolder;
    public string sceneName;
    private bool isDead;
    public float nextTimeToFire = 0f;
    public static int killedOpponents = 0;
    public static bool areOppKilled = false;


    public AudioSource player;
    public AudioClip hit;
    public AudioClip deathHit;
    public AudioClip step;
    public AudioClip inwater;
    public AudioClip in_out_water;

    public float timerToStep = 0f;
    public float stepTime = 0.5f;
    public float waterStepTime = 3f;


    private void Start()
    {
        Cursor.visible = false;
        boxCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        weapon_out = false;
        sceneName = SceneManager.GetActiveScene().name;
        healthPoints = 100;
        Cursor.lockState = CursorLockMode.Locked;
        ////////////////
        gun = weaponHolder.selectedGun;
        RenderHUD();
    }

    private void FixedUpdate()
    {
        gun = weaponHolder.selectedGun;
        sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.Equals("Cave"))
        {
            anim.SetBool("weaponOut", false);
            weapon_out = false;
        }

        if (!isDead)
        {
            if (healthPoints <= 0 && isDead == false)
            {
                death();
            }

            // ws
            float moveAxis = Input.GetAxis(MoveInputAxis);
            //ad
            float turnAxis = Input.GetAxis(TurnInputAxis);
            //shift
            bool shiftKey = Input.GetKey(KeyCode.LeftShift);
            // bool spaceBar = Input.GetKeyDown(KeyCode.Space);
            bool mouseClick = Input.GetMouseButton(0);
            bool mouseClickDragWeapon = Input.GetMouseButtonUp(0);

            //mouse
            float mouseLefRight = horizontalSpeed * Input.GetAxis("Mouse X");
            float mouseUpDown = horizontalSpeed * Input.GetAxis("Mouse Y");


            if (mouseClickDragWeapon && gun.ammoInJacket == 0)
                gun.fireEmptyJacketSound();
            // $$$$$$$$$$$$$$$$$$$$$$$$$$ fire
            if (mouseClick && gun.ammoInJacket > 0 && weapon_out == true && !sceneName.Equals("Cave") &&
                Time.time >= nextTimeToFire)
            {
                gun.ammoInJacket -= 1;
                gun.spawnBullet();
                nextTimeToFire = Time.time + 1f / gun.fireRate;
                anim.SetTrigger("weaponFire");
            }

            // if (Input.GetKey(KeyCode.LeftAlt))
            // {
            //     PlayerControllerTransform.healthPoints -= 10;
            // }

            // if (mouseClick == false)
            //     anim.SetBool("weaponFire", false);

            // $$$$$$$$$$$$$$$$$$$$$$$$$$ weapon out

            if (mouseClickDragWeapon && weapon_out == false && !sceneName.Equals("Cave"))
            {
                anim.SetBool("weaponOut", true);
                anim.SetTrigger("weaponAction");
                gun.getGun();
                weapon_out = true;
            }


            // HIDE WEAPON
            if (Input.GetKeyDown(KeyCode.LeftControl) && weapon_out)
            {
                anim.SetBool("weaponOut", false);
                weapon_out = false;
                gun.getGun();
            }

            // RELOAD
            if (Input.GetKeyDown(KeyCode.R) && gun.ammoInJacket < gun.jacketCapacity)
            {
                ///////
                int actualAmmo = gun.ammoInJacket;
                int ammoNedded = gun.jacketCapacity - actualAmmo;

                if (ammoNedded > 0 && gun.totalAmmo >= ammoNedded)
                {
                    gun.ammoInJacket += ammoNedded;
                    gun.totalAmmo -= ammoNedded;
                    gun.reload();

                    acctualWeaponJacketAmmoText.text = gun.ammoInJacket.ToString();
                    acctualWeaponTotalAmmoText.text = gun.totalAmmo.ToString();
                    anim.SetTrigger("reload");
                }
            }


            // HEAL
            if (Input.GetKeyDown(KeyCode.H) &&
                healthPoints < 100 &&
                amountOfApteczkaInt > 0)
            {
                amountOfApteczkaInt--;
                int healing = (int) (0.3 * 100);
                int acctualHeal = Random.Range(20, healing);
                healthPoints += acctualHeal;
                if (healthPoints > 100)
                    healthPoints = 100;
                textHealthUI.text = healthPoints.ToString();
                fillageOfHpImage.fillAmount = healthPoints / 100;
            }

            //left diagonal m1t1
            if (moveAxis > 0 && turnAxis > 0)
            {
                moveSpeed = 0.9f;
                leftOrRight = 1.1f;
            }

            //left m0t1
            else if (moveAxis == 0 && turnAxis > 0)
            {
                moveSpeed = 1.1f;
                leftOrRight = 1.1f;
            }

            //right diagonal m1t-1
            else if (moveAxis > 0 && turnAxis < 0)
            {
                moveSpeed = 0.9f;
                leftOrRight = -1.1f;
            }

            //right m0t-1
            else if (moveAxis == 0 && turnAxis < 0)
            {
                moveSpeed = 1.1f;
                leftOrRight = -1.1f;
            }

            //run sprint forwart

            else if (moveAxis > 0 && turnAxis == 0)
            {
                moveSpeed = 1.1f;
                leftOrRight = 0f;

                if (shiftKey)
                {
                    moveSpeed = 2.5f;
                }
                
                if (inWater)
                {
                    moveSpeed = 0.5f;
                }
            }

            //run back 
            else if (moveAxis < 0 && turnAxis == 0)
            {
                moveSpeed = -1.1f;
                leftOrRight = 0f;
            }

            //stand still
            else
            {
                leftOrRight = 0f;
                moveSpeed = 0f;
            }
            ////////////////////////

            ///APPLY
            if (moveAxis < 0)
                turnAxis *= -1;

            ApplyInput(moveAxis, turnAxis);
            aimScript.SetRotation(mouseUpDown);
            anim.SetFloat("AimAngle", aimScript.GetAngle());

            anim.SetFloat("moveSpeed", moveSpeed);
            anim.SetFloat("leftOrRight", leftOrRight);

            transform.Rotate(0, mouseLefRight, 0);
        }
        else
        {
            if (sceneName.Equals("Cave"))
            {
                respawn();
            }
        }


        if (PlayerControllerTransform.killedOpponents == 3 && !areOppKilled)
        {
            areOppKilled = true;
            SceneManager.LoadScene("Cave");
        }

        StepSound();


        RenderHUD();
    }


    private void RenderHUD()
    {
        if (gun != null)
        {
            acctualWeaponJacketAmmoText.text = gun.ammoInJacket.ToString();
            acctualWeaponTotalAmmoText.text = gun.totalAmmo.ToString();
        }

        if (PlayerControllerTransform.healthPoints < 0)
        {
            PlayerControllerTransform.healthPoints = 0;
        }

        textHealthUI.text = PlayerControllerTransform.healthPoints.ToString();
        fillageOfHpImage.fillAmount = (float) healthPoints / 100;
        amountOfAidKitsText.text = PlayerControllerTransform.amountOfApteczkaInt.ToString();


        if (healthPoints > 66)
        {
            fillageOfHpImage.color = Color.green;
        }
        else if (healthPoints > 33)
        {
            fillageOfHpImage.color = Color.yellow;
        }
        else
        {
            fillageOfHpImage.color = Color.red;
        }
    }

    private void death()
    {
        anim.enabled = false;

        isDead = true;
        player.PlayOneShot(deathHit);
    }

    private void respawn()
    {
        anim.enabled = true;
        gun.getGun();
        anim.SetFloat("moveSpeed", 0);
        anim.SetFloat("leftOrRight", 0);
        anim.SetBool("is_dead 0", false);
        anim.SetBool("weaponOut", false);
        weapon_out = false;
        killedOpponents = 0;

        isDead = false;
        healthPoints = 35;
        anim.Play("HideWeapon");
    }

    private void ApplyInput(float moveInput,
        float turnInput)
    {
        Move(moveInput);
        Turn(turnInput);
    }

    private void Move(float input)
    {
        if (input > 0)
        {
            rb.AddForce(transform.forward * input * moveSpeed * 800, ForceMode.Acceleration);
        }

        if (input < 0)
        {
            rb.AddForce(transform.forward * -input * moveSpeed * 900, ForceMode.Acceleration);
        }

        transform.Translate(Vector3.forward * Mathf.Abs(input) * moveSpeed);        
    }

    private void Turn(float input)
    {
        if (input < 0)
        {
            rb.AddForce(transform.right * input * moveSpeed * 900, ForceMode.Acceleration);
        }

        if (input > 0)
        {
            rb.AddForce(transform.right * input * moveSpeed * 900, ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            rb.velocity = Vector3.zero;
            transform.Translate(Vector3.zero);
        }
        
        if (collision.gameObject.CompareTag("water"))
        {
            anim.SetBool("inWater", true);
            player.PlayOneShot(in_out_water);
            inWater = true;
            moveSpeed = 0.5f;
        }
        
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("water") && inWater && moveSpeed > 0)
        {
            WaterStepSound();
        }

    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("water"))
        {
            player.PlayOneShot(in_out_water);
            anim.SetBool("inWater", false);
            inWater = false;
        }
    }

    public void getHit(int damageAmount)
    {
        healthPoints -= damageAmount;

        if (healthPoints <= 0)
        {
            death();
        }
        else
        {
            anim.SetTrigger("hit");
            player.PlayOneShot(hit);
        }
    }

    void StepSound()
    {
        if (timerToStep > 0)
        {
            timerToStep -= Time.deltaTime * 1.5f;
        }

        if (timerToStep <= 0 && moveSpeed > 0 && !inWater)
        {
            player.PlayOneShot(step);
            timerToStep = stepTime;
        }
    }

    void WaterStepSound()
    {
        if (timerToStep > 0)
        {
            timerToStep -= Time.deltaTime * .01f;
        }

        if (timerToStep <= 0 && moveSpeed > 0)
        {
            player.PlayOneShot(inwater);
            timerToStep = waterStepTime;
        }
    }
}