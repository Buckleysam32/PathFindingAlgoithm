using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            "*.......*......*",
            "*..............*",
            "*..............*",
            "*.....****.....*",
            "*.....*........*",
            "*.....*........*",
            "*.....*........*",
            "*.....******...*",
            "*.....*........*",
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

        // Return empty path
        return new List<Vector2Int>();

    }
}
