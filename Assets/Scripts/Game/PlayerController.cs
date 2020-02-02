using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody controller;

    private Vector3 velocity = new Vector3();

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private Vector3 gravityDirection = new Vector3(0, -1, 0);

    [SerializeField]
    private float gravityStrength = -9.8f;

    [SerializeField]
    private float moveForce = 5f;
    [SerializeField]
    private float jumpForce = 25f;

    [SerializeField]
    private float maxHorizontalVelocity = 5f;

    [SerializeField]
    private float groundCheckDistance = 0.1f;

    [SerializeField]
    private float jumpCoolDown = 0.1f;

    private float lastJumpTime = -1;

    [SerializeField]
    private bool disableGravity = false;

    [SerializeField]
    private Player player;

    [SerializeField]
    private List<Transform> groundReferences = new List<Transform>();

    private int hasGroundVelocityDecay = 0;

    [SerializeField]
    private Transform rotationTransform;
    void Awake()
    {
        controller = gameObject.GetComponent<Rigidbody>();
    }

    public void EnableGroundVelocityDecay()
    {
        hasGroundVelocityDecay--;
    }
    public void DisableGroundVelocityDecay()
    {
        hasGroundVelocityDecay++;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3();
        bool grounded = false;
        Vector3 groundNormal = Vector3.up;
        velocity.y = controller.velocity.y;
        for (int i = 0; i < groundReferences.Count; i++)
        {
            bool ground = Physics.Raycast(new Ray(groundReferences[i].position + Vector3.up * groundCheckDistance, Vector3.down), groundCheckDistance * 2f, groundMask, QueryTriggerInteraction.Ignore);
            if (ground)
            {

                grounded = true;
                break;
            }
        }

        float left = Input.GetKey(KeyCode.A) ? 1 * (player.allowLeftMovement ? 1 : 0) : 0;
        float right = Input.GetKey(KeyCode.D) ? 1 * (player.allowRightMovement ? 1 : 0) : 0;
        float scale = (rotationTransform.eulerAngles.y + 360);
        if (left > 0 || velocity.x < 0)
        {
            scale = 180;
        }
        else
        {
            scale = 0;
        }
        Vector3 euler = rotationTransform.eulerAngles;
        euler.y = scale;
        rotationTransform.eulerAngles = euler;
        movement += Vector3.right * moveForce * right * (velocity.x < 0 ? 2 : 1);
        movement += Vector3.left * moveForce * left * (velocity.x > 0 ? 2 : 1);
        velocity += movement * Time.deltaTime;
        if (grounded && (left + right == 0) && hasGroundVelocityDecay == 0)
        {
            velocity.x *= Time.deltaTime * 50;
        }
        velocity.x = Mathf.Min(Mathf.Abs(velocity.x), maxHorizontalVelocity * (grounded ? 1 : 1.5f)) * Mathf.Sign(velocity.x);


        if ((!grounded || Time.time - lastJumpTime <= jumpCoolDown && player.allowJump) && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("No jump ground " + grounded + " " + (Time.time - lastJumpTime <= jumpCoolDown));
        }

        if (grounded && Input.GetKeyDown(KeyCode.Space) && Time.time - lastJumpTime >= jumpCoolDown && player.allowJump)
        {
            lastJumpTime = Time.time;
            velocity.y = jumpForce;
            grounded = false;
        }

        else if (!disableGravity)
        {
            velocity += gravityDirection * gravityStrength * Time.deltaTime;
        }
        controller.velocity = (velocity);
    }

    private void FixedUpdate()
    {
        if (velocity.y <= 0)
        {
            float maxDist = 0;
            RaycastHit raycastHit;

            for (int i = 0; i < groundReferences.Count; i++)
            {
                Transform groundRef = groundReferences[2];
                bool hit = Physics.Raycast(new Ray(groundRef.position + Vector3.up, Vector3.down), out raycastHit, .1f + Mathf.Abs(velocity.x) * .2f, groundMask, QueryTriggerInteraction.Ignore);
                if (hit && Vector3.Dot(raycastHit.normal, Vector3.down) > 0.45f)
                {
                    Debug.DrawLine(raycastHit.point, raycastHit.point + raycastHit.normal, Color.red, 0.1f);
                    maxDist = Mathf.Min((raycastHit.point - groundRef.position).y, maxDist);
                }
            }
            if (maxDist < .1f)
            {
                controller.MovePosition(transform.position + Vector3.up * maxDist);
            }
        }
    }

    internal void AddForce(Vector3 vector3)
    {
        velocity += vector3;
        velocity.x = Mathf.Min(Mathf.Abs(velocity.x), maxHorizontalVelocity) * Mathf.Sign(velocity.x);
    }
}
