using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    public float rotationSpeed;
    public float jumpForce;
    public Animator animator;
    public Transform camera;
    public Transform footReference;

    private Rigidbody rigidbody;
    private bool inAir;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        Cursor.visible = false;
    }

    private void Update()
    {
        Animation();
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!inAir)
            Movement();

        if (Input.GetButtonDown("Jump") && !inAir)
        {
            Jump();
        }
    }
    void Movement()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 forceForward = (camera.transform.forward * vertical * speed);
        Vector3 forceRight = (camera.transform.right * horizontal * speed);
        forceForward.y = 0;
        forceRight.y = 0;
        rigidbody.AddForce(forceForward);
        rigidbody.AddForce(forceRight);

        if (rigidbody.velocity.magnitude > maxSpeed)
            rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;


        Vector3 a = new Vector3(camera.position.x, 0, camera.position.z);
        Vector3 b = new Vector3(transform.position.x, 0, transform.position.z);
        Quaternion rotationA = Quaternion.LookRotation((b - a), Vector3.up); 
        transform.rotation = rotationA;

        CalcSpeed();

    }
    void CalcSpeed()
    {
        float forwardSpeed = MultiplyVectorsAndSize(transform.forward,rigidbody.velocity);
        float rightSpeed = MultiplyVectorsAndSize(transform.right,rigidbody.velocity);
        float upSpeed = MultiplyVectorsAndSize(transform.up, rigidbody.velocity);


        animator.SetFloat("forwardSpeed", forwardSpeed / maxSpeed);
        animator.SetFloat("rightSpeed", rightSpeed / maxSpeed);
        animator.SetFloat("upSpeed", upSpeed / maxSpeed);
    }
    float MultiplyVectorsAndSize(Vector3 a, Vector3 b)
    {
        return  (a.x * b.x + a.y * b.y + a.z * b.z);
    }

    void Animation()
    {
        animator.SetFloat("speed", rigidbody.velocity.magnitude / maxSpeed);
    }

    void Jump()
    {
        rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Floor")
        {
            inAir = false;
            animator.SetBool("inAir", inAir);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Floor")
        {
            inAir = true;
            animator.SetBool("inAir", inAir);
        }
    }
}
