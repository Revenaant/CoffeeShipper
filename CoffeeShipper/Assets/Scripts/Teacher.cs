using UnityEngine;
using UnityEngine.AI;

public class Teacher : MonoBehaviour
{
    public Player player;
    public NavMeshAgent agent;

    public int moveSpeed;
    public Vector3 patrolStart;
    public Vector3 patrolEnd;

    private bool patrolComplete = false;
    private bool followPlayer = false;
    public int viewAngle;
    public int viewDistance;
    private float timeOfLastCoffee;
    public float coffeeCooldown;
    private bool hasCoffee = false;

    // Start is called before the first frame update
    void Start()
    {
        agent.speed = moveSpeed;
        timeOfLastCoffee = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.coffeeCount > 0)
        {
            Vector3 angleToPlayer = player.transform.position - transform.position;
            if (Vector3.Angle(transform.forward, angleToPlayer) < viewAngle)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, angleToPlayer, out hit, viewDistance))
                {
                    if (hit.collider.gameObject.Equals(player.gameObject))
                    {
                        DetectPlayer();
                    }
                    else
                    {
                        followPlayer = false;
                    }
                }
            }
        }

        if (Time.time > timeOfLastCoffee + coffeeCooldown && hasCoffee)
        {
            FinishCoffee();
        }

        if (followPlayer && !hasCoffee)
        {
            agent.SetDestination(player.transform.position);
        }
        else if (patrolComplete)
        {
            agent.SetDestination(patrolStart);
            if (agent.remainingDistance < 0.1f && agent.remainingDistance != Mathf.Infinity && !agent.pathPending)
            {
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

    private void DetectPlayer()
    {
        followPlayer = true;
        Debug.Log("Found!");
    }

    private void GetCoffee()
    {
        Debug.Log("Taken");
        player.LoseCoffee();
        timeOfLastCoffee = Time.time;
        hasCoffee = true;
    }

    private void FinishCoffee()
    {
        Debug.Log("Finished");
        hasCoffee = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (player.coffeeCount > 0 && !hasCoffee)
            {
                GetCoffee();
            }
        }
        if(other.gameObject.tag == "Sound")
        {
            if(player.coffeeCount > 0 && !hasCoffee)
            {
                DetectPlayer();
            }
        }
    }
}
