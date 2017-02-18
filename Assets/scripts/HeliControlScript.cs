using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliControlScript : MonoBehaviour {

    public float speed = 75;
    public float rotate_speed = 10f;
    public float turn_speed = 15f;
    public GameObject rocket;
    public float rocket_speed = 10;
    public float rocket_down_force = 5000f;
    public float flyHeight = 75f;
    public float maxHeight = 200f;

    private bool airborne = false;
    private CharacterController charControl;
    private Rigidbody rb;
    private float forwardMove;
    private float strafeMove;
    private float rotationMove;
    private Terrain levelTerrain;

    // Use this for initialization
    private void Start()
    {
        this.levelTerrain = Terrain.activeTerrain;
        this.charControl = this.GetComponent<CharacterController>();
        this.rb = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rotationMove = Input.GetAxis("Horizontal") * turn_speed;
        forwardMove = Input.GetAxis("Vertical") * speed;
        strafeMove = 0f;

        if (Input.GetButtonDown("Fire1"))
        {

            //Vector3 down30 = new Vector3(-45, 0, 0);
            Quaternion down30 = Quaternion.EulerAngles(-45, 0, 0);
            Debug.Log("my rotation " + transform.rotation.eulerAngles + " after adjust " + (transform.rotation * down30).eulerAngles);
            GameObject newRocket = Instantiate(rocket, transform.position+transform.forward*15, transform.rotation );
            newRocket.GetComponent<Rigidbody>().AddForce(transform.forward * rocket_speed, ForceMode.Impulse);
            newRocket.GetComponent<Rigidbody>().AddForce(Vector3.down*rocket_down_force);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            strafeMove -= rotate_speed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            strafeMove += rotate_speed;
        }
        PlayFlySound();
    }

    private void PlayFlySound()
    {
       // audioSource.Play();
    }

    private void FixedUpdate()
    {
        Move();
        AdjustAltitude();
        Turn();
    }

    private void Move()
    {
        //float terrainHeight = levelTerrain.SampleHeight(transform.position);
        Vector3 move = transform.rotation * new Vector3(strafeMove, 0, forwardMove) * Time.deltaTime;
        //charControl.Move(move);
        this.rb.AddForce(move);
    }

    private void AdjustAltitude()
    {
        float newHeight = flyHeight;
        float terrainHeight = levelTerrain.SampleHeight(transform.position);
        if(terrainHeight >= maxHeight)
        {
            newHeight = maxHeight;
        }
        else
        {
            newHeight = terrainHeight + flyHeight;
        }
        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
    }

    private void Turn()
    {
        Vector3 rotation = new Vector3(0f, rotationMove, 0f) * Time.deltaTime;
        this.transform.Rotate(rotation * Time.deltaTime);
    }
}
