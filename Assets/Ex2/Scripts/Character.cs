using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Vector3 _velocity = Vector3.zero;

    private Vector3 _acceleration = Vector3.zero;

    private const float AccelerationMagnitude = 2;

    private const float MaxVelocityMagnitude = 5;

    private const float DamagePerSecond = 50;

    private const float DamageRange = 10;

    private void Update()
    {
        Move();

        /*Les deux fonction DamageNearbyCircles et UpdateAcceleration utilise toute deux le même voisinage
        On le détermine donc en amont.*/
        var nearbyColliders = Physics2D.OverlapCircleAll(transform.position, DamageRange);
        //On récupère directemment les cercles à partir des colliders prélevés juste au dessus.
        /*Dans le cas où un de ces colliders ne serait pas attaché à un gameObject avec une composant Circle,
        nous avons préféré utiliser une liste que l'on transforme ensuite en array pour une plus grande performance
        du fait d'un accès contigu des valeurs en mémoire.*/
        var nearbyCirclesList = new List<Circle>();
        for (var i = 0; i < nearbyColliders.Length; i++)
        {
            var nearbyCollider = nearbyColliders[i];
            if (nearbyCollider != null && nearbyCollider.TryGetComponent<Circle>(out var circle))
            {
                nearbyCirclesList.Add(circle);
            }
        }
        var nearbyCircles = nearbyCirclesList.ToArray();

        DamageNearbyCircles(nearbyCircles);
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
        foreach (var circle in nearbyCircles)
        {
            direction += (circle.transform.position - transform.position) * circle.Health;
        }
        _acceleration = direction.normalized * AccelerationMagnitude;
    }

    private void DamageNearbyCircles(Circle[] nearbyCircles)
    {
        // Si aucun cercle proche, on retourne a (0,0,0)
        if (nearbyCircles.Length == 0)
        {
            transform.position = Vector3.zero;
        }

        //Calcul des HPs enlevés avant d'itérer sur tous les cercles voisins.
        float hpReceived = -DamagePerSecond * Time.deltaTime;
        foreach (var circle in nearbyCircles)
        {
            circle.ReceiveHp(hpReceived);
        }
    }
}
