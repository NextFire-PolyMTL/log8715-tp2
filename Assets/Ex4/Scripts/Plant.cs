using UnityEngine;

[RequireComponent(typeof(Lifetime))]
public class Plant : MonoBehaviour
{
    private Lifetime _lifetime;
    
    void Awake()
    {
        _lifetime = GetComponent<Lifetime>();
    }
    
    void Update()
    {
        transform.localScale = Vector3.one * _lifetime.GetProgression();
    }
}
