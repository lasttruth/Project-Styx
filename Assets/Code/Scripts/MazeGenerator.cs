using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Range(5, 500)]
    public int mazeWidth = 5, mazeHeight = 5;       // The dimension of the maze.
    public int startX, startY;                      // The position our algorithm will start from.
    MazeCell[,] maze;                               // An array of maze cells representing the maze grid.

    Vector2Int currentCell;                         // The maze cell we are currently looking at.

    private void Start()
    {
        maze = new MazeCell[mazeWidth, mazeHeight];

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                maze[x, y] = new MazeCell(x, y);
            }
        }
    }
List<Direction> directions = new List<Direction> { 

    Direction.Up, Direction.Left, Direction.Right, Direction.Down
};

    List<Direction> GetRandomDirections()
    {
        List<Direction> dir = new List<Direction>(directions);

        List<Direction> rndDir = new List<Direction>();

        while (dir.Count > 0)                       // Loop until our rndDir list is empty.
        {
            int rnd = Random.Range(0, dir.Count);   // Get random index in list.
            rndDir.Add(dir[rnd]);                   // add the random direction to ur list.
            dir.RemoveAt(rnd);                      // remove that direction so we can't choose it again
        }

        // When we've got all four directions in a random order, return the queue.
        return rndDir;
    }

    bool IsCellValid(int x, int y)
    {
        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1 || maze[x,y].visited)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    Vector2Int CheckNeighbor()
    {
        List<Direction> rndDir = GetRandomDirections();

        for (int i = 0; i < rndDir.Count; i++)
        {
            Vector2Int neighbor = currentCell;

        switch (rndDir[i])
        {
            case Direction.Up:
                neighbor.y++;
                break;
            case Direction.Down:
                neighbor.y--;
                break;
            case Direction.Right:
                neighbor.x++;
                break;
            case Direction.Left:
                neighbor.x--;
                break;
            }

            if (IsCellValid(neighbor.x, neighbor.y))
            {
                return neighbor;
            }
        }

        return currentCell;
    }

    void breakWalls(Vector2Int primaryCell, Vector2Int secondaryCell)
    {
        if(primaryCell.x > secondaryCell.x)
        {
            maze[primaryCell.x, primaryCell.y].leftWall = false;
        }
        else if(primaryCell.x < secondaryCell.x)
        {
            maze[secondaryCell.x, secondaryCell.x].leftWall = false;
        }
        else if (primaryCell.y < secondaryCell.y)
        {
            maze[primaryCell.y, primaryCell.y].topWall = false;
        }
        else if (primaryCell.y > secondaryCell.y)
        {
            maze[secondaryCell.y, secondaryCell.y].topWall = false;
        }
    }

    // Starting at the x, y passed in carves a path through the maze until it encounters a "dead end"
    // (dead end meaning is a cell with no valid neighbors 
    void carvePath(int x, int y)
    {
        //Perform a quick check to make sure our start position is within the boundaries of the map,
        //if not, set them to a default(I'm using 0) and throw a little warning Up.
        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1 || maze[x, y].visited)
        {
            x = y = 0;
            Debug.LogWarning("starting position is out of bounds, defaulting to 0, 0");
        }

        // Set current cell to the starting position we were passed.
        currentCell = new Vector2Int(x, y);

        // A list to Keep track of current path.
        List<Vector2Int> path = new List<Vector2Int>();
    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class MazeCell
{
    public bool visited;
    public int x, y;

    public bool topWall;
    public bool leftWall;

    //return x and y as a vector2Int for conveience sake.
    public Vector2Int position
    {
        get
        {
            return new Vector2Int(x, y);
        }
    }

    public MazeCell (int x, int y)
    {
        //The coordinates of this cell in the maze grid.
        this.x = x;
        this.y = y;

        visited = false;
        // All walls are present until the algorithm removes them.
        topWall = leftWall = true;
    }
}
