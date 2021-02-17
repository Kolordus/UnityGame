using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public int selectedWeapon = 0;
    public Gun selectedGun;
    public List<Gun> allGuns = new List<Gun>();

    private void Start()
    {
        SelectWeapon();
    }

    public void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            allGuns.Add(weapon.GetChild(0).gameObject.GetComponent<Gun>());
            if (i == selectedWeapon)
            {
                if (weapon.GetChild(0).gameObject.GetComponent<Gun>().isAllowed)
                {
                    weapon.gameObject.SetActive(true);
                    selectedGun = weapon.GetChild(0).gameObject.GetComponent<Gun>();
                    
                }
            }
                
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
