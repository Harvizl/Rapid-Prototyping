using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    // The Speed in m/s
    public float speed = 10f;

    // Seconds/shots (Unused)
    public float fireRate = 0.3f;

    // Health
    public float health = 10;

    // Points earned for destorying this
    public int score = 100;

    // # frames to show damage
    public int showDamageForFrames = 2;

    // Chance to drop a power-up
    public float powerUpDropChance = 1f; 

    public bool ________________;

    public Color[] originalColors;

    // All the Materials of this & its children
    public Material[] materials;

    // Damage frames left
    public int remainingDamageFrames = 0; 

    // The Bounds of this and its children
    public Bounds bounds;

    //Distance of bounds.center from position
    public Vector3 boundsCenterOffset;

    void Awake()
    {
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }

        InvokeRepeating("CheckOffscreen", 0f, 2f);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (remainingDamageFrames > 0)
        {
            remainingDamageFrames--;
            if (remainingDamageFrames == 0)
            {
                UnShowDamage();
            }
        }
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    // This is a Property: A method that acts like a field
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    void CheckOffscreen()
    {
        // If Bounds are still their default value...
        if (bounds.size == Vector3.zero)
        {
            // Then set them
            bounds = Utils.CombineBoundsOfChildren(this.gameObject);

            // Also find the difference between bounds.center & transform.position
            boundsCenterOffset = bounds.center - transform.position;
        }

        // Every time, update the bounds to the current position
        bounds.center = transform.position + boundsCenterOffset;

        // Check to see whether the Bounds are completely offscreen
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen);
        if (off != Vector3.zero)
        {
            // If this enemy has gone off the bottom edge of the screen
            if (off.y < 0)
            {
                // Then destroy it
                Destroy(this.gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        switch (other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();

                // Enemies don't take damage unless they're onscreen
                // This stops the player from shooting them before they are visible
                bounds.center = transform.position + boundsCenterOffset;
                if (bounds.extents == Vector3.zero || Utils.ScreenBoundsCheck(bounds,
                BoundsTest.offScreen) != Vector3.zero)
                {
                    Destroy(other);
                    break;
                }

                // Hurt this Enemy
                ShowDamage();

                // Get the damage amount from the Projectile.type & Main.W_DEFS
                health -= Main.W_DEFS[p.type].damageOnHit;
                if (health <= 0)
                {
                    // Tell the Main singleton that this ship has been destroyed
                    Main.S.ShipDestroyed(this);

                    // Destroy this Enemy
                    Destroy(this.gameObject);
                }
                Destroy(other);
                break;
        }
    }

    void ShowDamage()
    {
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }
        remainingDamageFrames = showDamageForFrames;
    }
    public void UnShowDamage()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
    }
}
