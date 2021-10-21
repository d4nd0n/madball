using UnityEngine;

public class CursorTrail : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _trails;
    [SerializeField]
    private ParticleSystem[] _particleSystems;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private float _emissionRate;
    [SerializeField]
    private float _stamina;
    [SerializeField]
    private bool _isPc;
    private float distance;
    private float defStamina;
    private Touch touch;


    // Start is called before the first frame update
    void Start()
    {
        distance = Vector3.Distance(_camera.transform.position, _trails[0].transform.position);
        defStamina = _stamina;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        // The trail follows the movement of the mouse 
        foreach (GameObject trail in _trails)
        {
            trail.transform.position = _camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, distance));
        }
        // Manage platform
        if (_isPc)
        {
            // Use mouse input
            if (Input.GetMouseButton(0))
            {
                // When i click mouse zero active particle system
                foreach (ParticleSystem ps in _particleSystems)
                {
                    if (_stamina > 0)
                    {
                        var emission = ps.emission;
                        emission.rateOverDistance = _emissionRate;
                        emission.rateOverTime = _emissionRate;
                    }
                    else
                    {
                        var emission = ps.emission;
                        emission.rateOverDistance = 0.0f;
                        emission.rateOverTime = 0.0f;
                    }
                    _stamina = descreaseStamina(0.5f);
                }
            }
            else
            {
                if (_stamina <= defStamina)
                {
                    _stamina = _stamina + Time.deltaTime;
                }
                foreach (ParticleSystem ps in _particleSystems)
                {
                    var emission = ps.emission;
                    emission.rateOverDistance = 0.0f;
                    emission.rateOverTime = 0.0f;
                }
            }
        }
        else
        {
            // Use touch input
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    foreach (ParticleSystem ps in _particleSystems)
                    {
                        if (_stamina > 0)
                        {
                            var emission = ps.emission;
                            emission.rateOverDistance = _emissionRate;
                            emission.rateOverTime = _emissionRate;
                        }
                        else
                        {
                            var emission = ps.emission;
                            emission.rateOverDistance = 0.0f;
                            emission.rateOverTime = 0.0f;
                        }
                        _stamina = descreaseStamina(0.5f);
                    }
                }
            }
            else
            {
                if (_stamina <= defStamina)
                {
                    _stamina = _stamina + Time.deltaTime;
                }
                foreach (ParticleSystem ps in _particleSystems)
                {
                    var emission = ps.emission;
                    emission.rateOverDistance = 0.0f;
                    emission.rateOverTime = 0.0f;
                }
            }
        }
    }
    private float descreaseStamina(float rate)
    {
        float stamina = 0;
        if (_stamina > 0)
        {
            stamina = _stamina - rate * Time.deltaTime;
        }
        return stamina;
    }
}


