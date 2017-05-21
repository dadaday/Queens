using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class QueenScript : MonoBehaviour {
	private QueenAlgo qAlgo;

	private static int MAXQUEENS = 12;
	private static int MINQUEENS = 4;

	public GameObject whiteCellPrefab;
	public GameObject blackCellPrefab;
	public GameObject blueQueenPrefab;
	public Button solButton;

	public Dropdown dropDownNumQ;
	public Slider speedSlider;
	public ScrollRect solutionScroll;

	public int numQueens;

	private int solCount = 0;
	private bool finished = false;
	private bool readyTodisplay = false;
	private float cellWidth;
	private float lastUpdate = 0.0f;
	public float timeBetweenUpdates;

	private GameObject[] queensArr;
	private List<int[]> solArr;

	// Use this for initialization
	void Start () {
		init (numQueens);

		for (int i = MINQUEENS; i <= MAXQUEENS; i++) {
			dropDownNumQ.options.Add (new Dropdown.OptionData (i.ToString()));
		}
		dropDownNumQ.onValueChanged.AddListener (resetAll);

		speedSlider.onValueChanged.AddListener(delegate {changeSpeed(); });
	}

	private void init(int numQ) {
		solCount = 0;
		timeBetweenUpdates = 1.0f / speedSlider.value;
		finished = false;
		readyTodisplay = false;
		cellWidth = 12.0f / numQ;
		qAlgo = new QueenAlgo (numQueens, this);
		queensArr = new GameObject[numQ];
		solArr = new List<int[]> ();

		blueQueenPrefab.transform.localScale = new Vector3(cellWidth * 17.0f,cellWidth * 17.0f, cellWidth * 34.0f);

		for (int i = 0; i < numQ; i++) {
			queensArr[i] = (GameObject) Instantiate	(blueQueenPrefab, 
				new Vector3 ((i+1) * cellWidth, 0, (numQueens - qAlgo.getPositionAtCol(i)) * cellWidth), 
				Quaternion.Euler(-90.0f, 0.0f, 0.0f));		
		}

		drawTable (numQ);
	}

	// Update is called once per frame
	void Update () {
		lastUpdate += Time.deltaTime;

		if (!finished && lastUpdate > timeBetweenUpdates) {
			
			int movedQpos = qAlgo.makeStep ();

			if (movedQpos >= 0) {
				updateQueensPosAtCol (movedQpos);
			}
			else {
				finished = true;
			}

			lastUpdate = 0.0f;
		}
		if (finished && !readyTodisplay) {
			readyTodisplay = true;

			GameObject[] solButtons = GameObject.FindGameObjectsWithTag ("SolButTag");
			foreach(GameObject but in solButtons) {
				but.GetComponent<Button>().interactable = true;
			}

			int jj = 1;
			foreach(int[] solA in solArr) {
				int sol = 0;
				for (int i = 0; i < numQueens; i++) {
					sol = sol*10 + solA [i];				
				}
				Debug.Log ("SOLUTION " + jj + ": " + sol);
				jj++;
			}
		}
	}

	public void addSolution() {
		int[] solutionA = new int[numQueens];
		for (int i = 0; i < numQueens; i++) {
			solutionA [i] = qAlgo.getPositionAtCol (i);
		}
		solArr.Add (solutionA);

		Button button = (Button)Instantiate (solButton);

		button.transform.SetParent (solutionScroll.content.transform, false);
		button.interactable = false;
		button.onClick.AddListener(() => loadSolution(solCount));
		Debug.Log (solCount + "Delegated");
		solCount++;
		button.GetComponentInChildren<Text> ().text = "Solution " + (solCount);
	}

	// sets the queen at column col to given color
	public void setColor(int col, Color color) {
		queensArr[col].GetComponent<MeshRenderer> ().material.SetColor ("_Color", color);
	}

	private void updateQueensPosAtCol(int changedCol) {
		queensArr [changedCol].transform.position =
			new Vector3 ((changedCol+1) * cellWidth, 0, (numQueens - qAlgo.getPositionAtCol(changedCol)) * cellWidth);
	}

	private void loadSolution(int solNo) {
		for (int i = 0; i < numQueens; i++) {
			queensArr [i].transform.position =
				new Vector3 ((i+1) * cellWidth, 0, (numQueens - solArr[solNo - 1][i]) * cellWidth);
		}
	}

	private void drawTable(int num) {
		
		whiteCellPrefab.transform.localScale = new Vector3(cellWidth, 0.5f, cellWidth);
		blackCellPrefab.transform.localScale = new Vector3(cellWidth, 0.5f, cellWidth);

		for (int row = 1; row <= num; ++row) {
			for (int col = 1; col <= num; ++col) {
				if (row % 2 != 0) {
					if (col % 2 == 0)
						Instantiate (whiteCellPrefab, new Vector3 (row * cellWidth, 0, col * cellWidth), Quaternion.identity);
					else {
						Instantiate(blackCellPrefab, new Vector3(row * cellWidth, 0, col * cellWidth), Quaternion.identity);
					}
				} else {
					if (col % 2 == 0) {
						Instantiate (blackCellPrefab, new Vector3 (row * cellWidth, 0, col * cellWidth), Quaternion.identity);
					} else {
						Instantiate (whiteCellPrefab, new Vector3 (row * cellWidth, 0, col * cellWidth), Quaternion.identity);
					}
				}
			}
		}
	}

	private void changeSpeed() {
		timeBetweenUpdates = 1.0f / speedSlider.value;
	}

	private void resetAll(int n) {
		int chN = n + MINQUEENS;

		if (chN == numQueens)
			return;

		numQueens = chN;

		GameObject[] cellObjects = GameObject.FindGameObjectsWithTag ("CellTag");
		foreach(GameObject cell in cellObjects) {
			Destroy (cell);
		}

		foreach(GameObject queen in queensArr) {
			Destroy (queen);
		}

		GameObject[] solButtons = GameObject.FindGameObjectsWithTag ("SolButTag");
		foreach(GameObject but in solButtons) {
			Destroy (but);
		}

		init (chN);
	}
}
