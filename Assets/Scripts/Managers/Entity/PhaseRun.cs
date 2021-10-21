using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseRun : MonoBehaviour
{
    public GameObject madBall;
    private BallBehaviour ballBehaviour;

    private void Start()
    {
        ballBehaviour = madBall.GetComponent<BallBehaviour>();
    }
    public void Run(Phase phase)
    {
        switch (phase.Id)
        {
            case "charge":
                chargeAttack(phase);
                StartCoroutine(attackDelay(phase));
                break;
            case "idle":
                break;
            case "home":
                home(phase);
                break;
        }
    }

    private void chargeAttack(Phase phase)
    {
        if (ballBehaviour.wantAttack)
        {
            ballBehaviour.chargeAttack(phase.Duration);
        }
    }
    IEnumerator attackDelay(Phase phase)
    {
        yield return new WaitForSeconds(phase.Duration);
        ballBehaviour.standardAttack();
    }
    private void home(Phase phase)
    {
        ballBehaviour.transform.position = new Vector3(0, 0, 0);
    }
}

