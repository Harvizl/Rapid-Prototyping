using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an enum of the various possible weapon types
// It also includes a "Shield" type to allow a Shield Power Up
public enum WeaponType
{
    // The default / no weapon
    none,
    // A simple blaster
    blaster,
    // Two shots simultaneously
    spread,
    // Shots that move in waves
    phaser,
    // Homing missiles
    missile,
    // Damage over time
    laser,
    // Raise shieldLevel
    shield
}

// The WeaponDefinition class allows you to set the properties
// of a specific weapon in the Inspector. Main has an array
// of WeaponDefinitions that makes this possible.

// [System.Serializable] tells Unity to try to view WeaponDefinition
// in the Inspector pane. It doesn't work for everything, but it
// will work for simple classes like this
[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;

    // The letter to show on the power-up
    public string letter;

    // Color of Collar & power-up
    public Color color = Color.white;

    // Prefab for projectiles
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;

    // Amount of damage caused
    public float damageOnHit = 0;

    // Damage per second (Laser)
    public float continuousDamage = 0; 
    public float delayBetweenShots = 0;

    // Speed of projectiles
    public float velocity = 20; 
}

public class Weapon : MonoBehaviour {

    static public Transform PROJECTILE_ANCHOR;
    public bool ____________________;
    [SerializeField]
    private WeaponType _type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;

    // Time last shot was fired
    public float lastShot;

    void Awake()
    {
        collar = transform.Find("Collar").gameObject;
    }

    // Use this for initialization
    void Start()
    {
        // Call SetType() properly for the default _type
        SetType(_type);

        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_Projectile_Anchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        // Find the fireDelegate of the parent
        GameObject parentGO = transform.parent.gameObject;

        if (parentGO.tag == "Hero")
        {
            Hero.S.fireDelegate += Fire;
        }
    }

    // Why isn't this working?!
    public WeaponType type
    {
        get { return (_type); }
        set { SetType(value); }
    }

    public void SetType(WeaponType wt)
    {
        _type = wt;
        if (type == WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }

        def = Main.GetWeaponDefinition(_type);
        collar.GetComponent<Renderer>().material.color = def.color;

        // You can always fire immediately after _type is set
        lastShot = 0;
    }
    public void Fire()
    {
        // If this.gameObject is inactive, return
        if (!gameObject.activeInHierarchy) return;
        
        // If it hasn't been enough time between shots, return
        if (Time.time - lastShot < def.delayBetweenShots)
        {
            return;
        }
        Projectile p;
        switch (type)
        {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = Vector3.up * def.velocity;
                break;
            case WeaponType.spread:
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = Vector3.up * def.velocity;
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = new Vector3(-.2f, 0.9f, 0) * def.velocity;
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = new Vector3(.2f, 0.9f, 0) * def.velocity;
                break;
        }
    }

    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate(def.projectilePrefab) as GameObject;
        if (transform.parent.gameObject.tag == "Hero")
        {
            print("Player Shoots");
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position;
        go.transform.parent = PROJECTILE_ANCHOR;
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShot = Time.time;
        return (p);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
