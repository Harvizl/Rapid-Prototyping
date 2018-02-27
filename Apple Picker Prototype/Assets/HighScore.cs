using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour {

    static public int score = 0;
    
	// Use this for initialization
	void Start () {
		
	}

    void Awake()
    {
        // If the ApplePickerHighScore already exists, read it
        if (PlayerPrefs.HasKey ("ApplePickerHighScore"))
        {
            score = PlayerPrefs.GetInt("ApplePickerHighScore");
        }
        // Assign the High Score to ApplePickerHighScore
        PlayerPrefs.SetInt("ApplePickerHigherScore", score);
    }
	
	// Update is called once per frame
	void Update () {
        GUIText gt = this.GetComponent<GUIText>();
        gt.text = "High Score: " + score;
        // Update ApplePickerHighScore in PlayerPrefs if necessary
        if (score > PlayerPrefs.GetInt("ApplePickerHighScore"))
        {
            PlayerPrefs.SetInt("ApplePickerHighScore", score);
        }
	}
}
