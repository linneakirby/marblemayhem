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
	private Renderer rend;
	private SphereCollider sc;
	private Rigidbody2D rigid;

	public void init(float x, float y, Marble owner, GameObject modelObject) {
		this.owner = owner;
		this.x = x;
		this.y = y;

		transform.parent = owner.transform;					
		transform.localPosition = new Vector3(0,0,-2);		
		name = "Marble Model";									

		rend = GetComponent<Renderer> ();
		rend.material = Resources.Load<Material> ("Material/Marble");

		sc = modelObject.GetComponent<SphereCollider> ();


		/*mat = GetComponent<Renderer>().material;
		mat.renderQueue = 4000;
		mat.mainTexture = Resources.Load<Texture2D>("Textures/marble");	
		mat.color = new Color(1,1,1);											
		mat.shader = Shader.Find ("Transparent/Diffuse");*/
	}

	void Start () {
		clock = 0f;
	}

	void OnMouseUpAsButton(){
		if (owner.cooldown <= 0) {
			rend.material.color = Color.green;
			owner.speed = 3.5f;
			owner.cooldown = 10f;
		}
	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "gem") {
			owner.score++;
		} else if (collision.gameObject.tag == "marble") {
			owner.health--;
			if (owner.health <= 0) {
				print ("Marble destroyed!");
				Destroy (this.gameObject);
			}
		} else if (collision.gameObject.tag == "pit") {
			print ("Marble destroyed!");
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter(Collider other){
		/*if (other.gameObject.tag == "gem") {
			print ("Plus one!");
			owner.score++;
		} else */if (other.gameObject.tag == "marble") {
			owner.health--;
			print ("Marble hit!");
			if (owner.health <= 0) {
				print ("Marble destroyed!");
				Destroy (this.gameObject);
			}
		} /*else if (other.gameObject.tag == "pit") {
			print ("Marble destroyed!");
			Destroy (this.gameObject);
		}*/
	}

	void Update () {
		if (owner.speed > 1.5f) {
			rend.material.color = Color.green;
			owner.speed -= Time.deltaTime;
		} else if (owner.cooldown > 0) {
			rend.material.color = Color.red;
			owner.cooldown -= Time.deltaTime;
		} else {
			rend.material.color = Color.white;
		}
		if (owner.gm.go && !owner.gm.pause) {
			clock = clock + Time.deltaTime;
			transform.eulerAngles = new Vector3 (0, 0, -360 * clock * owner.speed);
		}
	}
}

