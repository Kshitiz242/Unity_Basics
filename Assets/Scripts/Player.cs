using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Transform groundcheckTransform;
    [SerializeField] private LayerMask playerMask;
    private bool Jumpkeypress;
    float horizontalinput;
    Rigidbody rbcomp;   //defined it to improve performance by fetching all rigid body characterstics at once
    int superJumpsRemaining;

    // Start is called before the first frame update
    void Start()
    {
        rbcomp = GetComponent<Rigidbody>();        
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Space)) { 
            Jumpkeypress= true;
        }
        horizontalinput = Input.GetAxis("Horizontal");
    }

    //FixedUpdate is called once every Physic update
   void FixedUpdate()
    {
        rbcomp.velocity = new Vector3(horizontalinput, rbcomp.velocity.y, 0); //adds lateral movement
        
        if (Physics.OverlapSphere(groundcheckTransform.position,0.1f, playerMask).Length == 0)
        {
            return;
        }

        if (Jumpkeypress)
        {
            float jumpPower=6.5f;
            if (superJumpsRemaining > 0)
            {
                jumpPower *= 1.5f;
                superJumpsRemaining--;
            }
            rbcomp.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            Jumpkeypress = false; 
        }
   }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        { 
            Destroy(other.gameObject);
            superJumpsRemaining++;
        }
    }

}
