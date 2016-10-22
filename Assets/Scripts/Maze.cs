using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze {

	private const int WALL = 2;
	private const int FLOOR = 1;
	private const int SPAWN = 3;
	private const int EXIT = 4;

	private const int TOP = 1;
	private const int RIGHT = 2;
	private const int BOTTOM = 3;
	private const int LEFT = 4;

	public int nRows;
	public int nCols;
	private int nVRows;
	private int nVCols;
	private int toVisit;
	private int[,] maze;
	private bool[,] visited;
	private Cell guide;
	private Stack<Cell> pathVisited;
	private Cell[] candidates;

	private struct Cell {
		public int x;
		public int y;
		public bool valid;

		public Cell(int x, int y) {
			this.x = x;
			this.y = y;
			this.valid = true;
		}

		public int vx() {
			return (this.x - 1) / 2;
		}

		public int vy() {
			return (this.y - 1) / 2;
		}
	}

	public Maze (int nRows, int nCols) {
		this.nRows = nRows;
		this.nCols = nCols;
		this.nVRows = (nRows - 1) / 2;
		this.nVCols = (nCols - 1) / 2;
		this.toVisit = this.nVRows * this.nVCols;
		this.maze = new int[nCols, nRows];
		this.visited = new bool[nVCols, nVRows];
		this.pathVisited = new Stack<Cell> ();
		this.candidates = new Cell[4];

		// First fill the maze with walls
		for (int i = 0; i < nRows; i++) {
			for (int j = 0; j < nCols; j++) {
				this.maze [i,j] = WALL;
			}
		}

		// Next we "free" the cells that have odd x and y coordinates
		for (int i = 1; i < nRows; i += 2) {
			for (int j = 1; j < nCols; j += 2) {
				this.maze [i,j] = FLOOR;
			}
		}

		// Randomly pick a cell to be the guide cell. This cell has to be next to the borders
		int whichSide = Random.Range(1, 4); // 1 = top, 2 = right, 3 = bottom, 4 = left

		guide = new Cell ();
		if (whichSide == TOP) {
			this.guide.y = nRows - 2;
			this.guide.x = Random.Range (1, nVCols) * 2 - 1;
		} else if (whichSide == BOTTOM) {
			this.guide.y = 1;
			this.guide.x = Random.Range (1, nVCols) * 2 - 1;
		} else if (whichSide == RIGHT) {
			this.guide.y = Random.Range (1, nVRows) * 2 - 1;
			this.guide.x = nCols - 2;
		} else if (whichSide == RIGHT) {
			this.guide.y = Random.Range (1, nVRows) * 2 - 1;
			this.guide.x = 1;
		}

		this.maze [this.guide.x, this.guide.y] = EXIT;
		this.visited [this.guide.vx (), this.guide.vy ()] = true;
		this.toVisit--;
		this.pathVisited.Push (this.guide);

		while (this.toVisit > 0) {
			Cell next = this.getRandomNextCell ();
			if (next.valid) {
				this.visited [next.vx (), next.vy ()] = true;
				this.toVisit--;

				if (next.x > this.guide.x) {
					this.maze [this.guide.x + 1, this.guide.y] = FLOOR;
				} else if (next.x < this.guide.x) {
					this.maze [this.guide.x - 1, this.guide.y] = FLOOR;
				} else if (next.y > this.guide.y) {
					this.maze [this.guide.x, this.guide.y + 1] = FLOOR;
				} else if (next.y < this.guide.y) {
					this.maze [this.guide.x, this.guide.y - 1] = FLOOR;
				}

				this.pathVisited.Push (next);
				this.guide = next;
			} else {
				this.guide = this.pathVisited.Pop ();
			}
		}
	}

	private Cell getRandomNextCell() {
		int index = 0;

		// If above cell hasn't been visisted add it to candidates
		if (((this.guide.vy() + 1) < this.nVRows) && !this.visited[this.guide.vx(), this.guide.vy() + 1]) {
			this.candidates[index] = new Cell (this.guide.x, this.guide.y + 2);
			index++;
		}
		// If below cell hasn't been visisted add it to candidates
		if (((this.guide.vy() - 1) >= 0) && !this.visited[this.guide.vx(), this.guide.vy() - 1]) {
			this.candidates[index] = new Cell (this.guide.x, this.guide.y - 2);
			index++;
		}
		// If left cell hasn't been visisted add it to candidates
		if (((this.guide.vx() - 1) >= 0) && !this.visited[this.guide.vx() - 1, this.guide.vy()]) {
			this.candidates[index] = new Cell (this.guide.x - 2, this.guide.y);
			index++;
		}
		// If right cell hasn't been visisted add it to candidates
		if (((this.guide.vx() + 1) < this.nVCols) && !this.visited[this.guide.vx() + 1, this.guide.vy()]) {
			this.candidates[index] = new Cell (this.guide.x + 2, this.guide.y);
			index++;
		}

		if (index > 0) {
			return this.candidates[Random.Range (0, index)];
		}

		Cell cell = new Cell();
		cell.valid = false;
		return cell;
	}

	public int[,] getMaze() {
		return this.maze;
	}

}
