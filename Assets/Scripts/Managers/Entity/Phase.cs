using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Phase : ScriptableObject
{
    [SerializeField]
    private string id;
    [SerializeField]
    private float duration;
    [SerializeField]
    private bool isMacro;
    [SerializeField]
    private bool isRunning;
    [SerializeField]
    private List<Phase> nextPhases = new List<Phase>();
    [SerializeField]
    private List<Phase> microPhase;

    public string Id { get => id; set => id = value; }
    public float Duration { get => duration; set => duration = value; }
    public bool IsMacro { get => isMacro; set => isMacro = value; }
    public bool IsRunning { get => isRunning; set => isRunning = value; }
    public List<Phase> MicroPhase { get => microPhase; set => microPhase = value; }
    public List<Phase> NextPhases { get => nextPhases; set => nextPhases = value; }

}
