using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPause
{
    [SerializeField]
    private MarkAudioPlayer audioPlayer;
    public MarkAudioPlayer AudioPlayer => audioPlayer;
    
    public CharacterController charController;
    public GameObject model;
    public GameObject noiseArea;
    public GameObject detectionUI;
    public GameObject detectionArrow;

    [SerializeField]
    private GameObject markHappyBalloon;
    [SerializeField]
    private GameObject markAngryBalloon;
    [SerializeField]
    private GameObject yvensBalloon;
    [SerializeField]
    private GameObject hansBalloon;

    public float noiseSensitivity;

    private List<GameObject> arrows;
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
    private bool isPaused;
    private float timeWhenPaused;
    public float pauseTime;

    public static bool inConversation;
    
    private bool balloonIsVisible;
    [SerializeField]
    private float balloonDuration;
    private float balloonAppearTime;
    private GameObject visibleBalloon;
    
    public Action OnCoffeeDelivered;
    public Action OnCoffeeLost;
    public Action OnTripToCoffeeMachine;

    private void Start()
    {
        maxCoffee = coffeeCups.Count;
        oldPosition = transform.position;
        UpdateCoffeeCups();
        Teacher.onDetectPlayer += Detected;
        Teacher.onLosePlayer += Undetected;
        arrows = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused || inConversation)
        {
            if (Time.time > timeWhenPaused + pauseTime) 
                inConversation = false;
            
            return;
        }
        
        CheckInput();
        Move();

        float soundRadius = (transform.position - oldPosition).magnitude * noiseSensitivity;
        noiseArea.transform.localScale = new Vector3(soundRadius, 0.1f, soundRadius);

        if(Time.time > balloonAppearTime + balloonDuration && balloonIsVisible)
        {
            HideBalloon();
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
        velocity += newVelocity * Time.deltaTime;

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

        model.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velocity, Vector3.up), 360);

        velocity *= 0.99f;
    }

    private void GrabCoffee()
    {
        coffeeCount = maxCoffee;
        UpdateCoffeeCups();

        // Play this here?
        audioPlayer.PlayCoffeePour(); 
        OnTripToCoffeeMachine?.Invoke();
    }

    public void LoseCoffee()
    {
        if(coffeeCount > 0)
        {
            coffeeCount--;
            UpdateCoffeeCups();
            OnCoffeeLost?.Invoke();
        }
    }

    public void GiveCoffee(Student student)
    {
        if (coffeeCount > 0 && !student.hasCoffee)
        {
            student.ReceiveCoffee();
            LoseCoffee();
            audioPlayer.PlayHappy();
            ShowBalloon(markHappyBalloon);
            OnCoffeeDelivered?.Invoke();
        }
    }

    private void ShowBalloon(GameObject objectToShow)
    {
        balloonIsVisible = true;
        objectToShow.SetActive(true);
        visibleBalloon = objectToShow;
        balloonAppearTime = Time.time;
    }

    public void ShowAngryMark()
    {
        ShowBalloon(markAngryBalloon);
    }

    public void ShowYvens()
    {
        ShowBalloon(yvensBalloon);
    }

    public void ShowHans()
    {
        ShowBalloon(hansBalloon);
    }

    private void HideBalloon()
    {
        balloonIsVisible = false;
        visibleBalloon.SetActive(false);
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

    public void StartConversation()
    {
        inConversation = true;
        timeWhenPaused = Time.time;
    }

    void IPause.Pause()
    {
        isPaused = true;
    }

    void IPause.Unpause()
    {
        isPaused = false;
    }

    private void Detected(Teacher teacher)
    {
        if (detectionUI == null)
            return;
        
        GameObject newArrow = Instantiate(detectionArrow, detectionUI.transform);
        DetectionArrow arrow = newArrow.GetComponent<DetectionArrow>();
        arrow.parent = this;
        arrow.teacherToPointAt = teacher;
        arrows.Add(newArrow);
    }

    private void Undetected(Teacher teacher)
    {
        for(int i = arrows.Count - 1; i >= 0; i--)
        {
            GameObject arrow = arrows[i];
            if (arrow.GetComponent<DetectionArrow>().teacherToPointAt == teacher)
            {
                arrows.Remove(arrow);
                Destroy(arrow);
            }
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
