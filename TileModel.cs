using UnityEngine;
using System.Collections;

public class TileModel : MonoBehaviour
{
	private int row;
	private int col;
	private int tiletype;
	private float clock;		// Keep track of time since creation for animation.
	private Tile owner;			// Pointer to the parent object.
	private Material mat;		// Material for setting/changing texture and color.

	public void init(float row, float col, int tiletype, Tile owner) {
		this.owner = owner;
		this.row = (int)row;
		this.col = (int)col;
		this.tiletype = tiletype;

		transform.parent = owner.transform;					// Set the model's parent to the gem.
		transform.localPosition = new Vector3(0,0,0);		// Center the model on the parent.
		name = "Tile Model";									// Name the object.

		mat = GetComponent<Renderer>().material;								// Get the material component of this quad object.
		mat.mainTexture = Resources.Load<Texture2D>("Textures/tile"+tiletype);	// Set the texture.  Must be in Resources folder.
		mat.color = new Color(1,1,1);											// Set the color (easy way to tint things).
		mat.shader = Shader.Find ("Transparent/Diffuse");						// Tell the renderer that our textures have transparency. 
	}

	void Start () {
		clock = 0f;
	}

	void Update () {

		// Incrememnt the clock based on how much time has elapsed since the previous update.
		// Using deltaTime is critical for animation and movement, since the time between each call
		// to Update is unpredictable.
		clock = clock + Time.deltaTime;
	}
}

