using UnityEngine;
using System.Collections;

public class Marble : MonoBehaviour {

	private MarbleModel model;		// The model object.
	private int row;
	private int col;
	private GameManager gm;		// A pointer to the manager (not needed here, but potentially useful in general).

	// The Start function is good for initializing objects, but doesn't allow you to pass in parameters.
	// For any initialization that requires input, you'll probably want your own init function. 

	public void init(float row, float col, GameManager gm) {
		this.row = (int)row;
		this.col = (int)col;
		this.gm = gm;

		var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);	// Create a quad object for holding the gem texture.
		model = modelObject.AddComponent<MarbleModel>();						// Add a MarbleModel script to control visuals of the gem.
		model.init(row, col, this);						
	}
}
