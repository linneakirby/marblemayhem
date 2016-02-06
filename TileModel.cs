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

		transform.parent = owner.transform;					
		transform.localPosition = new Vector3(0,0,0);		
		name = "Tile Model";									

		addTexture ();
	}
		
	private void addTexture(){
		mat = GetComponent<Renderer>().material;
		mat.mainTexture = Resources.Load<Texture2D>("Textures/tile"+tiletype);	
		mat.color = new Color(1,1,1);											
		mat.shader = Shader.Find ("Transparent/Diffuse");
	}

	void Start () {
		clock = 0f;
	}

	void Update () {
		clock = clock + Time.deltaTime;
	}
}

