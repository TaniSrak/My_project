using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaveGenerator
{
    public class Grid : MonoBehaviour
    {
        [SerializeField]
        private Vector3Int[,] _grid; //����� ��� ����� ������������� �������

        [SerializeField]
        private int _gridSize; //������ �����

        public void GenerateGrid() //���������� ������
        {
            _grid = new Vector3Int[_gridSize, _gridSize];
            for (int i = 0; i < _gridSize; i++)
            {
                for (int j = 0; j < _gridSize; j++)
                {
                    _grid[i, j] = new Vector3Int(i * 10, 0, j * 10);
                }
            }
        }

        public bool SetRoomPosition(GameObject room) //���������� �������� ������ �����
        {
            if (_grid == null)
            {
                GenerateGrid();
            }


            int cellX, cellY, counter = 0;
            do
            {
                counter++;
                cellX = Random.Range(0, _gridSize);
                cellY = Random.Range(0, _gridSize);
                if (counter >= _gridSize * _gridSize)
                {
                    return false;
                }
            } while (_grid[cellX, cellY].y != 0);

            _grid[cellX, cellY].y = 1; //�������� ��� ������ ������
            BlockCells(cellX, cellY); //�������� ���� ����� �������� ������ �����

            room.transform.position = _grid[cellX, cellY];

            return true;
        }

        //���������� ������ ����� � ��������� �������
        private void BlockCells(int x, int y)
        {
            bool lessX = x - 1 >= 0, greatX = x + 1 <= _gridSize - 1, lessY = y - 1 >= 0, greatY = y + 1 <= _gridSize - 1;
            if (lessX)
            {
                _grid[x - 1, y].y = 1;
                if (lessY)
                {
                    _grid[x - 1, y - 1].y = 1;
                }
                if (greatY)
                {
                    _grid[x - 1, y + 1].y = 1;
                }
            }
            if (greatX)
            {
                _grid[x + 1, y].y = 1;
                if (greatY)
                {
                    _grid[x + 1, y + 1].y = 1;
                }
                if (lessY)
                {
                    _grid[x + 1, y - 1].y = 1;
                }
            }

            if (lessY)
            {
                _grid[x, y - 1].y = 1;

            }
            if (greatY)
            {
                _grid[x, y + 1].y = 1;

            }
        }
    }
}


