using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUiMulti : MonoBehaviour
{
    public Canvas hudMenu;
    public Button btnStart;
    public Button btnExit;
    private Canvas manuUI;
    PlayerControllerTransform player;
    Gun gun;


    private void Start()
    {
        manuUI = (Canvas) GetComponent<Canvas>();
        hudMenu = hudMenu.GetComponent<Canvas>();
        // quitMenu = quitMenu.GetComponent<Canvas>();

        btnStart = btnStart.GetComponent<Button>(); //Ustawienie przycisku uruchomienia gry.
        btnExit = btnExit.GetComponent<Button>(); //Ustawienie przycisku wyjścia z gry.
        
        manuUI.enabled = false;
        hudMenu.enabled = true;
        Time.timeScale = 1;
        Cursor.visible = false;
    }
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            manuUI.enabled = !manuUI.enabled;
            hudMenu.enabled = !hudMenu.enabled;

            if (manuUI.enabled)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                btnStart.enabled = true;
                btnExit.enabled = true;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                // quitMenu.enabled = false; //Ukrycie menu pytania.
            }


            Cursor.visible = manuUI.enabled;
        }
    }
    
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
        Application.Quit(); //Powoduje wyjście z gry.
    }
    
    
    public void DoGroty()
    {
        manuUI.enabled = false;
        hudMenu.enabled = true;
        Time.timeScale = 1;
        GraczInstancja.startNr = "Respawn";
        Application.LoadLevel(
            "Cave"); //this will load our first level from our build settings. "1" is the second scene in our game	
    }
}
