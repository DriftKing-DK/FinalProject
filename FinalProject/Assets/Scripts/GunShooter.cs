using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GunShooter : MonoBehaviour
{
    public GameObject[] gunList;
    public Camera cam;
    public Slider AmmoLeft;
    public Slider TimeSlider;
    
    //private GameObject myGun;
    private Gun myGun;

    private bool timerNeedUpdate;
    private int gunAction = 0;

    private int currentGun = 0;
    private int nbGuns;

    void Start()
    {
        nbGuns = gunList.Length;
        for (int i = 0; i < nbGuns; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<Gun>().Setup(cam);
            gunList[i].SetActive(false);
        }

        UpdateGunProperties();
        UISetup();
    }

    private void Update()
    {
        ChangeGun(Input.GetKeyDown(KeyCode.N), Input.GetKeyDown(KeyCode.P));

        gunAction = myGun.ManageInput();
        UIManagement();
    }

    private void UISetup(bool reload = false, bool load = false)
    {
        if (reload) // reloading setup
        {
            //set red color
            Color color = new Color(233f/255f, 79f/255f, 55f/255f);
            TimeSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = color;

            timerNeedUpdate = true;
            TimeSlider.maxValue = myGun.reloadTime;
            TimeSlider.value = myGun.reloadTime;
        }
        else if (load) //loading setup
        {
            timerNeedUpdate = true;
            TimeSlider.value = myGun.timeBetweenShooting;
            AmmoLeft.value = myGun.GetBulletLeft();
        }
        else // init setup
        {
            //AmmoLeft slider
            AmmoLeft.maxValue = myGun.magazineSize;
            AmmoLeft.minValue = 0;
            AmmoLeft.value = myGun.GetBulletLeft();

            //TimeSlider

            // set white color
            Color color = new Color(255f/255f, 255f/255f, 255f/255f);
            TimeSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = color;

            timerNeedUpdate = false;
            TimeSlider.maxValue = myGun.timeBetweenShooting;
            TimeSlider.minValue = 0;
            TimeSlider.value = 0;
        }
    }

    private void UIManagement()
    {
        if (gunAction == 1) // reload
        {
            UISetup(reload : true);
        }
        else if (gunAction == 2)
        {
            UISetup(load : true);
        }

        if (timerNeedUpdate)
        {
            if (TimeSlider.value == 0)
            {
                timerNeedUpdate = false;
                UISetup();
            }
            else
            {
                TimeSlider.value -= Time.deltaTime;
            }
        }
    }

    private void UpdateGunProperties()
    {
        gunList[currentGun].SetActive(true);
        myGun = transform.GetChild(currentGun).gameObject.GetComponent<Gun>();
        AmmoLeft.maxValue = myGun.magazineSize;
        AmmoLeft.value = myGun.GetBulletLeft();
        UISetup();
    }

    private void ChangeGun(bool n, bool p)
    {
        if (n != p && myGun.CanChangeGun())
        {
            gunList[currentGun].SetActive(false);
            if (n == true) // next gun
            {
                currentGun += 1;
                if (currentGun >= nbGuns)
                    currentGun = 0;
            }
            else // previous gun
            {
                currentGun -= 1;
                if (currentGun < 0)
                    currentGun = nbGuns - 1;
            }
            UpdateGunProperties();
        }
    }
}
