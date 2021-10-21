using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject _madBall;
    [SerializeField]
    private BallStats _ballStats;
    [SerializeField]
    private Animator _ballAnimator;
    [SerializeField]
    private Field _field;
    [SerializeField]
    private TimeManager _tm;
    [SerializeField]
    private PhaseManager _pm;
    [SerializeField]
    private PhaseRun _pr;
    [SerializeField]
    private Transform _ballTrans;
    [SerializeField]
    private float RotationSpeed;

    private Quaternion lookRotation;
    public float minDistance;
    public float slowDownRate;
    public bool isAttaccking;
    public bool wantAttack;
    public bool isSlowingDown;
    public Vector3 target;
    public float velocity;
    public float minVel;
    private Rigidbody rb;

    void Start()
    {
        target = new Vector3(0, 0, 0);
        isAttaccking = false;
        rb = transform.GetComponent<Rigidbody>();
    }
    void Update()
    {
        velocity = rb.velocity.magnitude;
    }

    public Vector3 getBestSpot()
    {
        Vector2 ballPos = _madBall.transform.position;
        List<Vector2> hitSpots = new List<Vector2>();

        foreach (Vector2 spot in _field.getHitSpots())
        {
            float distance = _field.getPointsDistance(ballPos, spot);
            if (distance >= minDistance)
            {
                hitSpots.Add(spot);
            }
        }
        Vector2 bestSpot = hitSpots[Random.Range(0, hitSpots.Count - 1)];
        Vector3 target = new Vector3(bestSpot.x, bestSpot.y, 0);
        return target;
    }
    // Charge attack
    public void chargeAttack(float time)
    {
        // Look at target
        lookAtTarget();
        // Play charge animation
        _ballAnimator.SetTrigger("charge");
    }
    // Ball basic attack
    public void standardAttack()
    {
        // Play attack animation
        _ballAnimator.SetTrigger("standard_attack");
        // Attack phisics
        rb.AddForce(target * _ballStats.getForce(), ForceMode.Impulse);
    }
    private void OnParticleCollision(GameObject other)
    {
        collided();
    }
    public void collided()
    {
        //Stop animation
        _ballAnimator.SetTrigger("stop");

        Rigidbody rb = transform.GetComponent<Rigidbody>();
        //rb.AddForce(Vector3.Reflect(rb.position, Vector3.right));
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //StartCoroutine(attackDelay(_ballStats.getAttackDelay()));
    }
    public void ballPhase(Phase phase)
    {
        _pr.Run(phase);
    }
    private void lookAtTarget()
    {
        target = getBestSpot();
        Vector3 direction = (target - _ballTrans.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        _ballTrans.rotation = Quaternion.Slerp(_ballTrans.rotation, lookRotation, Time.deltaTime * RotationSpeed);
    }
}
