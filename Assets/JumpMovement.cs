using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMovement : MonoBehaviour
{

    public float fallMultiplier = 2.5f;
    public float JumpMultiplier = 2.5f;

    public float jumpHeight;
    public float accelerationRate = 0.5f;
    public float deccelerationRate = 0.5f;
    public Rigidbody2D rigidbody;
    public float defaultJump = 50;

    public PlayerMovement playerMovement;

    public float maxJump = 75;

    [SerializeField] bool canJump = true;

    [SerializeField] bool isGrounded = false;
    [SerializeField] bool isSloped = false;
    public Transform GroundCheck1; // Put the prefab of the ground here
    public LayerMask groundLayer;
    public LayerMask SlopeLayer;

    [SerializeField] Collider2D slopeCol;

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck1.position, 0.15f, groundLayer);

        if (Physics2D.OverlapCircle(GroundCheck1.position, 0.15f, groundLayer) != null)
        {
            GameObject ghostplatform = Physics2D.OverlapCircle(GroundCheck1.position, 0.15f, groundLayer).GetComponent<GameObject>();
            Debug.Log(ghostplatform);
        }
        isSloped = Physics2D.OverlapCircle(GroundCheck1.position, 0.15f, SlopeLayer);

        if (isSloped)
        {
            playerMovement.OnSlope = true;
            playerMovement.moreDire = 0;
            slopeCol = Physics2D.OverlapCircle(GroundCheck1.position, 0.15f, SlopeLayer).gameObject.GetComponent<Collider2D>();

            if (slopeCol != null)
            {
                if (slopeCol.gameObject.GetComponent<SlopeBlock>().direction.ToString().Equals("Right"))
                {
                    rigidbody.velocity = new Vector2(350f * Time.fixedDeltaTime, rigidbody.velocity.y);
                    playerMovement.moreDire = 8;

                }
                else
                {
                    rigidbody.velocity = new Vector2(-350f * Time.fixedDeltaTime, rigidbody.velocity.y);
                    playerMovement.moreDire = -8;
                }
            }

            return;
        }
        else
        {
            playerMovement.OnSlope = false;
            slopeCol = null;
        }


        if (Input.GetKey(KeyCode.Space))
        {
            if(jumpHeight < maxJump)
            {
                jumpHeight += jumpHeight * accelerationRate * Time.fixedDeltaTime;
            }


        }
        else if (jumpHeight >= defaultJump)
        {
            jumpHeight -= defaultJump * deccelerationRate * Time.fixedDeltaTime;
        }
        else
        {
            jumpHeight = defaultJump;
        }

        if (isGrounded && canJump)
        {
            playerMovement.ballsfx.Play();
            canJump = false;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0f);
            rigidbody.AddForce(Vector2.up * jumpHeight * 10);
            StartCoroutine(Jump());
        }



        if (rigidbody.velocity.y < 0)
        {
            rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }

    }

    IEnumerator Jump()
    {
        yield return new WaitForSeconds(0.5f);
        canJump = true;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Wall"))
        {
            if(rigidbody.velocity.x < 0)
            {
                rigidbody.AddForce(Vector2.up * -rigidbody.velocity.x * 5);
            }
            else
            {
                rigidbody.AddForce(Vector2.up * rigidbody.velocity.x * 5);
            }
        }

 
    }



}
