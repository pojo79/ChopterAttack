using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AATurretScript : EnemyBase {

    public float rotateSpeed = 45f;
    public float shootAngle = 15f;
    public GameObject AAMissle;
    public float firingRange = 300f;
    public float fireRate = 5f;
    public Transform firePosition;
    public GameObject Fire;
    public Light flash;
    public float flashTime = .3f;

    private float flashStart = 0f;
    private float lastShot = 0f;
    
    // Update is called once per frame
    protected override void EnemySpecificUpdate()
    {
        if(Time.time - flashStart > flashTime)
        {
            flash.enabled = false;
        }

        Transform player = GameObject.FindWithTag("Player").transform;
        if (player)
        {
            Vector3 direction_diff = (transform.position - player.position);
            direction_diff.y = 0;

            Quaternion rotation = Quaternion.LookRotation(direction_diff);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * rotateSpeed);

            float angle = Vector3.Angle(transform.forward, direction_diff);
            if (angle <= shootAngle && Vector3.Magnitude(direction_diff) <= firingRange)
            {
                shoot(player);
            }
        }
    }

    private void shoot(Transform player)
    {
        if (Time.time - lastShot > fireRate)
        {
            lastShot = Time.time;
            flashStart = Time.time;
            flash.enabled = true;
            Instantiate(AAMissle, firePosition.position, firePosition.rotation);
        }
    }

    protected override void EnemySpecificStart()
    {
    }

    protected override void DoDeathSequence()
    {
        Destroy(this.gameObject);
        Instantiate(Fire, transform.position, transform.rotation);
    }

    protected override void EnemySpecificFixedUpdate()
    {
        
    }
}
