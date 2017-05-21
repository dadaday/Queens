using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QueenAlgo {
	private QueenScript qScript;

	private int[] queensPositions;
	private int[] currentQpos;
	private int nQueens;

	private int[] rows;
	private int[] diag1;
	private int[] diag2;

	private Stack<int> stk;

	private bool[] isBlue;

	public QueenAlgo(int numberOfQueens, QueenScript qs) {
		qScript = qs;
		nQueens = numberOfQueens;

		queensPositions = new int[numberOfQueens];
		currentQpos = new int[numberOfQueens];
		isBlue = new bool[numberOfQueens];

		rows = new int[numberOfQueens];
		diag1 = new int[2 * numberOfQueens - 1];
		diag2 = new int[2 * numberOfQueens - 1];

		for(int i = 0; i < numberOfQueens; ++i) {
			currentQpos[i] = -1;
			queensPositions[i] = -1;
			rows[i] = 0;
			isBlue [i] = true;
		}

		for(int i = 0; i < 2 * numberOfQueens - 1; ++i) {
			diag1[i] = 0;
			diag2[i] = 0;
		}

		stk = new Stack<int>();
		stk.Push (-1);
	}

	public int getPositionAtCol(int col) {
		return currentQpos[col];
	}

	private void put(int row, int col) {
		queensPositions[col] = row;
		rows[row] = 1;
		diag1[row - col + nQueens - 1] = 1;
		diag2[row + col] = 1;
	}

	private void take(int row, int col) {
		queensPositions[col] = -1;
		rows[row] = 0;
		diag1[row - col + nQueens -1] = 0;
		diag2[row + col] = 0;
	}

	private bool check(int row, int col)
	{
		return rows[row] == 0 && diag1[row - col + nQueens - 1] == 0 && diag2[row + col] == 0;
	}

	public void solve() {
		while(stk.Count != 0)
		{
			if(stk.Count == nQueens + 1)
			{
				Debug.Log("FOUND SOLUTION!");
				Debug.Log(queensPositions);
				stk.Pop();
				take(stk.Peek(), stk.Count - 1);
			}
			else if(stk.Peek() == nQueens)
			{
				stk.Pop();
				if(stk.Count != 0)
				{
					take(stk.Peek(), stk.Count - 1);
				}
			}
			else
			{
				int top = stk.Pop();
				stk.Push(++top);
				if(stk.Peek() < nQueens && check(stk.Peek(), stk.Count - 1))
				{
					put(stk.Peek(), stk.Count - 1);
					stk.Push(-1);
				}
			}
		}
	}

	public int makeStep() {
		int changedCol = -1;
		int colNewRow = -1;

		Debug.Log ("step");
		// if all columns were processed without conflicts the solution is there
		if(stk.Count == nQueens + 1)
		{
			qScript.addSolution ();
			Debug.Log("FOUND SOLUTION!");
			stk.Pop();
			take(stk.Peek(), stk.Count - 1);

			changedCol = (stk.Count - 1);
			colNewRow = -1;
		}
		// if the row for given column exceeds the size of the board
		else if(stk.Peek() == nQueens)
		{
			stk.Pop();
			if(stk.Count != 0)
			{
				take(stk.Peek(), stk.Count - 1);

				changedCol = (stk.Count - 1);
				colNewRow = -1;
			}
		}
		// otherwise just go to the next row for current column
		else
		{
			int top = stk.Pop();
			stk.Push(++top);

			if (stk.Peek () < nQueens) {
				changedCol = (stk.Count - 1);
				colNewRow = stk.Peek ();

				if (check (stk.Peek (), stk.Count - 1)) {
					put (stk.Peek (), stk.Count - 1);
					stk.Push (-1);

					if (!isBlue [changedCol]) {
						isBlue [changedCol] = true;
						qScript.setColor (changedCol, Color.blue);
					}
				} else {
					isBlue [changedCol] = false;
					qScript.setColor (changedCol, Color.red);
				}
			} else {
				changedCol = (stk.Count - 1);
				colNewRow = -1;
				qScript.setColor (changedCol, Color.blue);
			}
		}

		if(changedCol >= 0)
			currentQpos [changedCol] = colNewRow; 
		return  changedCol;
	}
}
