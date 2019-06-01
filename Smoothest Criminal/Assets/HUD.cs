using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text levelTitle;
    public Text ammo;
    public Text bombs;

    public GameObject swordIcon;
    public GameObject gunIcon;
    public GameObject bombIcon;

    public GameObject deadText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ammo.text = "x " + LevelManager.instance.ammo;

        ammo.gameObject.SetActive(LevelManager.instance.maxAmmo > 0);
        gunIcon.SetActive(LevelManager.instance.maxAmmo > 0);

        bombs.text = "x " + LevelManager.instance.bombs;

        bombs.gameObject.SetActive(LevelManager.instance.maxBombs > 0);
        bombIcon.SetActive(LevelManager.instance.maxBombs > 0);

        swordIcon.SetActive(LevelManager.instance.maxSword > 0);

        levelTitle.text = LevelManager.instance.title;
    }
}
