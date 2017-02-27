using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAMissleScript : MonoBehaviour {

    public float timeToExplode = 2000f;
    public float thrust = 500f;
    public float turnRate = 80f;
    public GameObject explosion;

    private Transform player;
    private float bornOnDate;
    private Rigidbody rb;
	// Use this for initialization
	void Start () {
        bornOnDate = Time.time;
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
       
		if(Time.time - bornOnDate > timeToExplode)
        {
            ExplodeMissle();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * thrust;
        if (player)
        {
            Quaternion facing = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, facing, Time.deltaTime * turnRate);
        }
        
    }

    private void ExplodeMissle()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ExplodeMissle();
        }
    }
}
