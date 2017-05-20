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

	public Dropdown dropDownNumQ;

	public int numQueens;

	private float cellWidth;
	private float lastUpdate = 0.0f;
	public float timeBetweenUpdates = 1.0f;

	private GameObject[] queensArr;

	// Use this for initialization
	void Start () {
		for (int i = MINQUEENS; i <= MAXQUEENS; i++) {
			dropDownNumQ.options.Add (new Dropdown.OptionData (i.ToString()));
		}
		
		dropDownNumQ.onValueChanged.AddListener (resetAll);
		init (numQueens);
	}

	private void init(int numQ) {
		cellWidth = 12.0f / numQ;
		qAlgo = new QueenAlgo (numQueens);
		queensArr = new GameObject[numQ];

		for (int i = 0; i < numQ; i++) {
			queensArr[i] = (GameObject) Instantiate	(blueQueenPrefab, 
				new Vector3 ((i+1) * cellWidth, 0, (numQueens - qAlgo.getPositionAtCol(i)) * cellWidth), 
				Quaternion.Euler(-90.0f, 0.0f, 0.0f));		
		}

		drawTable (numQ);
	}

	// Update is called once per frame
	void Update () {
		if (Time.time - lastUpdate > timeBetweenUpdates) {
			int movedQpos = qAlgo.makeStep ();
			Debug.Log (movedQpos + " was moved");
			updateQueensPos (movedQpos);
			lastUpdate = Time.time;
		}
	}

	private void updateQueensPos(int mqp) {
		queensArr [mqp].transform.position =
			new Vector3 ((mqp+1) * cellWidth, 0, (numQueens - qAlgo.getPositionAtCol(mqp)) * cellWidth);
	}

	private void drawTable(int num) {
		
		whiteCellPrefab.transform.localScale = new Vector3(cellWidth, 0.5f, cellWidth);
		blackCellPrefab.transform.localScale = new Vector3(cellWidth, 0.5f, cellWidth);

		blueQueenPrefab.transform.localScale = new Vector3(cellWidth * 17.0f, cellWidth * 17.0f, cellWidth * 34.0f);

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

	private void resetAll(int n) {
		int chN = n + MINQUEENS;

		if (chN == numQueens)
			return;

		numQueens = chN;
		GameObject[] cellObjects = GameObject.FindGameObjectsWithTag ("CellTag");

		foreach(GameObject cell in cellObjects) {
			Destroy (cell);
		};
		init (chN);
	}
}
