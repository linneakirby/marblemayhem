using UnityEngine;
using System.Collections;

public class MarbleModel : MonoBehaviour
{
	public Vector2 dirN { get { return new Vector2(0, 1); } }
	public Vector2 dirS { get { return new Vector2(0, -1); } }
	public Vector2 dirE { get { return new Vector2(1, 0); } }
	public Vector2 dirW { get { return new Vector2(-1, 0); } }

	private float x;
	private float y;
	private float clock;		
	private Marble owner;		
	private Material mat;		

	public void init(float x, float y, Marble owner) {
		this.owner = owner;
		this.x = x;
		this.y = y;

		transform.parent = owner.transform;					
		transform.localPosition = new Vector3(0,0,0);		
		name = "Marble Model";									

		//TODO - fix render queue!
		mat = GetComponent<Renderer>().material;	
		mat.renderQueue = 4000;
		mat.mainTexture = Resources.Load<Texture2D>("Textures/marble");	
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

