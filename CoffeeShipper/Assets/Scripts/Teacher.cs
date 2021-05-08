using System;
using UnityEngine;
using UnityEngine.AI;

public class Teacher : MonoBehaviour
{
    private Player player;
    public NavMeshAgent agent;
    public GameObject exclamationMark;
    public GameObject coffee;
    public bool standStill;

    public int moveSpeed;
    public Vector3 patrolStart;
    public Vector3 patrolEnd;

    private Quaternion startingRotation;

    private bool patrolComplete = false;
    private bool followPlayer = false;
    public int viewAngle;
    public int viewDistance;
    private float timeOfLastCoffee;
    public float coffeeCooldown;
    private bool hasCoffee = false;
    private LayerMask layerMask;

    private bool canHearPlayer = false;
    private bool canSeePlayer = false;

    public static event Action<Teacher> onDetectPlayer;
    public static event Action<Teacher> onLosePlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        agent.speed = moveSpeed;
        timeOfLastCoffee = 0;
        exclamationMark.SetActive(false);
        coffee.SetActive(false);
        layerMask = LayerMask.GetMask("Player", "Default");
        startingRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Player.isPaused)
        {
            agent.isStopped = false;
            CheckLineOfSight();
            DrinkCoffee();
            UpdatePlayerDetection();
            UpdateAgentDestination();
            UpdatePopups();

            coffee.transform.rotation = Quaternion.Euler(75, 0, transform.rotation.z * -1f);
            exclamationMark.transform.rotation = Quaternion.Euler(75, 0, transform.rotation.z * -1f);
        }
        else
        {
            agent.isStopped = true;
        }
    }

    private void CheckLineOfSight()
    {
        if (player.coffeeCount > 0)
        {
            Vector3 angleToPlayer = player.transform.position - transform.position;
            if (Vector3.Angle(transform.forward, angleToPlayer) < viewAngle)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, angleToPlayer, out hit, viewDistance, layerMask, QueryTriggerInteraction.Ignore))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        canSeePlayer = true;
                    }
                    else
                    {
                        canSeePlayer = false;
                    }
                }
            }
        }
    }

    private void DrinkCoffee()
    {
        if (Time.time > timeOfLastCoffee + coffeeCooldown && hasCoffee)
        {
            FinishCoffee();
        }
    }

    private void UpdateAgentDestination()
    {
        if (followPlayer && !hasCoffee)
        {
            agent.SetDestination(player.transform.position);
        }
        else if (patrolComplete || standStill)
        {
            agent.SetDestination(patrolStart);
            if (agent.remainingDistance < 0.1f && agent.remainingDistance != Mathf.Infinity && !agent.pathPending)
            {
                if (standStill)
                {
                    transform.rotation = startingRotation;
                }

                patrolComplete = false;
            }
        }
        else
        {
            agent.SetDestination(patrolEnd);
            if (agent.remainingDistance < 0.1f && agent.remainingDistance != Mathf.Infinity && !agent.pathPending)
            {
                patrolComplete = true;
            }
        }
    }

    private void UpdatePlayerDetection()
    {
        if ((canHearPlayer || canSeePlayer) && player.coffeeCount > 0 && !hasCoffee)
        {
            if (!followPlayer)
            {
                DetectPlayer();
            }
        }
        else if (followPlayer)
            LosePlayer();
    }

    private void DetectPlayer()
    {
        followPlayer = true;
        onDetectPlayer.Invoke(this);
    }

    private void LosePlayer()
    {
        followPlayer = false;
        onLosePlayer.Invoke(this);
    }

    private void UpdatePopups()
    {
        if (hasCoffee)
            coffee.SetActive(true);
        else
        {
            coffee.SetActive(false);
        }

        if (followPlayer && !hasCoffee)
            exclamationMark.SetActive(true);
        else if (!followPlayer || hasCoffee)
            exclamationMark.SetActive(false);
    }

    private void GetCoffee()
    {
        Debug.Log("Taken");
        player.LoseCoffee();
        player.AudioPlayer.PlayAngry();
        
        timeOfLastCoffee = Time.time;
        hasCoffee = true;
        player.Pause();
    }

    private void FinishCoffee()
    {
        Debug.Log("Finished");
        hasCoffee = false;
        coffee.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (player.coffeeCount > 0 && !hasCoffee)
                GetCoffee();
        }
        if (other.gameObject.tag == "Sound")
        {
            if (player.coffeeCount > 0 && !hasCoffee)
                canHearPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Sound")
            canHearPlayer = false;
    }
}
