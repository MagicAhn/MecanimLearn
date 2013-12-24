using System;
using UnityEngine;
using System.Collections;

public class LocomotionPlayer : MonoBehaviour
{
    protected Animator animator;

    private Single speed = 0;
    private Single direction = 0;
    private Locomotion locomotion = null;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        Locomotion.Anim = animator;
        locomotion = Locomotion.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator && Camera.main)
        {
            JoystickToEvents.Do(transform, Camera.main.transform, ref speed, ref direction);
            locomotion.Do(speed, direction);
        }
    }
}
