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
	private Renderer rend;
	private BoxCollider bc;

	public void init(float row, float col, int tiletype, Tile owner, GameObject modelObject) {
		this.owner = owner;
		this.row = (int)row;
		this.col = (int)col;
		this.tiletype = tiletype;

		transform.parent = owner.transform;	
		if (tiletype == 2) {
			transform.localPosition = new Vector3 (0, 0, -1);
		}
		else if (tiletype == 3) {
			transform.localPosition = new Vector3 (0, 0, -2);
			bc = modelObject.GetComponent<BoxCollider> ();
		} else {
			transform.localPosition = new Vector3 (0, 0, 0);
		}
		name = "Tile Model";									

		addTexture ();
	}
		
	private void addTexture(){
		/*mat = GetComponent<Renderer>().material;
		mat.mainTexture = Resources.Load<Texture2D>("Textures/tile"+tiletype);	
		mat.color = new Color(1,1,1);											
		mat.shader = Shader.Find ("Transparent/Diffuse");*/
		rend = GetComponent<Renderer> ();
		rend.material = Resources.Load<Material> ("Material/tile"+tiletype);
	}

	void Start () {
		clock = 0f;
	}

	void Update () {
		clock = clock + Time.deltaTime;
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "marble") {
			print ("MARBLE LOST!");
			Destroy (other.gameObject);
		}
	}
}

