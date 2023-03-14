using UnityEngine;
using UnityEngine.Serialization;

public class Circle : MonoBehaviour
{
    [FormerlySerializedAs("I")] [HideInInspector]
    public int i;

    [FormerlySerializedAs("J")] [HideInInspector]
    public int j;

    public float Health { get; private set; }

    private const float BaseHealth = 1000;

    private const float HealingPerSecond = 1;
    private const float HealingRange = 3;
    private Grid grid; //rajout
    //public int id;//rajout
    private Collider2D[] neighboursCircleCollider;/*rajout-> est-ce que je peux me permettre de boussillier la maintenanabilité ?
    car là je considère que les voisins de changent jamais*/
    private Circle[] neighboursCircle;//rajout
    // Start is called before the first frame update
    private void Start()
    {
        Health = BaseHealth;
        grid = GameObject.FindObjectOfType<Grid>();//rajout
        neighboursCircleCollider=Physics2D.OverlapCircleAll(transform.position, HealingRange);//là je considère que les voisins de changent jamais...
        int nb_circles=neighboursCircleCollider.Length;//rajout
        neighboursCircle=new Circle[nb_circles];//rajout
        Collider2D nearbyCollider;//rajout
        for (var i = 0; i < nb_circles; i++)
        {
            nearbyCollider=neighboursCircleCollider[i];
            if (nearbyCollider != null && nearbyCollider.TryGetComponent<Circle>(out var circle))
            {
                //circle.ReceiveHp(HealingPerSecond * Time.deltaTime);
                neighboursCircle[i]=circle;
                //grid.HPReceived[circle.id]+=HealingPerSecond * Time.deltaTime ;//rajout
                //nearbyCollider.gameObject.GetComponent<Circle>().ReceiveHp(HealingPerSecond * Time.deltaTime);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //ReceiveHp(grid.HPReceived[id]);
        UpdateColor();
        HealNearbyShapes();
    }

    private void UpdateColor()
    {
        //var grid = GameObject.FindObjectOfType<Grid>(); //mauvaise façon -> d'autant plus qu'il y a une seule grid...
        //il ne faut pas s'amuser à la chercher à chaque update...
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = grid.Colors[i, j] * Health / BaseHealth;
    }

    private void HealNearbyShapes()
    {
        //var nearbyColliders = Physics2D.OverlapCircleAll(transform.position, HealingRange);
        //foreach (var nearbyCollider in nearbyColliders)
        /*
        foreach (var nearbyCollider in neighboursCircleCollider)
        {
           // Circle circle=nearbyCollider.gameObject.GetComponent<Circle>();
            if (nearbyCollider != null && nearbyCollider.TryGetComponent<Circle>(out var circle))
            //TryGetComponent prend du tps ? -> mettre plutôt nearbyCollider.gameObject.tag=="circle"?? -> mauvaise idée
            //if (nearbyCollider != null &&  nearbyCollider.gameObject.tag=="circle")
            {
                circle.ReceiveHp(HealingPerSecond * Time.deltaTime);
                //grid.HPReceived[circle.id]+=HealingPerSecond * Time.deltaTime ;//rajout
                //nearbyCollider.gameObject.GetComponent<Circle>().ReceiveHp(HealingPerSecond * Time.deltaTime);
            }
        }
        */

        foreach (var circle in neighboursCircle)
        {
            circle.ReceiveHp(HealingPerSecond * Time.deltaTime);
        }
    }

    public void ReceiveHp(float hpReceived)
    {
        Health += hpReceived;
        Health = Mathf.Clamp(Health, 0, BaseHealth);
    }
}
