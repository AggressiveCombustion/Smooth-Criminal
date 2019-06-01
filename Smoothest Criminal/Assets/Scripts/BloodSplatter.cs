﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : MonoBehaviour
{
    public Sprite[] splatters;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = splatters[Random.Range(0, splatters.Length - 1)];
        if (SfxManager.instance != null)
            SfxManager.instance.PlaySFX(SfxManager.instance.blood);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
}
