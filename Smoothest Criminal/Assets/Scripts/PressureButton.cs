using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressureButton : MonoBehaviour
{
    public OnButtonPress buttonEvents;

    public bool oneShot = false;
    public bool pressed = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        pressed = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (!oneShot)
        {
            pressed = false;
        }
    }


}
[System.Serializable]
public class OnButtonPress : UnityEvent
{

}
