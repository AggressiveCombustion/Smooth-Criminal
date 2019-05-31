using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    LineRenderer lr;
    public Transform startPoint;
    Vector3 endPoint;

    public float maxDistance = 100.0f;
    
    //public OnBeatEvent onBeat;

    public float speed = 0;

    public bool toggle = false;
    public float limit = 0;
    public float elapsed = 0;
    

    public bool on = true;
    
    public float timeOn = 1;
    public float timeOff = 1;

    public float delay = 0.0f;

    bool started = false;
    bool startToggle;
    bool startOn;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        startToggle = toggle;
        startOn = on;
        //conductor = FindObjectOfType<Conductor>();
        //conductor.onBeat.AddListener(onBeat.Invoke);
        Timer tn = new Timer(delay, Toggle);
        GameManager.instance.AddTimer(tn, gameObject);

    }

    public void Restart()
    {
        on = startOn;
        started = false;
        elapsed = 0;
        toggle = startToggle;

        Timer tn = new Timer(delay, Toggle);
        GameManager.instance.AddTimer(tn, gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (on)
        {
            lr.enabled = true;
            RaycastHit2D hit;
            hit = Physics2D.Raycast(startPoint.position, -transform.up, maxDistance);
            if (hit)
            {
                endPoint = hit.point;
                if (hit.transform.tag == "Player")
                {
                    // hit player
                    Alert();
                }
            }
            else
            {
                endPoint = startPoint.position - transform.up * maxDistance;
            }

            lr.SetPosition(0, startPoint.position);
            lr.SetPosition(1, endPoint);
        }
        else
        {
            lr.enabled = false;
        }
        
    }

    public void Rotate()
    {
        transform.Rotate(Vector3.forward, speed);
        elapsed += Mathf.Abs(speed);
    }

    public void Toggle()
    {
        if (!toggle)
            return;

        on = !on;

        if (on)
        {
            Timer tn = new Timer(timeOn, Toggle);
            GameManager.instance.AddTimer(tn, gameObject);
        }

        else
        {
            Timer tf = new Timer(timeOff, Toggle);
            GameManager.instance.AddTimer(tf, gameObject);
        }
    }

    public void Alert()
    {
        foreach(Laser l in FindObjectsOfType<Laser>())
        {
            l.on = false;
            l.toggle = false;
        }

        FindObjectOfType<Alarm>().Trigger();
    }
    
}
