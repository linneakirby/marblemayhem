using UnityEngine;
using System.Collections;

public class Gem : MonoBehaviour {
	private GemModel model;		// The model object.
	private int gemType;		// Will determine the color and animation for the model.
	private GameManager gm;		// A pointer to the manager (not needed here, but potentially useful in general).
	public float timeToLive = 10;
	public Tile tile;

	// The Start function is good for initializing objects, but doesn't allow you to pass in parameters.
	// For any initialization that requires input, you'll probably want your own init function. 

	public void init(int gemType, GameManager gm, Tile t) {
		this.gemType = gemType;
		this.gm = gm;
		this.tile = t;


		var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
		modelObject.tag = "gem";
		MeshCollider mc = modelObject.GetComponent<MeshCollider> ();
		mc.enabled = false;

		BoxCollider bc = modelObject.AddComponent<BoxCollider> ();
		bc.size = new Vector3(.85f, .8f, 0);
		Rigidbody rb = modelObject.AddComponent<Rigidbody> ();
		rb.useGravity = false;
		rb.isKinematic = false;

		model = modelObject.AddComponent<GemModel>();						// Add a gemModel script to control visuals of the gem.
		model.init(gemType, this, modelObject);	

		tile.hasGem = true;
	}

	void Update(){
		if (timeToLive < 0) {
			tile.hasGem = false;
			Destroy (this.gameObject);
		} else if (!gm.pause){
			timeToLive -= Time.deltaTime;
		}
	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "marble") {
			Destroy (this.gameObject);
		}
	}


}
