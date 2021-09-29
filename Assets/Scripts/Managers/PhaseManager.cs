using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    [SerializeField]
    private TimeManager _tm;
    [SerializeField]
    private BallBehaviour _ballBehaviour;
    [SerializeField]
    private Animator _ballAnimator;
    public List<Phase> _phases;
    [SerializeField]
    private Phase currentPhase;
    [SerializeField]
    private Phase currentMicroPhase;
    [SerializeField]
    private List<Phase> phaseHistory;
    private int phaseCount;
    [SerializeField]
    private List<Phase> microPhases;


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(phaseTimer(currentPhase));
        microPhases = new List<Phase>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeManager._isPlaying)
        {
            setPhase();
        }
    }

    private void setPhase()
    {
        // TODO: HERE da finire il proseguimento di una fase
        Phase phase;
        // 0: init, 3: attack, 10: idle, 13: attack
        // Set the phase based on time
        if (currentPhase.IsMacro && currentPhase.IsRunning) { // Macro phase
            if (microPhases.Count > 0 && !microPhases[0].IsRunning) {
                print(microPhases.Count + ", " + !microPhases[0].IsRunning);
                currentMicroPhase = microPhases[0];
                phaseManager(microPhases[0]);
                //StartCoroutine(phaseTimer(microPhases[0]));
                //microPhases[0].IsRunning = false;
                microPhases.Remove(microPhases[0]);
            }
        } else if (!currentPhase.IsRunning) {
            if(currentPhase.NextPhases.Count > 0)
            {
                phase = currentPhase.NextPhases[Random.Range(0, currentPhase.NextPhases.Count-1)];
            } else  {
                float time = _tm.getTimer();
                float roundTime = Mathf.RoundToInt(time);
                // Here change the phase
                switch (roundTime)
                {
                    case 1:
                        // Init
                        phaseCount = 0;
                        break;
                    case 3:
                        // Attack
                        phaseCount = 2;
                        break;
                    case 10:
                        // idle
                        phaseCount = 1;
                        break;
                    case 13:
                        // attack
                        phaseCount = 2;
                        break;
                }
                phase = _phases[phaseCount];
            }
            if (phase.IsMacro) {
                microPhases.AddRange(phase.MicroPhase);
            }
            currentPhase = phase;

            if (!phase.Equals(phaseHistory[phaseHistory.Count - 1]))
            {
                print(string.Format("===== {0} phase at time: {1} =====", phase.name, _tm.getTimer().ToString()));
                // Adding phase to phaseHistory
                phaseHistory.Add(phase);
                phaseManager(phase);
            }
        }
    }

    void phaseManager(Phase phase)
    {
        // Phase division
        // Execute phase one by one
        //Avvio la prima fase e da questa viene tolto time delta time
        if (!phase.IsRunning)
        {
            _ballAnimator.SetTrigger(phase.name);
            _ballBehaviour.ballPhase(phase);
            if(phase.name.Equals("base_attack"))
            {
                print("ok");
            }
            StartCoroutine(phaseTimer(phase));
        }
    }
    public IEnumerator phaseTimer(Phase phase)
    {
        //Debug.Log(phase.name + " phase iniziata");
        phase.IsRunning = true;
        yield return new WaitForSeconds(phase.Duration);
        //Debug.Log(phase.name + " phase finita");
        phase.IsRunning = false;
    }
}
