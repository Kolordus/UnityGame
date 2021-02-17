using UnityEngine.UI;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    // public Canvas quitMenu;
    public Canvas hudMenu;
    public Button btnStart;
    public Button btnExit;
    private Canvas manuUI;
    PlayerControllerTransform player;
    Gun gun;
    
    void Start()
    {
        player = (PlayerControllerTransform) GameObject.FindGameObjectWithTag("Player")
            .GetComponent<PlayerControllerTransform>();

        manuUI = (Canvas) GetComponent<Canvas>();
        hudMenu = hudMenu.GetComponent<Canvas>();
        // quitMenu = quitMenu.GetComponent<Canvas>();

        btnStart = btnStart.GetComponent<Button>(); //Ustawienie przycisku uruchomienia gry.
        btnExit = btnExit.GetComponent<Button>(); //Ustawienie przycisku wyjścia z gry.

        // quitMenu.enabled = false; //Ukrycie menu z pytaniem o wyjście z gry.
        manuUI.enabled = false;
        hudMenu.enabled = true;
        Time.timeScale = 1;
        Cursor.visible = manuUI.enabled;

        if (File.Exists(Application.persistentDataPath + "/save_game.deer"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/save_game.deer", FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            SaveGame save = (SaveGame) binaryFormatter.Deserialize(file);
            file.Close();

            if (player.weaponHolder.allGuns.Count > 0)
            {
                Gun M16 = player.weaponHolder.allGuns.Find(gun => gun.name.Equals("AR15"));
                Gun M1 = player.weaponHolder.allGuns.Find(gun => gun.name.Equals("M!GARAND"));
                Gun MK98 = player.weaponHolder.allGuns.Find(gun => gun.name.Equals("mk98"));

                M16.isAllowed = save.m16Available;
                M16.totalAmmo = save.m16TotalAmmo;
                M16.ammoInJacket = save.m16JacketAmmo;

                M1.isAllowed = save.m1Available;
                M1.totalAmmo = save.m1TotalAmmo;
                M1.ammoInJacket = save.m1JacketAmmo;

                MK98.isAllowed = save.m98Available;
                MK98.totalAmmo = save.m98TotalAmmo;
                MK98.ammoInJacket = save.m98JacketAmmo;
            }
            
            PlayerControllerTransform.amountOfApteczkaInt = save.aidKit;
            PlayerControllerTransform.healthPoints = save.health;
        }
        else
        {
            PlayerControllerTransform.healthPoints = 100;
            PlayerControllerTransform.amountOfApteczkaInt = 3;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            manuUI.enabled = !manuUI.enabled;
            hudMenu.enabled = !hudMenu.enabled;

            if (manuUI.enabled)
            {
                Cursor.lockState = CursorLockMode.Confined; //Odblokowanie kursora myszy.
                Cursor.visible = true; //Pokazanie kursora.
                Time.timeScale = 0; //Zatrzymanie czasu.
                // quitMenu.enabled = false; //Ukrycie menu pytania.
                btnStart.enabled = true; //Aktywacja przycsiku 'Start'.
                btnExit.enabled = true; //Aktywacja przycsiku 'Wyjście'.
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1; //Włączenie czasu.
                // quitMenu.enabled = false; //Ukrycie menu pytania.
            }


            Cursor.visible = manuUI.enabled;
        }
    }

    //Metoda wywoływana po naciśnięciu przycisku "Exit"
    public void PrzyciskWyjscie()
    {
        // quitMenu.enabled = true; //Uaktywnienie meny z pytaniem o wyjście
        btnStart.enabled = false; //Deaktywacja przycsiku 'Start'.
        btnExit.enabled = false; //Deaktywacja przycsiku 'Wyjście'.
    }

    //Metoda wywoływana podczas udzielenia odpowiedzi przeczącej na pytanie o wyjście z gry.
    public void PrzyciskNieWychodz()
    {
        // quitMenu.enabled = false; //Ukrycie menu z pytaniem o wyjście z gry.
        btnStart.enabled = true; //Uaktywnienie przycisku 'Start'.
        btnExit.enabled = true; //Uaktywnienie przycisku 'Wyjscie'.
    }

    //Metoda wywoływana przez przycisk uruchomienia gry 'Play Game'
    public void PrzyciskStart()
    {
        manuUI.enabled = false;
        hudMenu.enabled = true;

        Time.timeScale = 1;

        Cursor.visible = false; //Ukrycie kursora.
        Cursor.lockState = CursorLockMode.Locked; //Zablokowanie kursora myszy.
    }

    public void PrzyciskTakWyjdz()
    {
        SaveGame save = new SaveGame();
        Gun M16 = player.weaponHolder.allGuns.Find(gun => gun.name.Equals("AR15"));
        Gun M1 = player.weaponHolder.allGuns.Find(gun => gun.name.Equals("M!GARAND"));
        Gun MK98 = player.weaponHolder.allGuns.Find(gun => gun.name.Equals("mk98"));

        save.aidKit = PlayerControllerTransform.amountOfApteczkaInt;
        save.health = PlayerControllerTransform.healthPoints;

        save.m16Available = M16.isAllowed;
        save.m16TotalAmmo = M16.totalAmmo;
        save.m16JacketAmmo = M16.ammoInJacket;

        save.m1Available = M1.isAllowed;
        save.m1TotalAmmo = M1.totalAmmo;
        save.m1JacketAmmo = M1.ammoInJacket;

        save.m98Available = MK98.isAllowed;
        save.m98TotalAmmo = MK98.totalAmmo;
        save.m98JacketAmmo = MK98.ammoInJacket;
        
        System.IO.FileStream file = File.Create(Application.persistentDataPath + "/save_game.deer");
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(file, save);
        file.Close();
        Application.Quit(); //Powoduje wyjście z gry.
    }


    public void DoGroty()
    {
        manuUI.enabled = false;
        hudMenu.enabled = true;
        Time.timeScale = 1;
        GraczInstancja.startNr = "Respawn";
        SceneManager.LoadScene("Cave");
    }
}