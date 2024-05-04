using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Cave : MonoBehaviour
{
    [SerializeField]
    private int _roomCount; //количество заспавненных комнат

    [SerializeField]
    private GameObject _roomPrefab; //ссылка на комнату

    [SerializeField]
    private CaveGenerator.Grid _caveGrid; //ссылка на сетку

    [SerializeField]
    private List<GameObject> _rooms = new List<GameObject>(); //список заспавненных комнат

    [SerializeField]
    private Graph _graph;   //ссылка на граф

    private void Start()
    {
        GenerateRooms();
    }

    private void GenerateRooms() //генерим комнаты
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
                Destroy(spawnedRoom); //уничтожвем дистроем
            }
        }

        _graph.Triangulate(_rooms);
    }

}
