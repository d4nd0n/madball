using System.Collections;
using UnityEngine;

public class BallStats : MonoBehaviour
{
    [SerializeField]
    private float force;
    [SerializeField]
    private float stamina;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float attackDelay;
    [SerializeField]
    private TimeManager _tm;

    public bool canDie;

    private BallBehaviour ballBehaviour;
    private Rigidbody rb;
    private float defForce;
    private float defstamina;
    private float defspeed;
    private float defattackDelay;
    public bool _isPlaying;

    public void Start()
    {
        ballBehaviour = transform.GetComponent<BallBehaviour>();

        StartCoroutine(increaseStatsByTime());
        defForce = force;
        defstamina = stamina;
        defspeed = speed;
        defattackDelay = attackDelay;
        rb = transform.GetComponent<Rigidbody>();
    }

    public float getForce()
    {
        return force;
    }
    public float getStamina()
    {
        return stamina;
    }
    public float getSpeed()
    {
        return speed;
    }
    public float getAttackDelay()
    {
        return attackDelay;
    }

    public void restart()
    {
        if (canDie)
        {
            _isPlaying = false;
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(0, 0, 0);
            ballBehaviour.target = ballBehaviour.getBestSpot();
            force = defForce;
            stamina = defstamina;
            speed = defspeed;
            attackDelay = defattackDelay;
            _tm.Stop();
        }
    }

    IEnumerator increaseStatsByTime()
    {
        yield return new WaitForSeconds(20f);
        print("INCREASE STATS NOW");

        //Setting caps
        if (speed <= 10f)
        {
            speed *= 1.2f;
        }
        if (attackDelay >= 0.5)
        {
            attackDelay = attackDelay / 2;
        }
        if (force <= 1f)
        {
            force = force + 0.05f;
        }
        if (_isPlaying)
        {
            StartCoroutine(increaseStatsByTime());
        }
    }


}
