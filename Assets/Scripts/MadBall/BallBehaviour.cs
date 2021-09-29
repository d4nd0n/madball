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
    private Transform _ballTrans;
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
        StartCoroutine(attackDelay(_ballStats.getAttackDelay()));
        isAttaccking = false;
        rb = transform.GetComponent<Rigidbody>();
    }
    void Update()
    {
        velocity = rb.velocity.magnitude;

        if (TimeManager._isPlaying)
        {
            if (isSlowingDown)
            {
                //slowDown(rb);
            }
            //attackBehaviour();
        }
    }

    private void attackBehaviour()
    {
        isAttaccking = false;

        Rigidbody rigidbody = _madBall.GetComponent<Rigidbody>();
        Vector3 ballPos = _madBall.transform.position;

        if (isAttaccking && wantAttack) {
            chargeAttack(_ballStats.getAttackDelay());
        }
        else {
            //target = getBestSpot();
            //isAttaccking = true;
        }
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
    void chargeAttack(float time)
    {
        print("chargeAttack");
        _ballAnimator.SetTrigger("charge");
        // Look at target
        //lookRotation = Quaternion.LookRotation(target);
        //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 0);
        _ballTrans.LookAt(target);
        print("Rotate: " + transform.rotation);
        StartCoroutine(attackDelay(_ballStats.getAttackDelay()));
        standardAttack(getBestSpot());
    }
    // Ball basic attack
    public void standardAttack(Vector3 target)
    {
        // Set target
        this.target = target;
        print("base_Attack");

        isSlowingDown = false;

        // Attack animation
        //_ballAnimator.SetTrigger("standard_attack");
        
        // Attack phisics
        if(_madBall.transform.position != target)
        {
            //_madBall.GetComponent<Rigidbody>().AddForce(target * _ballStats.getForce());
            _madBall.GetComponent<Rigidbody>().AddForce(target * _ballStats.getForce(), ForceMode.Impulse);
            //print("Impulse");
        } else
        {
            foreach(Vector2 spot in _field.getHitSpots())
            {
                if (spot.Equals(target))
                {
                    if (_ballStats.canDie)
                    {
                        //DIE
                        print("===== DIE ======");
                        _tm.Stop();
                        StartCoroutine(attackDelay(_ballStats.getAttackDelay()));
                    } else
                    {
                        // Manage if cannot die
                        _madBall.transform.position = new Vector3(0, 0, 0);
                        //target = getBestSpot();
                    }
                }
            }
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        //Debug.Log("===== Ball collision =====");
        collided();
        //if (rb.velocity.magnitude > minVel)
        //{
        //    collided();
        //}
    }
    IEnumerator attackDelay(float delay)
    {
        //isAttaccking = false;
        yield return new WaitForSeconds(delay);
        //isAttaccking = true;
        _ballStats._isPlaying = true;
    }
    public void collided()
    {
        //Stop animation
        _ballAnimator.SetTrigger("stop");

        Rigidbody rb = transform.GetComponent<Rigidbody>();
        //rb.AddForce(Vector3.Reflect(rb.position, Vector3.right));
        isSlowingDown = true;

        //StartCoroutine(attackDelay(_ballStats.getAttackDelay()));
    }
    private void slowDown(Rigidbody rb)
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            isAttaccking = false;
            rb.velocity = rb.velocity * slowDownRate;
            velocity = rb.velocity.magnitude;
        }
        else {
            isSlowingDown = false;
        }
    }
    public void ballPhase(Phase phase){
        switch (phase.name) {
            case "base_attack":
                chargeAttack(_ballStats.getAttackDelay());
                break;
            case "idle":
                break;
            case "home":
                _madBall.transform.position = new Vector3(0, 0, 0);
                break;
        }

    }
}
