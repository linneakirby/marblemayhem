using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public Vector2 dirN { get { return new Vector2(0, 1); } }
	public Vector2 dirS { get { return new Vector2(0, -1); } }
	public Vector2 dirE { get { return new Vector2(1, 0); } }
	public Vector2 dirW { get { return new Vector2(-1, 0); } }

	public bool N = false;
	public bool S = false;
	public bool E = false;
	public bool W = false;

	private Square square;


	private TileModel model1;	
	private TileModel model2;
	private float x;
	private float y;
	private int tiletype;
	private GameManager gm;		

	private bool turn = false;

	public ArrayList marbles = new ArrayList();

	void Update(){
		if (Input.GetMouseButtonUp(0) && isTurn() && !hasMarble()) {
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			float mouseX = worldPos.x;
			float mouseY = worldPos.y;
			if (mouseX >= x-.5f && mouseX <= x+.5f && mouseY >= y-.5f && mouseY <= y+.5f) {
				rotateR ();
			}
		}
		if (Input.GetMouseButtonUp(1) && isTurn() && !hasMarble()) {
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			float mouseX = worldPos.x;
			float mouseY = worldPos.y;
			if (mouseX >= x-.5f && mouseX <= x+.5f && mouseY >= y-.5f && mouseY <= y+.5f) {
				rotateL ();
			}
		}
	}

	public void init(int tiletype, Square s, GameManager gm) {
		this.x = s.x;
		this.y = s.y;
		this.tiletype = tiletype;
		this.gm = gm;
		this.square = s;

		checkTurn ();

		if (isTurn ()) {
			var modelObject2 = GameObject.CreatePrimitive (PrimitiveType.Quad);

			model2 = modelObject2.AddComponent<TileModel> ();
			model2.init (x, y, 2, this);
		}
		addEmptyTexture ();
	}

	private bool hasMarble(){
		if (marbles.Count > 0) {
			return true;
		}
		return false;
	}

	private void addEmptyTexture(){
		var modelObject1 = GameObject.CreatePrimitive (PrimitiveType.Quad);	
		model1 = modelObject1.AddComponent<TileModel> ();						
		model1.init (x, y, 1, this);
	}

	private void checkTurn(){
		if (tiletype == 2) {
			turn = true;
			S = true;
			E = true;
		}
	}

	public bool isTurn(){
		if (turn) {
			return true;
		}
		return false;
	}

	private void rotateR(){
		if (S && E) {
			E = false;
			W = true;
		} else if (S && W) {
			S = false;
			N = true;
		} else if (N && W) {
			W = false;
			E = true;
		} else {
			N = false;
			S = true;
		}
		this.transform.Rotate (0, 0, -90);
	}

	private void rotateL(){
		if (S && E) {
			S = false;
			N = true;
		} else if (N && E) {
			E = false;
			W = true;
		} else if (N && W) {
			N = false;
			S = true;
		} else {
			W = false;
			E = true;
		}
		this.transform.Rotate (0, 0, 90);
	}

	public Vector2 getNewDirection (Vector2 direction){
		if (direction.Equals (dirN) && S) {
			return checkEW ();
		} else if (direction.Equals (dirS) && N) {
			return checkEW ();
		} else if (direction.Equals (dirE) && W) {
			return checkNS ();
		} else if (direction.Equals (dirW) && E) {
			return checkNS ();
		} else {
			return direction;
		}
	}

	private Vector2 checkNS(){
		if (N) {
			return dirN;
		}
		return dirS;
	}

	private Vector2 checkEW(){
		if (E) {
			return dirE;
		}
		return dirW;
	}
}

