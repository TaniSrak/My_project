using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Cave : MonoBehaviour
{
    [SerializeField]
    private int _roomCount; //���������� ������������ ������

    [SerializeField]
    private GameObject _roomPrefab; //������ �� �������

    [SerializeField]
    private CaveGenerator.Grid _caveGrid; //������ �� �����

    [SerializeField]
    private List<GameObject> _rooms = new List<GameObject>(); //������ ������������ ������

    [SerializeField]
    private Graph _graph;   //������ �� ����

    private void Start()
    {
        GenerateRooms();
    }

    private void GenerateRooms() //������� �������
    {
        for (int i = 0; i < _roomCount; i++)
        {
            GameObject spawnedRoom = Instantiate(_roomPrefab);
            if(_caveGrid.SetRoomPosition(spawnedRoom))
            {
                _rooms.Add(spawnedRoom);
            }
            else
            {
                Destroy(spawnedRoom); //���������� ��������
            }
        }

        _graph.Triangulate(_rooms);
    }

}
