using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public bool on = true;
    public Animator anim;

    public bool toggle = true;
    public float timeOn = 1;
    public float timeOff = 1;

    float elapsed = 0;

    // Start is called before the first frame update
    void Start()
    {
        /*if (toggle)
        {

            TimeToggle();
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("on", on);

        if (!toggle)
            return;

        elapsed += GameManager.instance.speed * Time.deltaTime;

        if(on && elapsed > timeOn)
        {
            Toggle();
            elapsed = 0;
        }

        else if (!on && elapsed > timeOff)
        {
            Toggle();
            elapsed = 0;
        }
    }

    public void Toggle()
    {
        on = !on;
        //TimeToggle();
    }

    void TimeToggle()
    {
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
}
