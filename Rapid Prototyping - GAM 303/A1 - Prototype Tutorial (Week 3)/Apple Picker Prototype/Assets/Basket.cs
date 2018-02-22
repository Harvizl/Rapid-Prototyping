using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basket : MonoBehaviour {

    public GUIText timerGT;
    public GUIText scoreGT;
    public float timer = 30f;
    
    // Use this for initialization
    void Start () {
        // Find a reference to the ScoreCounter GameObject
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        // Get the GUIText Component of that GameObject
        scoreGT = scoreGO.GetComponent<GUIText>();
        // Set the starting number of points to 0
        scoreGT.text = "0";
        // Find a reference to the ScoreCounter GameObject
        GameObject timerGO = GameObject.Find("Timer");
        // Get the GUIText Compoinent of that Game Object
        timerGT = timerGO.GetComponent<GUIText>();
        timerGT.text = "30";
        
	}

    // Update is called once per frame
    void Update()
    {
        // Counts down Timer GUI
        timer -= Time.deltaTime;
       
            timerGT.text = timer.ToString("##");
        
            if (timer < 0)
        {
            Time.timeScale = 0;
        }

        // Get the current screen position of the mouse from Input
        Vector3 mousePos2D = Input.mousePosition;
        // The Camera's Z position sets the 'how far to push' the mouse into 3D
        mousePos2D.z = -Camera.main.transform.position.z;
        // Convert the point from 2D screen space into 3D game world space
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        // Move the X position of this Basket to the X position of the Mouse
        Vector3 pos = this.transform.position;
        pos.x = mousePos3D.x;
        this.transform.position = pos;
    }

    void FixedUpdate()
    {
        
    }

    void OnCollisionEnter(Collision coll)
    {
        // Find out what hit this basket, destroys Apple
        GameObject collidedWith = coll.gameObject;
        if (collidedWith.tag == "Apple")
        {
            Destroy(collidedWith);

            // Parse the text of the scoreGT into an int
            int score = int.Parse(scoreGT.text);

            // Add points for catching an Apple
            score += 100;
            Debug.Log(score);

            // Convert the score back to a string and display it
            scoreGT.text = score.ToString();

            // Track the High Score
            if (score > HighScore.score)
            {
                HighScore.score = score;
            }
        }
        
    }
}
