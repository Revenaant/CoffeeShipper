using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController charController;
    public GameObject model;
    public GameObject noiseArea;
    public float noiseSensitivity;

    public List<GameObject> coffeeCups;

    public float moveSpeed;
    private Vector3 velocity;
    public Vector3 gravity;

    public int coffeeCount;
    private int maxCoffee;

    private bool nearCoffeeMachine = false;
    private bool nearStudent = false;
    private Student nearbyStudent;

    private Vector3 oldPosition;
    public static bool isPaused;
    private  float timeWhenPaused;
    public float pauseTime;

    private void Start()
    {
        maxCoffee = coffeeCups.Count;
        oldPosition = transform.position;
        UpdateCoffeeCups();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            CheckInput();
            Move();

            float soundRadius = (transform.position - oldPosition).magnitude * noiseSensitivity;
            noiseArea.transform.localScale = new Vector3(soundRadius, soundRadius, soundRadius);

            model.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velocity, Vector3.up), 360);
        }

        if(isPaused && Time.time > timeWhenPaused + pauseTime)
        {
            isPaused = false;
        }
    }

    private void CheckInput()
    {
        Vector3 newVelocity = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            newVelocity += new Vector3(0, 0, moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            newVelocity += new Vector3(0, 0, -moveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            newVelocity += new Vector3(-moveSpeed, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            newVelocity += new Vector3(moveSpeed, 0, 0);
        }

        newVelocity.Normalize();
        newVelocity *= moveSpeed;
        velocity += newVelocity;

        if (nearCoffeeMachine && Input.GetKeyDown(KeyCode.E))
        {
            GrabCoffee();
        }

        if(nearStudent && Input.GetKeyDown(KeyCode.E))
        {
            GiveCoffee(nearbyStudent);
        }
    }

    private void Move()
    {
        oldPosition = transform.position;
        charController.Move(velocity + gravity);

        velocity *= 0.99f;
    }

    private void GrabCoffee()
    {
        Debug.Log("Coffee Grabbed");
        coffeeCount = maxCoffee;
        UpdateCoffeeCups();
    }

    public void LoseCoffee()
    {
        if(coffeeCount > 0)
        {
            coffeeCount--;
            UpdateCoffeeCups();
        }
    }

    public void GiveCoffee(Student student)
    {
        if (coffeeCount > 0 && !student.hasCoffee)
        {
            student.ReceiveCoffee();
            LoseCoffee();
        }
    }

    public void UpdateCoffeeCups()
    {
        for(int i = 0; i < coffeeCups.Count; i++)
        {
            if(coffeeCount > i)
            {
                coffeeCups[i].SetActive(true);
            }
            else
            {
                coffeeCups[i].SetActive(false);
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        timeWhenPaused = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Coffee Machine")
        {
            nearCoffeeMachine = true;
        }
        else if(other.gameObject.tag == "Student")
        {
            nearbyStudent = other.GetComponent<Student>();
            nearStudent = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Coffee Machine")
        {
            nearCoffeeMachine = false;
        }
        else if (other.gameObject.tag == "Student")
        {
            nearStudent = false;
        }
    }
}
