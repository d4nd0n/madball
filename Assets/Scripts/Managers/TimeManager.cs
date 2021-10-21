using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField]
    private float _startTime;
    [SerializeField]
    private GameObject _ball;
    [SerializeField]
    private GameObject _panel;
    [SerializeField]
    private Text _startTimer;
    [SerializeField]
    private Text _timerText;
    [SerializeField]
    private Text _timerTextMin;
    [SerializeField]
    private bool isActive;
    [SerializeField]
    private GameObject _pm;

    public static float timer;
    public static bool _isPlaying;

    // Start is called before the first frame update
    void Start()
    {
        // Set timer to zero 
        timer = 0;
        _isPlaying = false;
        isActive = false;
        setUi(true);
        // Set phase manager to false when timer not run, managed in execution order
        _pm.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            timer = timerManager();
            if (_isPlaying)
            { //Play
                string[] t = (timer - _startTime).ToString().Split(',');
                _timerText.text = t[0];
                if (t[1] != null)
                {
                    _timerTextMin.text = t[1].Substring(0, 2);
                }
            }
            else
            { //Caricamento iniziale
                setUi(true);
            }
        }

    }

    public void Play()
    {
        timer = 0;
        _isPlaying = true;
        isActive = true;
        setUi(false);
        _pm.SetActive(true);
    }

    public void Stop()
    {
        _isPlaying = false;
        isActive = false;
        setUi(true);
        timer = 0;
        _pm.SetActive(false);
    }

    public float timerManager()
    {
        timer += Time.deltaTime;
        return timer;
    }

    void setUi(bool active)
    {
        // Initialize Ui
        if (active == true)
        {
            _panel.SetActive(true);
            _ball.SetActive(false);
            _timerText.enabled = false;
            _timerTextMin.enabled = false;
        }
        else
        {
            _panel.SetActive(false);
            _ball.SetActive(true);
            _timerText.enabled = true;
            _timerTextMin.enabled = true;
        }

    }
    public static float getTimer() { return timer; }
}
