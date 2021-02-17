using System;
using UnityEngine;
using UnityEngine.UI;

public class ChooseWeapon : MonoBehaviour
{
    public string prompt;
    private GameObject canvasPrompt;
    private Canvas canvas;
    private PlayerControllerTransform player;
    private string text;

    // Start is called before the first frame update
    void Start()
    {
        canvasPrompt = GameObject.FindGameObjectWithTag("Prompt");
        canvas = canvasPrompt.GetComponent<Canvas>();
        canvas.enabled = false;

    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text = "";
            player = other.GetComponent<PlayerControllerTransform>();
            canvas.enabled = true;

            int i = 1;
            foreach (Gun gun in player.weaponHolder.allGuns)
            {
                if (gun.isAllowed)
                    text += gun.name + " --- press " + i + " to equip\n";
                i++;
            }

            setPrompt(text);
        }
    }

   

    private void OnTriggerStay(Collider other)
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1) && player.weaponHolder.allGuns[0].isAllowed)
        {
            player.weaponHolder.selectedWeapon = 0;
            player.weaponHolder.SelectWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && player.weaponHolder.allGuns[1].isAllowed)
        {
            player.weaponHolder.selectedWeapon = 1;
            player.weaponHolder.SelectWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && player.weaponHolder.allGuns[2].isAllowed)
        {
            player.weaponHolder.selectedWeapon = 2;
            player.weaponHolder.SelectWeapon();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            setPrompt("");
        }

        canvas.enabled = false;
    }

    private void setPrompt(String text)
    {
        GameObject.FindGameObjectWithTag("enterPrompt").GetComponent<Transform>().GetComponentInChildren<Text>().text = "";

        canvasPrompt.GetComponent<Transform>().GetComponentInChildren<Text>().text = text;
    }

}
