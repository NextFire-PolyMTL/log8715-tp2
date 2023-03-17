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

        _grid = GameObject.FindObjectOfType<Grid>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        var nearbyColliders = Physics2D.OverlapCircleAll(transform.position, HealingRange);
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
        HealNearbyShapes();
    }

    private void UpdateColor()
    {
        _spriteRenderer.color = _grid.Colors[i, j] * _health / BaseHealth;
    }

    private void HealNearbyShapes()
    {
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
