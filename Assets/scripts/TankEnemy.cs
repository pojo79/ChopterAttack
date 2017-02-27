using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class TankEnemy : EnemyBase
{
    public GameObject Fire;
    public GameObject AAShot;
    public float shotRandomness = 150;
    public float rotateSpeed = 15;
    public Transform turret;
    public float firingAngle = 30;
    public float firingRange = 400;
    public float fireRate = 5; //seconds
    public float thrust = 450;
    public float tankTurn = 20;

    private Vector3 travelDestination;
    private Rigidbody rb;
    private float lastFired = 0;
    

    protected override void EnemySpecificUpdate()
    {
        if (transform.position == travelDestination)
        {
            travelDestination = new Vector3(transform.position.x + UnityEngine.Random.Range(-800, 800), transform.position.y, transform.position.z + UnityEngine.Random.Range(-800, 800));
        }
    }

    private void shoot(Transform player)
    {
        if (Time.realtimeSinceStartup - lastFired >= fireRate)
        {
            Vector3 shotPostition = new Vector3(UnityEngine.Random.Range(0, shotRandomness) + player.position.x, UnityEngine.Random.Range(0, shotRandomness) + player.position.y, UnityEngine.Random.Range(0, shotRandomness) + player.position.z);
            Instantiate(AAShot, shotPostition, transform.rotation);
            lastFired = Time.realtimeSinceStartup;
            Debug.Log("Bang Bang lastFired = "+lastFired + " time = "+Time.realtimeSinceStartup);
        }
    }

    protected override void EnemySpecificFixedUpdate()
    {
        MoveTowardObjective();
        TryToKillPlayer();
    }

    protected void OnCollisionEnter(Collision collision)
    {
        travelDestination = transform.position;
    }

    protected override void DoDeathSequence()
    {
        Instantiate(Fire, transform.position, transform.rotation);
    }

    protected override void EnemySpecificStart()
    {
        this.rb = GetComponent<Rigidbody>();
        travelDestination = transform.position;
    }

    private void MoveTowardObjective()
    {
        Quaternion facing = Quaternion.LookRotation(travelDestination - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, facing, Time.deltaTime * tankTurn);
        rb.AddForce(transform.forward * thrust);
    }

    private void TryToKillPlayer()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        if (player)
        {
            Vector3 direction_diff = (turret.transform.position - player.position);
            direction_diff.y = 0;

            Quaternion rotation = Quaternion.LookRotation(direction_diff);
            turret.transform.rotation = Quaternion.RotateTowards(turret.transform.rotation, rotation, Time.deltaTime * rotateSpeed);


            float angle = Vector3.Angle(turret.transform.forward, direction_diff);
            if (angle < firingAngle && Vector3.Magnitude(direction_diff) <= firingRange)
            {
                shoot(player);
            }
        }
    }
}
