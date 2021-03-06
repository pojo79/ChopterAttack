﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket_Bullet : MonoBehaviour {

    public float rocket_life = 500f;
    public LayerMask enemyLayer;
    public float rocketExplodeRadius = 10f;
    public float rocketExplodeForce = 30f;
    public float rocketDamage = 80f;
    public GameObject explosion;

    public float lifeSpan = 750f;
    private float bornOnDate;
    // Use this for initialization
    void Start () {
        bornOnDate = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if( Time.time - bornOnDate >= lifeSpan)
        {
            Object.Destroy(this.gameObject);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.tag != "Non-Interactable")
        {
            //Debug.Log("colliding with "+ other.tag + other.GetType());
            Collider[] colliders = Physics.OverlapSphere(transform.position, rocketExplodeRadius, enemyLayer);

            for(int i=0; i< colliders.Length; i++)
            {
                Rigidbody trb = colliders[i].GetComponent<Rigidbody>();

                if(trb == null)
                {
                    continue;
                }

                EnemyBase enemy = colliders[i].GetComponent<EnemyBase>();
                enemy.DoDamage(CalculateDamageFromBlast(enemy.transform.position));
                trb.AddExplosionForce(rocketExplodeForce, transform.position, rocketExplodeRadius);
            }
            GameObject exp = Instantiate(explosion, transform.position, transform.rotation);

            Object.Destroy(this.gameObject);
        }
    }

    private float CalculateDamageFromBlast(Vector3 targetPosition)
    {
        Vector3 explosionDistanceToTarget = targetPosition - transform.position;

        float percentRadiusDistance = (rocketExplodeRadius - explosionDistanceToTarget.magnitude)/rocketExplodeRadius;

        return Mathf.Max(0f, percentRadiusDistance * rocketDamage);
    }
}
