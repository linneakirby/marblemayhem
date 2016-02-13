using UnityEngine;
using System.Collections;

public class Gem : MonoBehaviour {
	private GemModel model;		// The model object.
	private int gemType;		// Will determine the color and animation for the model.
	private GameManager gm;		// A pointer to the manager (not needed here, but potentially useful in general).
	public float timeToLive = 10;

	// The Start function is good for initializing objects, but doesn't allow you to pass in parameters.
	// For any initialization that requires input, you'll probably want your own init function. 

	public void init(int gemType, GameManager gm) {
		this.gemType = gemType;
		this.gm = gm;

		var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);	// Create a quad object for holding the gem texture.
		model = modelObject.AddComponent<GemModel>();						// Add a gemModel script to control visuals of the gem.
		model.init(gemType, this);						
	}

	void Update(){
		if (timeToLive < 0) {
			Destroy (this.gameObject);
		} else if (!gm.pause){
			timeToLive -= Time.deltaTime;
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		if (collision.gameObject.tag == "train") {
			Destroy (this.gameObject);
		}
	}
}
