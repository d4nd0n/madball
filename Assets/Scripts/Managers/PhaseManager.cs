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
        microPhases = new List<Phase>();
        float duration = 0;
        // Run init behaviour of phases
        foreach (Phase phase in _phases)
        {
            phase.IsRunning = false;
            duration = 0;
            if (phase.IsMacro)
            {
                foreach (Phase micro in phase.MicroPhase)
                {
                    duration = duration + micro.Duration;
                }
                phase.Duration = duration;
            }
        }
        phaseRunById("init");
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
        if (currentPhase.IsRunning)
        { // Macro phase
            print(currentPhase.Id);
            // This manage the microphases into Macro phase like a Pila
            if (microPhases.Count > 0 && !microPhases[0].IsRunning)
            { // Micro Phase
                phaseRun(microPhases[0]);
                microPhases.Remove(microPhases[0]);
            }
        }
        else
        { // Manage the selection of a new phase
            if (currentPhase.NextPhases.Count > 0)
            {
                // Choose randomly the next phase from NextPhases list
                phase = currentPhase.NextPhases[Random.Range(0, currentPhase.NextPhases.Count - 1)];
                if (phase.IsMacro)
                {
                    microPhases.AddRange(phase.MicroPhase);
                }
                if (!phase.Equals(phaseHistory[phaseHistory.Count - 1]))
                {
                    // Adding phase to phaseHistory
                    phaseHistory.Add(phase);
                    phaseRun(phase);
                }
            }
        }
    }
    private void phaseRun(Phase phase)
    {
        Logger.logWithTime(string.Format("Started phase: {0}", phase.Id));
        foreach(Phase p in phase.MicroPhase)
        {
            Logger.logWithTime(string.Format("Have phase: {0}", p.Id));
        }
        StartCoroutine(phaseTimer(phase));
        _ballBehaviour.ballPhase(phase);
        if (phase.IsMacro)
        {
            currentPhase = phase;
        }
        else
        {
            currentMicroPhase = phase;
        }
    }
    public void phaseRunById(string id)
    {
        foreach (Phase phase in _phases)
        {
            if (phase.Id == id)
            {
                phaseRun(phase);
                break;
            }
        }
    }
    public IEnumerator phaseTimer(Phase phase)
    {
        // Start a timer for the duration of the phase
        phase.IsRunning = true;
        yield return new WaitForSeconds(phase.Duration);
        phase.IsRunning = false;
    }
}
