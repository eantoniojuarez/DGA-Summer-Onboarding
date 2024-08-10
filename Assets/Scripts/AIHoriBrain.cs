using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class AIHoriBrain : MonoBehaviour
{
    public GameObject destinationA;
    public GameObject destinationB;
    public float speed = 5f;
    public bool isGoingToA = true;
    public GameObject playerDetector;
    public float stopDistance = 1f;
    public bool stopOnPlayer = false;
    public Transform player;
    public bool isHit = false;
    public float health = 10f;
    public AudioManager audioManager;
    public enum State
    {
        PATROL,
        CHASE
    }
    public State state = State.PATROL;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.PATROL)
        {
            Patrol();
        }
        else if (state == State.CHASE)
        {
            Chase();
        }

        if (playerDetector.GetComponent<AIBrainDetection>().isPlayerDetected)
        {
            isGoingToA = false;
            state = State.CHASE;
        }
        else
        {
            if (state == State.CHASE)
            {
                float distanceToA = Vector2.Distance(transform.position, destinationA.transform.position);
                float distanceToB = Vector2.Distance(transform.position, destinationB.transform.position);
                if (distanceToA < distanceToB)
                {
                    isGoingToA = true;
                }
                else
                {
                    isGoingToA = false;
                }
            }

            state = State.PATROL;
        }

        if (isHit)
        {
            health -= 10f;
            if (health <= 0)
            {
                audioManager.PlayKillSound();
                Destroy(gameObject);
            }
            // turn this object to red for a while
            GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(ResetColor());
            isHit = false;
        }
    }

    public void getHit(bool isMelee)
    {
        isHit = true;
        if (isMelee)
        {
            // knock back by a small force
            // GetComponent<Rigidbody2D>().velocity = new Vector2(-15f, 15f);
        }
    }

    IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void Patrol()
    {
        if (isGoingToA)
        {
            transform.position = Vector2.MoveTowards(transform.position, destinationA.transform.position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, destinationA.transform.position) < 0.5f)
            {
                isGoingToA = false;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, destinationB.transform.position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, destinationB.transform.position) < 0.5f)
            {
                isGoingToA = true;
            }
        }
    }

    void Chase()
    {

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), speed * Time.deltaTime);
        // Debug.Log("new position is " + new Vector2(player.position.x, transform.position.y));
    }
}
