using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionArrow : MonoBehaviour
{
    public Player parent;
    public Teacher teacherToPointAt;

    private void Start()
    {
        RotateTowardsTeacher();
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardsTeacher();
    }

    private void RotateTowardsTeacher()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.up, (teacherToPointAt.transform.position - parent.transform.position)), 50);
    }
}
