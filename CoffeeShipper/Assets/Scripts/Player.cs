using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController charController;
    public GameObject model;
    public GameObject noiseArea;
    public float noiseSensitivity;

    public float moveSpeed;
    private Vector3 velocity;
    public Vector3 gravity;

    public int coffeeCount;
    public int maxCoffee = 5;

    private bool nearCoffeeMachine = false;
    private bool nearStudent = false;
    private Student nearbyStudent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        Move();

        float soundRadius = velocity.magnitude * noiseSensitivity;
        noiseArea.transform.localScale = new Vector3(soundRadius, soundRadius, soundRadius);

        model.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velocity, Vector3.up), 360);

        Debug.Log("Coffee: " + coffeeCount);
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
        charController.Move(velocity + gravity);

        velocity *= 0.99f;
    }

    private void GrabCoffee()
    {
        Debug.Log("Coffee Grabbed");
        coffeeCount = maxCoffee;
    }

    public void GiveCoffee(Student student)
    {
        if(coffeeCount > 0 && !student.hasCoffee)
        {
            student.ReceiveCoffee();
            coffeeCount--;
        }
    }

    public void LoseCoffee()
    {
        if(coffeeCount > 0)
        {
            coffeeCount--;
        }
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
