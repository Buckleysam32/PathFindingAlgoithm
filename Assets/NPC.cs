using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public List<Vector2Int> Path = new List<Vector2Int>();

    void Start()
    {
    }

    void Update()
    {
        Vector2Int pos = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        for(int i=0;i<Path.Count-1;++i)
        {
            Debug.DrawLine(
                new Vector3(Path[i].x + 0.5f, 0.1f, Path[i].y + 0.5f),
                new Vector3(Path[i + 1].x + 0.5f, 0.1f, Path[i + 1].y + 0.5f),
                Color.green);
        }

        if (Path.Count == 0)
        {
            Vector2Int newtarget = new Vector2Int();
            do
            {
                newtarget.x = Random.Range(1, Pathfind.GridWidth - 1);
                newtarget.y = Random.Range(1, Pathfind.GridHeight - 1);
            } while (Pathfind.GetNode(newtarget).Wall);

            Path = Pathfind.FindPath(
                new Vector2Int((int)transform.position.x, (int)transform.position.z),
                newtarget);
        }
        if (Path.Count != 0)
        {   
            Vector3 target = new Vector3(
                Path[Path.Count - 1].x + Pathfind.CellSize * 0.5f, 
                0.5f, 
                Path[Path.Count - 1].y + Pathfind.CellSize * 0.5f);
            GetComponent<Rigidbody>().velocity = (target - transform.position).normalized * 8.0f;

            if(Vector3.Distance(transform.position-new Vector3(0,0.5f,0), target)<0.1f)
            {
                Path.RemoveAt(Path.Count - 1);
            }
        }
    }
}
