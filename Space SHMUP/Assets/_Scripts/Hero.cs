using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{

    // Singleton
    static public Hero S;

    public float gameRestartDelay = 2f;

    // Movement controls
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;

    // Ship status information
    [SerializeField]
    private float _shieldLevel = 1;

    // Weapon fields
    public Weapon[] weapons;

    public bool _;

    public Bounds bounds;

    // Declare a new delegate type WeaponFireDelegate
    public delegate void WeaponFireDelegate();

    // Create a WeaponFireDelegate field named fireDelegate.
    public WeaponFireDelegate fireDelegate;

    // This variable holds a reference to the last triggering GameObject
    public GameObject lastTriggerGo = null;

    void Awake()
    {
        // Sets the Singleton
        S = this;
        bounds = Utils.CombineBoundsOfChildren(this.gameObject);

        // Reset the weapons to start _Hero with 1 blaster
        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // pull information from the input class
        float yAxis = Input.GetAxis("Vertical");
        float xAxis = Input.GetAxis("Horizontal");

        // Change transform.position based on the axes
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        bounds.center = transform.position;

        // Keep the ship constrained to the Screen Bounds
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.onScreen);
        if (off != Vector3.zero)
        {
            pos -= off;
            transform.position = pos;
        }

        // Rorate the ship to make it feel more dynamic
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        // Use the fireDelegate to fire Weapons
        // First, make sure the Axis("Jump") button is pressed
        // Then ensure that fireDelegate isn't null to avoid an error
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Find the tag of other.gameObject or its parent GameObjects
        GameObject go = Utils.FindTaggedParent(other.gameObject);

        // Make sure it's not the same triggering go as last time
        if (go == lastTriggerGo)
        {
            return;
        }
        lastTriggerGo = go;
        if (go.tag == "Enemy")
        {
            // If the Shield was triggered by an Enemy
            // Decrease the level of the Shield by 1
            shieldLevel--;

            // Destroy the enemy
            Destroy(go);
            print("Triggered: " + go.name);
        }

        else if (go.tag == "PowerUp")
        {
            // If the shield was triggerd by a PowerUp
            AbsorbPowerUp(go);
        }

        else
        {
            // Otherwise announce the original other.gameObject
            print("Triggered: " + other.gameObject.name);
        }
    }

    // When a PowerUp is absorbed
    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {  
            // If it's the shield
            case WeaponType.shield:
            shieldLevel++;
            break;

            // If it's any Weapon PowerUp
            default:

            // Check the current weapon type
            if (pu.type == weapons[0].type)
                {
                    // then increase the number of weapons of this type
                    // Find an available weapon
                    Weapon w = GetEmptyWeaponSlot(); 

                    if (w != null)
                    {
                        // Set it to pu.type
                        w.SetType(pu.type);
                    }
                }
                else
                {
                    // If this is a different weapon
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;
        }
        pu.AbsorbedBy(this.gameObject);
    }

    // 
    Weapon GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].type == WeaponType.none)
            {
                return (weapons[i]);
            }
        }
        return (null);
    }

    // Clears PowerUps and resets to 1 Blaster
    void ClearWeapons()
    {
        foreach (Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
        }
    }

    // Game to identify what the current Shield level
    // and regulates values
    public float shieldLevel
    {
        // Gets current Shield level
        get
        {
            return (_shieldLevel);
        }

        // Sets restrictions
        set
        {
            _shieldLevel = Mathf.Min(value, 4);

            // If the Shield is going to be set to less than zero
            if (value < 0)
            {
                Destroy(this.gameObject);

                // Tell Main.S to restart the game after a delay
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }
}
