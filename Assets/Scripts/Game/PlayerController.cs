using System;
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

    internal void AddForce(Vector3 velocityChangeAbs)
    {
        this.velocity += velocityChangeAbs;
    }
}
