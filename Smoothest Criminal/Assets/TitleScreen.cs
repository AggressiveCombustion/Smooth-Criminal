using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Timer t = new Timer(0.5f, FindObjectOfType<SceneTransitions>().Begin);
        GameManager.instance.AddTimer(t, gameObject);

        Timer t2 = new Timer(2.5f, FindObjectOfType<SceneTransitions>().End);
        GameManager.instance.AddTimer(t2, gameObject);

        Timer t3 = new Timer(3.5f, FindObjectOfType<SceneTransitions>().Begin);
        GameManager.instance.AddTimer(t3, gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
