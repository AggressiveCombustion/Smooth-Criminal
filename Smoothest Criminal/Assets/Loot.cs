using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public GameObject indicator;
    public GameObject sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sprite.SetActive(!LevelManager.instance.hasGoal);
        indicator.SetActive(!LevelManager.instance.hasGoal);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (!LevelManager.instance.hasGoal)
            {
                LevelManager.instance.hasGoal = true;
            }
        }
    }
}
