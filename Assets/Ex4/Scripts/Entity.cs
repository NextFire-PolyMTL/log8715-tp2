using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Small `enum` used to differenciate easily the entities */
public enum EntityType {
	ETT_NONE = -1,
	ETT_PLNT,
	ETT_PREY,
	ETT_PRED,
}

/* The replacement for the old `Plant`, `Prey` and `Predator` classes
 * Not especially useful but maybe not worth the effort deleting to refactor */
public class Entity : MonoBehaviour {
	[SerializeField] public EntityType type;
	private Lifetime _lifetime;

	void Awake() {
		_lifetime = GetComponent<Lifetime>();
	}

	void Start(){}

	void Update() {
		if (type == EntityType.ETT_PLNT)
			transform.localScale = Vector3.one * _lifetime.GetProgression();
	}
}
