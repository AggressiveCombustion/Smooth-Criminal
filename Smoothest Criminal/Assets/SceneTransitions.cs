using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitions : MonoBehaviour
{
    public Animator anim;

    public AudioClip swoosh;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Begin()
    {
        anim.SetTrigger("begin");
    }

    public void End()
    {
        anim.SetTrigger("end");
        DoSwoosh();
    }

    public void DoSwoosh()
    {
        if (SfxManager.instance != null)
            SfxManager.instance.PlaySFX(swoosh);
    }
}
