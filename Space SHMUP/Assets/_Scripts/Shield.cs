﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    public float rotationPerSecond = 0.1f;
    public bool _;
    public int levelShown = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // Read the current Shield level from the Hero Singleton
        int currLevel = Mathf.FloorToInt(Hero.S.shieldLevel);

        // If this is different from levelShown...
        if (levelShown != currLevel)
        {
            levelShown = currLevel;
            Material mat = this.GetComponent<Renderer>().material;

            // Adjust the texture offset to show difference Shield levels
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);            
        }
        // Rotate the Shield a bit every second
        float rZ = (rotationPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
	}
}
