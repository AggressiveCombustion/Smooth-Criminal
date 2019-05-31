using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public GameObject blast;
    public bool splode = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (splode)
            Explode();
    }

    public void Explode()
    {
        Instantiate(blast, transform.position, Quaternion.Euler(Vector3.zero));
        // alert others
        foreach (Enemy e in FindObjectsOfType<Enemy>())
        {
            if (e != this)
            {
                if (e.transform.position.y < transform.position.y + 2 &&
                    e.transform.position.y > transform.position.y - 2)
                {
                    if (Vector2.Distance(e.transform.position, transform.position) < 20)
                    {
                        e.suspicious = true;
                        e.suspiciousPoint = transform.position;

                        Instantiate(e.question, e.transform.position + new Vector3(0, 1.5f), Quaternion.Euler(0, 0, 0));
                    }

                }
            }
        }
        Destroy(gameObject);
    }
}
