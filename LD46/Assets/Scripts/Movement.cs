using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] Rigidbody2D rigidbody;
    public Vector3 currentPosition { get; private set; }

    float maxAcc = 15f;
    float maxSpeed = 3.5f;
    float dragCoefficient = 8f;

    Vector2 input;


    void Start()
    {
        if(rigidbody == null)
        {
            rigidbody = transform.GetComponent<Rigidbody2D>();
            if (rigidbody == null)
            {
                Debug.LogError("Jogador nao possui RigidBody");
            }
        }

        currentPosition = transform.position;

    }
    float inputAcc;
    float inputVelocity;
    Vector2 inputDirection;
    Vector2 velocityVector;

    void Update()
    {
        Move();
        currentPosition = transform.position;


    }

    void Move()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        inputAcc = (input.magnitude * maxAcc) - (dragCoefficient * rigidbody.velocity.magnitude);
        inputVelocity += inputAcc * Time.deltaTime;
        inputVelocity = Mathf.Abs(Mathf.Clamp(inputVelocity, 0, maxSpeed));
        inputDirection = input.normalized;
        velocityVector = inputDirection * inputVelocity;
        rigidbody.velocity = velocityVector;
    }

}
