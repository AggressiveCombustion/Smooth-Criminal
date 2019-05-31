using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public bool hasGoal = false;

    public int maxActions = 1;

    public int maxAmmo = 0;
    public int maxSword = 0;
    public int maxBombs = 0;

    public int ammo;
    public int sword;
    public int bombs;
    public int actions;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
        
        Restart();

        if(FindObjectOfType<SceneTransitions>() != null)
            FindObjectOfType<SceneTransitions>().Begin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        ammo = maxAmmo;
        sword = maxSword;
        bombs = maxBombs;
        actions = maxActions;

        hasGoal = false;
    }

    public void EndLevel()
    {
        FindObjectOfType<SceneTransitions>().End();
    }
}
