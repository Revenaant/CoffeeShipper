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

    public void OnApproachCoffeeMachine()
    {
        nearCoffeeMachine = true;
    }

    public void OnLeaveCoffeeMachine()
    {
        nearCoffeeMachine = false;
    }
}
