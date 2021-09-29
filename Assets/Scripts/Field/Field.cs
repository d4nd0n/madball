using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
    [SerializeField]
    private Transform _field;
    [SerializeField]
    private GameObject _ball;
    [SerializeField]
    private Camera _backgroundCamera;
    [SerializeField]
    private GameObject _background;
    [SerializeField]
    private BoxCollider _boxCollider;
    [SerializeField]
    private float height;
    [SerializeField]
    private float width;
    [SerializeField]
    private float depth;

    private Rect hitRect;
    private List<Vector2> hitSpots;
    private List<Vector2> hitArea;

    // Start is called before the first frame update
    void Start()
    {
        setHitRect();
        setHitSpots();
        setHitArea();
        _boxCollider.size = hitRect.size; new Vector3(hitRect.x, hitRect.y, depth);
        _backgroundCamera.orthographicSize = hitRect.size.y/2;
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getPointsDistance(Vector2 a, Vector2 b)
    {
        float distance = Mathf.Sqrt((Mathf.Pow((b.x - a.x), 2) + (Mathf.Pow((b.y - a.y), 2))));
        return distance;
    }

    private void setHitRect()
    {
        Rect rect = new Rect();
        rect.center = _field.position;
        rect.height = height;
        rect.width = width;
        this.hitRect = rect;
    }

    public List<Vector2> getHitSpots() { 
        return this.hitSpots;
    }

    public void setHitSpots()
    {
        List<Vector2> hitSpots = new List<Vector2>();
        Vector2 upLeft = new Vector2(-hitRect.width/2, hitRect.height/2);
        Vector2 upRight = new Vector2(hitRect.width/2, hitRect.height/2);
        Vector2 bottomRight = new Vector2(hitRect.width/2, -hitRect.height/2);
        Vector2 bottomLeft = new Vector2(-hitRect.width/2, -hitRect.height/2);

        //Setting field perimeter
        float approximation = 0.1f;
        // upLeft - upRight
        for (float x = 0; x <= hitRect.width; x += approximation)
        {
            Vector2 offset = new Vector2(upLeft.x + x, upLeft.y);
            hitSpots.Add(offset);
        }
        // upLeft - bottomLeft
        for (float y = 0; y <= hitRect.height; y += approximation)
        {
            Vector2 offset = new Vector2(upLeft.x, upLeft.y - y);
            hitSpots.Add(offset);
        }
        // upRight - bottomRight
        for (float y = 0; y <= hitRect.height; y += approximation)
        {
            Vector2 offset = new Vector2(bottomRight.x, upRight.y - y);
            hitSpots.Add(offset);
        }
        // bottomLeft - bottomRight
        for (float x = 0; x <= hitRect.width; x += approximation)
        {
            Vector2 offset = new Vector2(bottomLeft.x + x, bottomLeft.y);
            hitSpots.Add(offset);
        }
        this.hitSpots = hitSpots;
    }
    public void setHitArea()
    {
        List<Vector2> hitSpots = new List<Vector2>();

        Vector2 upLeft = new Vector2(-hitRect.width / 2, hitRect.height / 2);
        Vector2 upRight = new Vector2(hitRect.width / 2, hitRect.height / 2);

        //Setting field Area
        float approximation = 0.1f;

        // upLeft - upRight 
        for (float y = 0; y <= hitRect.height; y += approximation)
        {
            for (float x = 0; x <= hitRect.width; x += approximation)
            {
                Vector2 offset = new Vector2(upLeft.x + x, upLeft.y - y);
                hitSpots.Add(offset);
            }
        }
        this.hitArea = hitSpots;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            if (_ball.GetComponent<BallStats>().canDie)
            {
                _ball.GetComponent<BallStats>().restart();
            } else
            {
                _ball.GetComponent<BallBehaviour>().collided();
                _ball.GetComponent<BallBehaviour>().target = new Vector3(0, 0, 0);
            }
        }
    }
}
