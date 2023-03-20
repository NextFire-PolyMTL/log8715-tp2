using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Circle : MonoBehaviour
{
    [FormerlySerializedAs("I")]
    [HideInInspector]
    public int i;

    [FormerlySerializedAs("J")]
    [HideInInspector]
    public int j;

    private float _health;
    public float Health { get => _health; }

    private const float BaseHealth = 1000;

    private const float HealingPerSecond = 1;
    private const float HealingRange = 3;

    private Grid _grid;
    private SpriteRenderer _spriteRenderer;
    private Circle[] _nearbyCircles;

    // Start is called before the first frame update
    private void Start()
    {
        _health = BaseHealth;

        //Le gameObject d'un cercle se trouve toujours dans la même grille: autant la conserver une bonne fois pour toute dans une variable globale
        _grid = GameObject.FindObjectOfType<Grid>();
        //Le gameObject d'un cerle ne change pas de spriteRendrer: autant le conserver une bonne fois pour toute dans une variable globale
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        //Les disques étant fixes, il n'est pas nécessaire d'actualiser leur voisinage à chaque itération
        var nearbyColliders = Physics2D.OverlapCircleAll(transform.position, HealingRange);
        //On récupère directemment les cercles à partir des colliders prélevés juste au dessus.
        /*Dans le cas où un de ces colliders ne seraient pas attaché à un gameObject avec une composant Circle,
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
        _nearbyCircles = nearbyCirclesList.ToArray();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateColor();
        HealNearbyCircles();
    }

    private void UpdateColor()
    {
        _spriteRenderer.color = _grid.Colors[i, j] * _health / BaseHealth;
    }

    private void HealNearbyCircles()
    {
        //Calcul les HP donnés avant d'itérer sur tous les disques voisins.
        float hpReceived = HealingPerSecond * Time.deltaTime;
        foreach (var circle in _nearbyCircles)
        {
            circle.ReceiveHp(hpReceived);
        }
    }

    public void ReceiveHp(float hpReceived)
    {
        _health = Mathf.Clamp(_health + hpReceived, 0, BaseHealth);
    }
}
