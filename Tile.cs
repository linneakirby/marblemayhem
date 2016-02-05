using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	private TileModel model;		// The model object.
	private int row;
	private int col;
	private int tiletype;
	private GameManager gm;		// A pointer to the manager (not needed here, but potentially useful in general).

	private bool turn = false;

	// The Start function is good for initializing objects, but doesn't allow you to pass in parameters.
	// For any initialization that requires input, you'll probably want your own init function. 

	public void init(float row, float col, int tiletype, GameManager gm) {
		this.row = (int)row;
		this.col = (int)col;
		this.tiletype = tiletype;
		this.gm = gm;

		checkTurn ();

		var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);	// Create a quad object for holding the gem texture.
		model = modelObject.AddComponent<TileModel>();						// Add a MarbleModel script to control visuals of the gem.
		model.init(row, col, tiletype, this);					
	}

	private void checkTurn(){
		if (tiletype == 2) {
			turn = true;
		}
	}

	public bool isTurn(){
		if (turn) {
			return true;
		}
		return false;
	}
}

