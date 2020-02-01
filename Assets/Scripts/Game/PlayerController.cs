using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;

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

    void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3();
        bool grounded = controller.isGrounded;
        for (int i = 0; i < groundReferences.Count; i++)
        {
            bool ground = Physics.Raycast(new Ray(groundReferences[i].position, Vector3.down), groundCheckDistance, groundMask);
            if (ground)
            {
                grounded = true;
                break;
            }
        }
        RaycastHit raycastHit;

        float left = Input.GetKey(KeyCode.A) ? 1 * (player.allowLeftMovement ? 1 : 0) : 0;
        float right = Input.GetKey(KeyCode.D) ? 1 * (player.allowRightMovement ? 1 : 0) : 0;
        movement += Vector3.right * moveForce * right * (velocity.x < 0 ? 2 : 1);
        movement += Vector3.left * moveForce * left * (velocity.x > 0 ? 2 : 1);
        velocity += movement * Time.deltaTime;
        if (grounded && left + right == 0)
        {
            velocity.x *= Time.deltaTime * 50;
        }
        if (grounded)
        {
            velocity.x = Mathf.Min(Mathf.Abs(velocity.x), maxHorizontalVelocity) * Mathf.Sign(velocity.x);
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
        if (grounded)
        {
            velocity.y = 0;
        }
        controller.Move(velocity);
        float maxDist = 0;
        for (int i = 0; i < groundReferences.Count; i++)
        {
            Transform groundRef = groundReferences[2];
            bool hit = Physics.Raycast(new Ray(groundRef.position + Vector3.up, Vector3.down), out raycastHit, 1.1f + Mathf.Abs(velocity.x * 2.0f), groundMask);
            if (hit)
            {
                Debug.DrawLine(raycastHit.point, raycastHit.point + raycastHit.normal, Color.red, 0.1f);
                maxDist = Mathf.Min((raycastHit.point - groundRef.position).y, maxDist);
            }
        }
        transform.position += Vector3.up * maxDist;
    }
}


/*
 *     private void FixedUpdate()
    {
        Simulate();
                       
        // Higher iteration results in stiffer ropes and stable simulation
        for (int i = 0; i < 80; i++)
        {
            ApplyConstraint();

            // Playing around with adjusting collisions at intervals - still stable when iterations are skipped
            if (i % 2 == 1)
                AdjustCollisions();
        }
    }

    private void Simulate()
    {
        // step each node in rope
        for (int i = 0; i < TotalNodes; i++)
        {            
            // derive the velocity from previous frame
            Vector3 velocity = RopeNodes[i].transform.position - RopeNodes[i].PreviousPosition;
            RopeNodes[i].PreviousPosition = RopeNodes[i].transform.position;

            // calculate new position
            Vector3 newPos = RopeNodes[i].transform.position + velocity;
            newPos += Gravity * Time.fixedDeltaTime;
            Vector3 direction = RopeNodes[i].transform.position - newPos;
                        
            // cast ray towards this position to check for a collision
            int result = -1;
            result = Physics2D.CircleCast(RopeNodes[i].transform.position, RopeNodes[i].transform.localScale.x / 2f, -direction.normalized, ContactFilter, RaycastHitBuffer, direction.magnitude);

            if (result > 0)
            {
                for (int n = 0; n < result; n++)
                {                    
                    if (RaycastHitBuffer[n].collider.gameObject.layer == 9)
                    {
                        Vector2 collidercenter = new Vector2(RaycastHitBuffer[n].collider.transform.position.x, RaycastHitBuffer[n].collider.transform.position.y);
                        Vector2 collisionDirection = RaycastHitBuffer[n].point - collidercenter;
                        // adjusts the position based on a circle collider
                        Vector2 hitPos = collidercenter + collisionDirection.normalized * (RaycastHitBuffer[n].collider.transform.localScale.x / 2f + RopeNodes[i].transform.localScale.x / 2f);
                        newPos = hitPos;
                        break;              //Just assuming a single collision to simplify the model
                    }
                }
            }

            RopeNodes[i].transform.position = newPos;
        }
    }
    
    private void AdjustCollisions()
    {
        // Loop rope nodes and check if currently colliding
        for (int i = 0; i < TotalNodes - 1; i++)
        {
            RopeNode node = this.RopeNodes[i];

            int result = -1;
            result = Physics2D.OverlapCircleNonAlloc(node.transform.position, node.transform.localScale.x / 2f, ColliderHitBuffer);

            if (result > 0)
            {
                for (int n = 0; n < result; n++)
                {
                    if (ColliderHitBuffer[n].gameObject.layer != 8)
                    {
                        // Adjust the rope node position to be outside collision
                        Vector3 collidercenter = ColliderHitBuffer[n].transform.position;
                        Vector3 collisionDirection = node.transform.position - collidercenter;

                        Vector3 hitPos = collidercenter + collisionDirection.normalized * ((ColliderHitBuffer[n].transform.localScale.x / 2f) + (node.transform.localScale.x / 2f));
                        node.transform.position = hitPos;
                        break;
                    }
                }
            }
        }    
    }

    private void ApplyConstraint()
    {
        // Check if the first node is clamped to the scene or is follwing the mouse
        if (Node1Lock != Vector2.zero)
        {
            RopeNodes[0].transform.position = Node1Lock;
        }
        else
        {
            RopeNodes[0].transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        for (int i = 0; i < TotalNodes - 1; i++)
        {
            RopeNode node1 = this.RopeNodes[i];
            RopeNode node2 = this.RopeNodes[i + 1];

            // Get the current distance between rope nodes
            float currentDistance = (node1.transform.position - node2.transform.position).magnitude;
            float difference = Mathf.Abs(currentDistance - NodeDistance);
            Vector2 direction = Vector2.zero;
           
            // determine what direction we need to adjust our nodes
            if (currentDistance > NodeDistance)
            {
                direction = (node1.transform.position - node2.transform.position).normalized;
            }
            else if (currentDistance < NodeDistance)
            {
                direction = (node2.transform.position - node1.transform.position).normalized;
            }

            // calculate the movement vector
            Vector3 movement = direction * difference;

            // apply correction
            node1.transform.position -= (movement * 0.5f);
            node2.transform.position += (movement * 0.5f);
        }
    }

 */
