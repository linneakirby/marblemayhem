using UnityEngine;
using System.Collections;

public class GemModel : MonoBehaviour
{
	private int gemType;		
	private float clock;		// Keep track of time since creation for animation.
	private Gem owner;			// Pointer to the parent object.
	private Material mat;		// Material for setting/changing texture and color.
	private Renderer rend;
	private BoxCollider bc;

	public void init(int gemType, Gem owner, GameObject modelObject) {
		this.owner = owner;
		this.gemType = gemType;

		transform.parent = owner.transform;					// Set the model's parent to the gem.
		transform.localPosition = new Vector3 (0, 0, -2);		// Center the model on the parent.
		name = "Gem Model";									// Name the object.


		rend = GetComponent<Renderer> ();
		rend.material = Resources.Load<Material> ("Material/gem" + gemType);

		bc = modelObject.GetComponent<BoxCollider> ();
	}
		
	void Start () {
		clock = 0f;
	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "marble") {
			owner.tile.hasGem = false;
			Destroy (this.gameObject);
		}
	}

	void Update () {
		
		// Incrememnt the clock based on how much time has elapsed since the previous update.
		// Using deltaTime is critical for animation and movement, since the time between each call
		// to Update is unpredictable.
		clock = clock + Time.deltaTime;
	}
}

