using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public int index = 1;

    public int numSpawned = 0;
    public int maxSpawned = 2;

    //
    public bool active = false;
    public float timeBetweenSpawns = 1.0f;

    public enum SpawnerType
    {
        alternate,
        chosen
    }
    public SpawnerType type = SpawnerType.alternate;
    public List<bool> types = new List<bool>();

    bool hasSpawned = false;

    bool startingActive = false;

    // Start is called before the first frame update
    void Start()
    {
        startingActive = active;
    }

    public void Restart()
    {
        active = startingActive;
        numSpawned = 0;
        hasSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && !hasSpawned)
        {
            int originalCount = types.Count;
            if (type == SpawnerType.chosen)
            {
                if (maxSpawned > types.Count)
                {
                    for (int i = 0; i < maxSpawned - originalCount; i++)
                    {
                        types.Add(false);
                    }
                }
            }

            Spawn();
        }
    }

    public void Spawn()
    {
        if (numSpawned >= maxSpawned)
            return;

        

        hasSpawned = true;
        Enemy e = Instantiate(enemy, transform.position, Quaternion.Euler(Vector3.zero)).GetComponent<Enemy>();

        e.facing = FindObjectOfType<PlayerController>().transform.position.x < e.transform.position.x ? -1 : 1;
        e.patrol = false;
        e.spawned = true;
        
        switch(type)
        {
            case SpawnerType.alternate:
                e.ranged = index == 0 ? true : false;
                index++;
                if (index > 1)
                    index = 0;

                numSpawned += 1;
                break;

            case SpawnerType.chosen:
                e.ranged = types[numSpawned];
                
                numSpawned += 1;
                break;
        }

        if (e.ranged)
            e.attackDistance = 30;
        else
            e.attackDistance = 1;

        // alternate between ranged and melee
        Timer t = new Timer(timeBetweenSpawns, Spawn);
        GameManager.instance.AddTimer(t, gameObject);
    }
}
