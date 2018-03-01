using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy_2 extends the Enemy class
// Because Enemy_2 extends Enemy, the _____ bool won't work
// the same way in the Inspector pane
public class Enemy_2 : Enemy
{
    // # seconds for a full sine wave
    public float waveFrequency = 2f;

    // Sine wave width in meters
    public float waveWidth = 4f;
    public float waveRotY = 45f;

    // The initial x value of pos
    // Why doesn't this need an f?
    private float x0 = -12345f;
    private float birthTime;

    // Use this for initialization
    void Start ()
    {
        // Set x0 to the initial x position of Enemy_2
        // This works fine because the position will have already
        // been set by Main.SpawnEnemy() before Start() runs
        // (Awake() would be too early).
        // This is also good because there is no Start() method on Enemy.
        x0 = pos.x;
        birthTime = Time.time;

       
    }

    // Override the Move function on Enemy
    public override void Move()
    { 
      // Because pos is a property, you can't directly set pos.x
      // so get the pos as an editable Vector3
        Vector3 tempPos = pos;

        // Theta adjusts based on time
        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        pos = tempPos;

        // Rotate a bit about y
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);

        // Base.Move() still handles the movement down in y
        base.Move();
    }

    // Update is called once per frame
    void Update ()
    {
        Move();

        if (remainingDamageFrames > 0)
        {
            remainingDamageFrames--;
            if (remainingDamageFrames == 0)
            {
                base.UnShowDamage();
            }
        }
    }
}
