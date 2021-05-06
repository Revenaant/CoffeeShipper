using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController charController;
    public float moveSpeed;
    private Vector3 velocity;
    private int coffeeCount;
    public int maxCoffee = 5;

    private bool nearCoffeeMachine = false;
    private bool nearStudent = false;
    private Student nearbyStudent;

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        Move();

        Debug.Log("Coffee: " + coffeeCount);
    }

    private void CheckInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            velocity += new Vector3(0, 0, moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity += new Vector3(0, 0, -moveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity += new Vector3(-moveSpeed, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity += new Vector3(moveSpeed, 0, 0);
        }

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
        charController.Move(velocity);

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
