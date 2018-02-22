using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour {

    // Prefab for instantiating Apples
    public GameObject applePrefab;

    // Speed at which the AppleTree moves in meters/second
    public float speed = 1f;

    // Distance where AppleTree turns around
    public float leftAndRightEdge = 10f;

    // Chance that the AppleTree will change directions
    public float chanceToChangeDirections = 0.1f;

    // Rate at which Apples will be instantiated
    public float secondsBetweenAppleDrops = 1f;

    // Use this for initialization
    void Start () {
        // Dropping Apples every second
        InvokeRepeating("DropApple", 2f, secondsBetweenAppleDrops);
	}

    void DropApple()
    {
        GameObject apple = Instantiate(applePrefab) as GameObject;
        apple.transform.position = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        // Basic movement
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;
        // Changing direction
        if (pos.x < -leftAndRightEdge)
        {
            // Move right
            speed = Mathf.Abs(speed);
        }
        else if (pos.x > leftAndRightEdge)
        {
            // Move left
            speed = -Mathf.Abs(speed);
        }
	}

    void FixedUpdate()
    {
        // Changing direction randomly
        if (Random.value < chanceToChangeDirections)
        {
            speed *= -1;
        }
    }
}
