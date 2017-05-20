using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QueenAlgo {

	private int[] queensPositions;
	private int nQueens;

	private int[] rows;
	private int[] diag1;
	private int[] diag2;

	private int numberOfSolutions;
	Stack<int> stk;

	public QueenAlgo(int numberOfQueens) {
		nQueens = numberOfQueens;

		queensPositions = new int[numberOfQueens];

		rows = new int[numberOfQueens];
		diag1 = new int[2 * numberOfQueens - 1];
		diag2 = new int[2 * numberOfQueens - 1];

		for(int i = 0; i < numberOfQueens; ++i) {
			queensPositions[i] = -1;
			rows[i] = 0;
		}

		for(int i = 0; i < 2 * numberOfQueens - 1; ++i) {
			diag1[i] = 0;
			diag2[i] = 0;
		}

		stk = new Stack<int>();
		stk.Push (-1);
	}

	public int getPositionAtCol(int col) {
		return queensPositions [col];
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
				++numberOfSolutions;
				Debug.Log("FOUND SOLUTION!");
				return;
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




	IEnumerator puti(int row, int col) {
		yield return new WaitForSeconds (1.0f);
		queensPositions[col] = row;
		rows[row] = 1;
		diag1[row - col + nQueens - 1] = 1;
		diag2[row + col] = 1;
	}

	IEnumerator takei(int row, int col) {
		yield return new WaitForSeconds (1.0f);
		queensPositions[col] = -1;
		rows[row] = 0;
		diag1[row - col + nQueens -1] = 0;
		diag2[row + col] = 0;
	}

	public IEnumerator solvei(int speed) {
		while(stk.Count != 0)
		{
			yield return new WaitForSeconds(1.0f);
			if(stk.Count == nQueens + 1)
			{
				++numberOfSolutions;
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
		Debug.Log ("step");
		if(stk.Count == nQueens + 1)
		{
			++numberOfSolutions;
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
		return  stk.Count - 2;
	}
}
