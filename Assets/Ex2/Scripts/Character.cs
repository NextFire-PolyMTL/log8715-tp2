using UnityEngine;
using System;//rajout
using System.Collections.Generic;//rajout


public class Character : MonoBehaviour
{
    private Vector3 _velocity = Vector3.zero;

    private Vector3 _acceleration = Vector3.zero;

    private const float AccelerationMagnitude = 2;

    private const float MaxVelocityMagnitude = 5;

    private const float DamagePerSecond = 50;

    private const float DamageRange = 10;
    public int _height;//
    public int _width;//
    public Grid grid;//
    public Circle[] circles;


    private void Start()
    {
        Grid grid = GameObject.FindObjectOfType<Grid>();
        //Circle[] circles = grid.circles;
        _height = grid.getHeight();
        _width = grid.getWidth();
    }
    private void Update()
    {
        Move();

        
        var nearbyColliders = Physics2D.OverlapCircleAll(transform.position, DamageRange); //changement
        Circle[] nearbyCircles = new Circle[nearbyColliders.Length];
        for (var i = 0; i < nearbyColliders.Length; i++)
        {
            var nearbyCollider = nearbyColliders[i];
            if (nearbyCollider != null && nearbyCollider.TryGetComponent<Circle>(out var circle))//utiliser tag que j'ai créé plutôt ?
            {
                nearbyCircles[i] = circle;
            }
        }



        //Circle[] nearbyCircles = getCircles();

        DamageNearbyShapes(nearbyCircles);
        UpdateAcceleration(nearbyCircles);
    }

    private void Move()
    {
        _velocity += _acceleration * Time.deltaTime;
        if (_velocity.magnitude > MaxVelocityMagnitude)
        {
            _velocity = _velocity.normalized * MaxVelocityMagnitude;
        }
        transform.position += _velocity * Time.deltaTime;
    }

    private void UpdateAcceleration(Circle[] nearbyCircles)
    {
        var direction = Vector3.zero;
        //var nearbyColliders = Physics2D.OverlapCircleAll(transform.position, DamageRange);
        /*
        foreach (var nearbyCollider in nearbyColliders)
        {
            if (nearbyCollider.TryGetComponent<Circle>(out var circle)) //utiliser tag que j'ai créé plutôt ?
            {
                direction += (circle.transform.position - transform.position) * circle.Health;
            }
        }
        */
        /*
        foreach(var nearbyCollider in nearbyColliders)
        {
            if (nearbyCollider.TryGetComponent<Circle>(out var circle))//utiliser tag que j'ai créé plutôt ?
            {
                circle.ReceiveHp(-DamagePerSecond * Time.deltaTime);
            }
        }
        */
        //Vector3 position;
        foreach (var circle in nearbyCircles)
        {
            //position=new Vector3(circle.j+0.5f-_height/2,circle.i+0.5f-_width/2,0);
            direction += (circle.transform.position - transform.position) * circle.Health;
        }
        _acceleration = direction.normalized * AccelerationMagnitude;
    }

    private void DamageNearbyShapes(Circle[] nearbyCircles)
    {
        //var nearbyColliders = Physics2D.OverlapCircleAll(transform.position, DamageRange);

        // Si aucun cercle proche, on retourne a (0,0,0)
        if (nearbyCircles.Length == 0)
        {
            transform.position = Vector3.zero;
        }

        foreach (var circle in nearbyCircles)
        {
            circle.ReceiveHp(-DamagePerSecond * Time.deltaTime);

        }
    }

/*
    private Circle[] getCircles()
    {
        List<Circle> targets = new List<Circle>();
        Circle[] circles = grid.circles;

        for (var i = 0; i < _width; i++)
        {

            for (var j = 0; j < _height; j++)
            {

                if (Math.Sqrt(Math.Pow((i+0.5 -1-_width / 2) - transform.position.x, 2) + Math.Pow((j-0.5 - _height/2) - transform.position.y, 2)) <= DamageRange)
                {
                    targets.Add(circles[i*_height+j]);
                }

            }

        }

        float pos_x=transform.position.x;
        float pos_y=transform.position.y;
        for (var i = pos_x-DamageRange; i < pos_x+DamageRange; i++)
        {

            for (var j = pos_y-DamageRange; j <pos_y+DamageRange; j++)
            {
                if ((i+0.5+_width / 2)<_width && (j+0.5+_height / 2)<_height){
                    double d=i+0.5+_width/2;
                    int indice_i=Math.Truncate(i);
                    targets.Add(circles[i*_height+j]);
                }

            }

        }



        foreach (Circle circle in circles)
        {
            if (Math.Sqrt(Math.Pow(circle.transform.position.x - transform.position.x, 2) + Math.Pow(circle.transform.position.y - transform.position.y, 2)) <= DamageRange)
            {
                targets.Add(circle);
            }
        }



        return targets.ToArray();
    }
    */

}
