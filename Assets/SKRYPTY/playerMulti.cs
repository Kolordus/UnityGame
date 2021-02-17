using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class playerMulti : NetworkBehaviour, HeroAbstract
{
    private string MoveInputAxis = "Vertical";
    private string TurnInputAxis = "Horizontal";
    private float horizontalSpeed = 3.0F;
    private float verticalSpeed = 3.0F;
    private float moveSpeed = 0f;
    private float leftOrRight = 0f;
    private Rigidbody rb;
    private Animator anim;
    private CapsuleCollider boxCollider;
    public AimScript aimScript;

    public Text acctualWeaponJacketAmmoText;
    public Text acctualWeaponTotalAmmoText;
    public Text textHealthUI;
    public Image fillageOfHpImage;
    public Text amountOfAidKitsText;

    public GameObject playerCamera;
    public GameObject playerHUD;
    public Transform bulletSpawnPoint;
    public GameObject bullet;
    [SyncVar] [SerializeField] public int healthPoints = 100;
    [SyncVar] [SerializeField] public int amountOfApteczkaInt = 1;
    public int spawnPoint = 1;

    [SyncVar] [SerializeField] public int livesLeft = 10;

    [SyncVar] [SerializeField] public bool inWater = false;

    public GameObject[] resourceSpawnPoints;

    public AudioSource player;
    public AudioClip hit;
    public AudioClip deathHit;
    public AudioClip step;
    public AudioClip inwater;
    public AudioClip in_out_water;
    public float nextTimeToFire = 0f;

    public float timerToStep = 0f;
    public float stepTime = 0.5f;
    public float waterStepTime = 3f;

    public Gun gun;
    // public WeaponHolder weaponHolder;

    public int ammoInJacket;

    public int totalAmmo;
    public int jacketCapacity;

    [SyncVar] [SerializeField] public int lifesLeft;


    public GameObject canvasPrompt;


    private void Start()
    {
        resourceSpawnPoints = GameObject.FindGameObjectsWithTag("resourceSpawnPoint");
        if (!isLocalPlayer || !hasAuthority)
        {
            spawnPoint = Random.Range(1, 6);
            playerCamera.SetActive(false);
            playerHUD.SetActive(false);
        }
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        playerCamera.SetActive(true);
        playerHUD.SetActive(true);
        ammoInJacket = 30;
        jacketCapacity = 30;
        totalAmmo = 300;
        boxCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        lifesLeft = 10;
        canvasPrompt.GetComponent<Canvas>().enabled = false;
    }

    private void FixedUpdate()
    {
        if (livesLeft < 1)
        {
            Destroy(this);
        }

        if (!isLocalPlayer)
        {
            return;
        }

        if (lifesLeft <= 0)
        {
            return;
        }

        float moveAxis = Input.GetAxis(MoveInputAxis);
        float turnAxis = Input.GetAxis(TurnInputAxis);
        bool shiftKey = Input.GetKey(KeyCode.LeftShift);
        bool getMouseButton = Input.GetMouseButtonDown(0);
        float mouseLefRight = horizontalSpeed * Input.GetAxis("Mouse X");
        float mouseUpDown = horizontalSpeed * Input.GetAxis("Mouse Y");

        if (getMouseButton && ammoInJacket > 0)
        {
            print(("hako kurwa player???"));
            nextTimeToFire = Time.time + 1f / gun.fireRate;
            anim.SetTrigger("weaponFire");
            Shoot();
        }

        if (getMouseButton == false)
            anim.SetBool("weaponFire", false);


        // heal
        if (Input.GetKeyDown(KeyCode.H) &&
            healthPoints < 100 &&
            amountOfApteczkaInt > 0)
        {
            amountOfApteczkaInt--;
            int healing = (int) (0.3 * 100);
            int acctualHeal = Random.Range(20, healing);
            healthPoints += acctualHeal;

            if (healthPoints > 100) healthPoints = 100;

            textHealthUI.text = healthPoints.ToString();
            fillageOfHpImage.fillAmount = healthPoints / 100;
        }


        // RELOAD
        if (Input.GetKeyDown(KeyCode.R) && ammoInJacket < jacketCapacity)
        {
            ///////
            int actualAmmo = ammoInJacket;
            int ammoNedded = jacketCapacity - actualAmmo;

            if (ammoNedded > 0 && totalAmmo >= ammoNedded)
            {
                ammoInJacket += ammoNedded;
                totalAmmo -= ammoNedded;
                gun.reload();

                acctualWeaponJacketAmmoText.text = ammoInJacket.ToString();
                acctualWeaponTotalAmmoText.text = totalAmmo.ToString();
                anim.SetTrigger("reload");
            }
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

        StepSound();

        RenderHUD();
    }

    private void ApplyInput(float moveInput, float turnInput)
    {
        Move(moveInput);
        Turn(turnInput);
    }

    private void Move(float input)
    {
        if (input > 0)
        {
            rb.AddForce(transform.forward * input * moveSpeed * 1400, ForceMode.Acceleration);
        }

        if (input < 0)
        {
            rb.AddForce(transform.forward * -input * moveSpeed * 900, ForceMode.Acceleration);
        }

        //transform.Translate(Vector3.forward * Mathf.Abs(input) * moveSpeed);        
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

    private void RenderHUD()
    {
        if (!isLocalPlayer || !hasAuthority)
        {
            return;
        }

        if (gun != null)
        {
            acctualWeaponJacketAmmoText.text = ammoInJacket.ToString();
            acctualWeaponTotalAmmoText.text = totalAmmo.ToString();
        }

        if (healthPoints < 0)
        {
            healthPoints = 0;
        }

        textHealthUI.text = healthPoints.ToString();
        fillageOfHpImage.fillAmount = (float) healthPoints / 100;
        amountOfAidKitsText.text = amountOfApteczkaInt.ToString();


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

    private void Shoot()
    {
        this.ammoInJacket--;
        gun.takeShot();
        CmdShoot();
    }

    [ClientRpc]
    private void RpcShoot()
    {
        GameObject _bullet =
            (GameObject) Instantiate(bullet, bulletSpawnPoint.transform.position, Quaternion.identity);

        Bullet __bullet = _bullet.GetComponent<Bullet>();

        _bullet.transform.rotation = bulletSpawnPoint.transform.rotation;
        NetworkServer.Spawn(__bullet.gameObject);
        this.ammoInJacket--;
        gun.takeShot();
    }

    [Command]
    private void CmdShoot()
    {
        GameObject _bullet =
            (GameObject) Instantiate(bullet, bulletSpawnPoint.transform.position, Quaternion.identity);

        Bullet __bullet = _bullet.GetComponent<Bullet>();

        _bullet.transform.rotation = bulletSpawnPoint.transform.rotation;
        NetworkServer.Spawn(__bullet.gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!isLocalPlayer)
        {
            return;
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

    public void GiveResources(int aidKit, int ammo)
    {
        if (isServer)
        {
            RpcGiveResources(aidKit, ammo);
        }
        else
        {
            CmdGiveResources(aidKit, ammo);
        }
    }

    [Command]
    public void CmdGiveResources(int aidKit, int ammo)
    {
        GiveResources(aidKit, ammo);
    }

    [ClientRpc]
    public void RpcGiveResources(int aidKit, int ammo)
    {
        amountOfApteczkaInt += aidKit;
        totalAmmo += ammo;
    }

    public void getHit(int damageAmount)
    {
        if (isServer)
        {
            RpcGetHit(damageAmount);
        }
        else
        {
            CmdGetHit(damageAmount);
        }
    }

    [Command]
    public void CmdGetHit(int damageAmount)
    {
        getHit(damageAmount);
    }

    [ClientRpc]
    public void RpcGetHit(int damageAmount)
    {
        healthPoints -= damageAmount;
        if (healthPoints <= 0)
        {
            death();
        }

        if (isServer)
        {
            anim.SetTrigger("hit");
            player.PlayOneShot(hit);
        }
    }

    private void PlayerRespawn()
    {
        if (isServer)
        {
            RpcPlayerRespawn();
        }
        else
        {
            CmdPlayerRespawn();
        }
    }

    [Command]
    private void CmdPlayerRespawn()
    {
        PlayerRespawn();
    }

    [ClientRpc]
    private void RpcPlayerRespawn()
    {
        healthPoints = 100;
        spawnPoint = Random.Range(0, resourceSpawnPoints.Length);
        this.transform.position = resourceSpawnPoints[spawnPoint].transform.position;
        livesLeft--;
    }

    private void death()
    {
        player.PlayOneShot(deathHit);
        PlayerRespawn();
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

    [Command]
    void CmdSyncHp(int damageAmount)
    {
        healthPoints -= damageAmount;
    }
}