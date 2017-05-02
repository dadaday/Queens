using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class QueenScript : MonoBehaviour {
	private static int MAXQUEENS = 12;
	private static int MINQUEENS = 4;

	public GameObject whiteCellPrefab;
	public GameObject blackCellPrefab;

	public Dropdown dropDownNumQ;

	public int numQueens;
	private float cellWidth;

	// Use this for initialization
	void Start () {

		for (int i = MINQUEENS; i <= MAXQUEENS; i++) {
			dropDownNumQ.options.Add (new Dropdown.OptionData (i.ToString()));
		}
		
		dropDownNumQ.onValueChanged.AddListener (resetAll);
		drawTable (numQueens);
	}

	// Update is called once per frame
	void Update () {

	}

	private void resetAll(int n) {
		int chN = n + MINQUEENS;

		if (chN == numQueens)
			return;

		numQueens = chN;
		GameObject[] cellObjects = GameObject.FindGameObjectsWithTag ("CellTag");
		Debug.Log (cellObjects.Length);

		foreach(GameObject cell in cellObjects) {
			Destroy (cell);
		};

		drawTable (chN);
	}

	private void drawTable(int num) {
		cellWidth = 12.0f / num;
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
}
