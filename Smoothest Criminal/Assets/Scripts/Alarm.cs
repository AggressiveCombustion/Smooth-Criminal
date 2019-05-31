using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    public bool damaged = false;
    public bool triggered = false;

    Animator anim;

    public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Restart()
    {
        damaged = false;
        triggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("triggered", triggered);

        GetComponent<SpriteRenderer>().sprite = damaged ? sprites[1] : sprites[0];

        if (damaged)
            triggered = false;
    }

    public void Trigger()
    {
        if (damaged)
            return;

        triggered = true;

        Timer t = new Timer(1.5f, Trigger);
        GameManager.instance.AddTimer(t, gameObject);

        foreach (Enemy e in FindObjectsOfType<Enemy>())
        {
            if (!e.pursue && !e.suspicious && FindObjectOfType<PlayerController>().state != PlayerController.PlayerState.death)
            {
                if (e.transform.position.y < transform.position.y + 20 &&
                    e.transform.position.y > transform.position.y - 20)
                {
                    if (Vector2.Distance(e.transform.position, transform.position) < 200)
                    {
                        e.suspicious = true;
                        e.suspiciousPoint = FindObjectOfType<PlayerController>().transform.position;

                        Instantiate(e.question, e.transform.position + new Vector3(0, 1.5f), Quaternion.Euler(0, 0, 0));
                    }

                }
            }
        }

        foreach(EnemySpawner es in FindObjectsOfType<EnemySpawner>())
        {
            es.active = true;
        }
    }
}
