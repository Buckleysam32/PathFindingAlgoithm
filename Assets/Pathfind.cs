using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using UnityEngine.tvOS;

public class Node
{
    public enum State
    {
        None,
        Open,
        Closed
    }
    public int F = 0;   // Total estimated path length. F = G + h
    public int G = 0;   // Distance travelled so far.
    public int H = 0;   // Estimated distance remaining to target.
    public int C = 1;   // Cost of walking over this node.
    public bool Wall = false;   // Walls block movement.
    public Vector2Int Parent = new Vector2Int(-1,-1);   // The node before this one.
    public State state = State.None;    // Current node state. Could be none (not reached yet), Open (possible next node) and Closed (reached the node).
}


public class Pathfind : MonoBehaviour
{
    public static int GridWidth = 16;
    public static int GridHeight = 16;
    public static float CellSize = 1.0f;

    public static Node[,] Nodes;
    public Material groundMat;
    static Texture2D tex;

    // Start is called before the first frame update
    void Start()
    {
        string[] mapData = { 
            "****************",
            "*.......*.*....*",
            "*..........*...*",
            "*..............*",
            "*.....****.....*",
            "*.....****.....*",
            "*.....****.....*",
            "*.....****.....*",
            "*.....****.....*",
            "*.....****.....*",
            "*..............*",
            "*.....*****....*",
            "*.....*****....*",
            "*..............*",
            "*..............*",
            "****************",
        };

        tex = new Texture2D(GridWidth, GridHeight);
        tex.filterMode = FilterMode.Point;
        groundMat.SetTexture("_MainTex", tex);

        Nodes = new Node[GridHeight, GridWidth];

        for (int y = 0; y < GridHeight; ++y)
        {
            for (int x = 0; x < GridWidth; ++x)
            {
                Nodes[GridHeight-y-1, x] = new Node();
                if (mapData[y][x] == '*')
                {
                    Nodes[GridHeight-y-1, x].Wall = true;
                    tex.SetPixel(x, GridHeight-y-1, Color.red);
                }
                else
                {
                    Nodes[GridHeight-y-1, x].Wall = false;
                    tex.SetPixel(x, GridHeight-y-1, Color.black);
                }
            }
        }
        tex.Apply();
    }

    // Update is called once per frame
    void Update()
    {
        for (int y = 0; y <= GridHeight; ++y)
        {
            Debug.DrawLine(new Vector3(0, 0.1f, y), new Vector3(GridWidth, 0.1f, y));
        }
        for (int x = 0; x <= GridWidth; ++x)
        {
            Debug.DrawLine(new Vector3(x, 0.1f, 0), new Vector3(x, 0.1f, GridHeight));
        }
    }

    public static Node GetNode(Vector2Int pos)
    {
        return Nodes[pos.y, pos.x];
    }

    public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        Vector2Int[] directions = new Vector2Int[] {
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1)
            //new Vector2Int(-1, -1),
            //new Vector2Int(1, 1),
            //new Vector2Int(-1, 1),
            //new Vector2Int(1, -1)
        };

        for (int y = 0; y < GridHeight; ++y)
        {
            for (int x = 0; x < GridWidth; ++x)
            {
                Nodes[y, x].state = Node.State.None;
                Nodes[y, x].Parent = new Vector2Int(-1, -1);
                Nodes[y, x].G = 0;
            }
        }


        // A* goes here

        // F = total path length
        // G = distance travelled in total
        // H = Estimated distance remaining to target.
        // C = Cost of walking over this node.



        //Make an open list (List<Vector2Int>)
        List<Vector2Int> openList = new List<Vector2Int>();

        //Make a current node variable (Vector2Int)
        Vector2Int currentCoord;

        //Add start to open 
        openList.Add(start);

        //Set start node to state of open
        GetNode(start).state = Node.State.Open;

        //Set start node G to 0
        GetNode(start).G = 0;

        //While open list is't empty
        while (openList.Count > 0)
        {

            //Find the lowest F in open list

            //Lowestindex=0
            Vector2Int lowestFCoord = openList[0];
            int lowestF = GetNode(lowestFCoord).F;
            int lowestFIndex = 0;

            //lowestF is first open list node F

            //for each open list node
            for(int i = 1; i < openList.Count; i++)
            {
                if(GetNode(openList[i])).F < lowestFIndex)
                {
                    lowestFIndex = i;
                    
                }
                
            }

        currentCoord = lowestFCoord;
        Node currentNode = GetNode(currentCoord);
        currentNode.state = Node.State.Closed;
        openList.RemoveAt(lowestFIndex);

        for(int i=0; i<directions.Length; i++) 
        { 
            Vector2Int adjacentCoord = currentCoord + directions[i];
            Node adjacentNode = GetNode(adjacentCoord);

            int cost = adjacentNode.C;

            if (adjacentNode.Wall)
            {
                //Skip
            }
            else if(adjacentNode.state == Node.State.Closed)
            {
                //Skip
            }
            else if(adjacentNode.state == Node.State.Open)
            {
                if(adjacentNode.G > currentNode.G + cost)
                { 
                    adjacentNode.G = currentNode.G + cost;
                    adjacentNode.H = 0;
                    adjacentNode.F = adjacentNode.G + adjacentNode.H;
                    adjacentNode.Parent = currentCoord;
                
                }
            }
            else if(adjacentNode.state == Node.State.None) 
            {
                adjacentNode.G = currentNode.G + cost;
                adjacentNode.H = 0;
                adjacentNode.F = adjacentNode.G + adjacentNode.H;
                adjacentNode.Parent = currentCoord;
                adjacentNode.state = Node.State.Open;
                openList.Add(adjacentCoord);
            }

        
        }

        if(GetNode(end).state == Node.State.Closed)
        { 
            List<Vector2Int> path = new List<Vector2Int>();

            Vector2Int backtackCoord = end;
            while(backtackCoord.x != -1)
            {
                path.Add(backtackCoord);
                backtackCoord = GetNode(backtackCoord).Parent;
            }
            return path;
        }

        // Return empty path
        return new List<Vector2Int>();

    }
}
