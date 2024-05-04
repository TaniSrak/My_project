using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField]
    private List<GraphNode> _nodes = new List<GraphNode> ();
    [SerializeField]
    private List<GraphNode> _mst = new List<GraphNode>();//поле под дерево

    private List<GameObject> _allVertexes; //все вершины

    [SerializeField]
    private bool _visualizeMST;


    //визуализируем
    private void Update()
    {
        if (_nodes.Count > 0)
        {
            if (_visualizeMST && _mst.Count != 0)
            {
                _mst.ForEach(node => { Debug.DrawLine(node.VertexA.transform.position, node.VertexB.transform.position, Color.green, 25f); });
            }
            else
            {
                _nodes.ForEach(node => { Debug.DrawLine(node.VertexA.transform.position, node.VertexB.transform.position, Color.blue, 25f); });
            }
        }
    }

    public void GeneratorMST()//генератор дерева
    {
        List<GraphNode> notUsedNodes = new List<GraphNode>(_nodes); //список использованных вершин
        List<GameObject> usedV = new List<GameObject>(); //неиспользованные
        List<GameObject> notUsedV = new List<GameObject>(_allVertexes); //копия всех вершин

        usedV.Add(notUsedV[Random.Range(0, notUsedV.Count)]);
        notUsedV.Remove(usedV[0]);

        while(notUsedV.Count > 0)
        {
            int minE = -1;
            for (int i = 0; i < notUsedNodes.Count; i++)
            {
                //ищем самую маленькую ноду
                if ((usedV.IndexOf(notUsedNodes[i].VertexA) != -1 && notUsedV.IndexOf(notUsedNodes[i].VertexB) != -1) ||
                    (usedV.IndexOf(notUsedNodes[i].VertexB) != -1) && notUsedV.IndexOf((notUsedNodes[i].VertexA)) != -1)
                {
                    if (minE != -1)
                    {
                        if (notUsedNodes[i].Cost < notUsedNodes[minE].Cost)
                        {
                            minE = i;
                        }
                    }
                    else
                    {
                        minE = i;
                    }
                }
            }
            if (minE != -1)
            {
                if (usedV.IndexOf(notUsedNodes[minE].VertexA) != -1)
                {
                    usedV.Add(notUsedNodes[minE].VertexB);
                    notUsedV.Remove(notUsedNodes[minE].VertexB);
                }
                else
                {
                    usedV.Add(notUsedNodes[minE].VertexA);
                    notUsedV.Remove(notUsedNodes[minE].VertexA);
                }
                _mst.Add(notUsedNodes[minE]);
                notUsedNodes.RemoveAt(minE);
            }
            else
            {
                break;
            }
        }
        
    }

    public void Triangulate(List<GameObject> rooms) //передали все вершины
    {
        _allVertexes = rooms;

        for (int i = 0; i < rooms.Count; i++) 
        { 
            List<GameObject> nearestRooms = new List<GameObject> ();
            while (nearestRooms.Count != 3)
            {
                GameObject temp = rooms.Find((r) => r != rooms[i] && !nearestRooms.Contains(r));
                for (int j = 0; j < rooms.Count; j++)
                {
                    if(i != j && !nearestRooms.Contains(rooms[j]))
                    {
                        if (Vector3.Distance(temp.transform.position, rooms[i].transform.position)
                           > Vector3.Distance(rooms[j].transform.position, rooms[i].transform.position))
                        {
                            temp = rooms[j];
                        }
                    }
                }
                nearestRooms.Add(temp);
                GraphNode node = new GraphNode(rooms[i], temp);
                if(CheckDublicate(node))
                {
                    _nodes.Add(node);
                }
            }
            
        }
        GeneratorMST();
    }

    //метод для удаления проверки на наличие дубликатов туннелей
    public bool CheckDublicate(GraphNode node)
    {
        return _nodes.Find((x) => x.VertexB == node.VertexA && x.VertexA == node.VertexB) == null;
    }
}

[System.Serializable]
public class GraphNode //узел графа
{
    [field:SerializeField]
    public GameObject VertexA { get; private set; }
    [field: SerializeField]
    public GameObject VertexB { get; private set; }

    public float Cost { get; private set; } //стоимость ребра

    public GraphNode(GameObject a, GameObject b)
    {
        VertexA = a;
        VertexB = b;

        Cost = Vector3.Distance(a.transform.position, b.transform.position);
    }
}
