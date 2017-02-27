using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAShot : MonoBehaviour {

    public float lifetime = 5; //seconds
    public LayerMask playerLayer;
    public float shrapnelRadius = 50;
    public float explosiveForce = 1200;
    public float AADamage = 25;

    private float bornOnDate = 0;
	// Use this for initialization
	void Start () {
        bornOnDate = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.realtimeSinceStartup - bornOnDate > lifetime)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
                Rigidbody trb = other.GetComponent<Rigidbody>();
                HeliControlScript heli = other.GetComponent<HeliControlScript>();
                heli.DoDamage(CalculateDamageFromBlast(heli.transform.position));
                trb.AddExplosionForce(explosiveForce, transform.position, shrapnelRadius);
        }
 
    }



    private float CalculateDamageFromBlast(Vector3 targetPosition)
    {
        Vector3 explosionDistanceToTarget = targetPosition - transform.position;

        float percentRadiusDistance = (shrapnelRadius - explosionDistanceToTarget.magnitude) / shrapnelRadius;

        return Mathf.Max(0f, percentRadiusDistance * AADamage);
    }
}
