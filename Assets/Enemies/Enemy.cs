using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected AudioSource audioSource;
    protected float direction;
    [SerializeField] protected float speed;
    [SerializeField] protected bool isInLevel;
    protected Vector3 distanceToPlayer;
    protected Vector3 PlayerPosition;

    protected float initialX;
    protected bool isPlayerInRange;
    [SerializeField] protected float range;
    protected float distanceFromSpawn;
    // Start is called before the first frame update
    public static Transform MapTransform;
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        direction = 1;

        isPlayerInRange = false;

        initialX = transform.position.x;

        if(MapTransform == null && isInLevel){
            MapTransform = GameObject.Find("Map").transform;
            initialX = transform.position.x - MapTransform.position.x;
        }
    }

    protected void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy")){
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
        }
    }

    protected void FixedUpdate(){

        PlayerPosition = PlayerMovement.instance.GetPosition();
        distanceToPlayer = PlayerPosition - transform.position;
        if(isInLevel){
            distanceFromSpawn = transform.position.x - (initialX + MapTransform.position.x);            
        }else{
            distanceFromSpawn = transform.position.x - initialX;
        }


        if(direction * transform.localScale.x < 0){
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        bool isPlayerActuallyInRange = Mathf.Abs(distanceToPlayer.x) < range && Mathf.Abs(distanceToPlayer.y) < 1;

        if(isPlayerActuallyInRange && !isPlayerInRange){
            isPlayerInRange = true;
            direction = distanceToPlayer.x/Mathf.Abs(distanceToPlayer.x);
        }

        else if(!isPlayerActuallyInRange && isPlayerInRange){
            isPlayerInRange = false;
        }

        if(Mathf.Abs(distanceFromSpawn) > range && direction * distanceFromSpawn > 0){
            direction *= -1;
        }

        if(isInLevel){
            rb.velocity = new Vector2(direction * speed - MapMover.offset, rb.velocity.y);
        }else{
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        }

    }
}
