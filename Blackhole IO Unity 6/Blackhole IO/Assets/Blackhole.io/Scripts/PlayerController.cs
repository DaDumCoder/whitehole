using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// bool : isMoving
    /// </summary>
    public static UnityAction<bool> OnPlayerMovementChanged;

    [Header(" Physics ")]
    [SerializeField] private Rigidbody rig;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private Joystick joystick;
    private Vector3 previousVelocity;

    [Header(" Settings ")]
    private bool canMove;

    private void Awake()
    {
        TimersManager.OnBeforeGameTimerEnded += StartMoving;
        TimersManager.OnGameTimerEnded += StopMoving;
    }

    private void OnDestroy()
    {
        TimersManager.OnBeforeGameTimerEnded -= StartMoving;
        TimersManager.OnGameTimerEnded -= StopMoving;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void StartMoving()
    {
        canMove = true;
    }

    public void StopMoving()
    {
        canMove = false;
        rig.linearVelocity = Vector3.zero;

        OnPlayerMovementChanged?.Invoke(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
            Move();
    }

    private void Move()
    {
        Vector3 velocity = new Vector3(joystick.Direction.x, 0, joystick.Direction.y) * maxMoveSpeed;
        rig.linearVelocity = velocity;

        if (previousVelocity.magnitude < .1f && velocity.magnitude > 0)
            OnPlayerMovementChanged?.Invoke(true);
        else if (previousVelocity.magnitude > 0 && velocity.magnitude < .1f)
            OnPlayerMovementChanged?.Invoke(false);

        previousVelocity = velocity;

        //playerRenderer.forward = Vector3.Lerp(playerRenderer.forward, velocity.normalized, .1f * Time.deltaTime * 60);
    }
}
