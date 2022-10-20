using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeGenerator : MonoBehaviour
{
     [SerializeField] private int _xLen;
     [SerializeField] private int _yLen;
     [SerializeField] private int _xStart, _yStart;
     [SerializeField] private Transform _pos;

     private Cell[,] _cells;
     
     private void Start()
     {
          _cells = StackGenerate(_xLen, _yLen, _xStart, _yStart);
     }

     private void Update()
     {
          for (int i = 0; i < _xLen; ++i)
          {
               for (int j = 0; j < _yLen; ++j)
               {
                    CreateCell(_cells[i, j], _pos.position + new Vector3(4 * j, 0, -4 * i));
               }
          }

          Debug.DrawLine(_pos.position + new Vector3(-2, 0, -4 * _xLen + 2),
               _pos.position + new Vector3(4 * _yLen - 2, 0, -4 * _xLen + 2));
          Debug.DrawLine(_pos.position + new Vector3(4 * _yLen - 2, 0, -4 * _xLen + 2),
               _pos.position + new Vector3(4 * _yLen - 2, 0, 2));
     }

     private void CreateCell(Cell cell, Vector3 position)
     {
          if (!cell.north) Debug.DrawLine(position + new Vector3(-2, 0, 2), position + new Vector3(2, 0, 2));
          if (!cell.west) Debug.DrawLine(position + new Vector3(-2, 0, 2), position + new Vector3(-2, 0, -2));
     }

     private Cell[,] StackGenerate(int xLen, int yLen, int xStart, int yStart)
     {
          int count = 0;
          Cell[,] res = new Cell[xLen, yLen];
          Stack<ValueTuple<int, int>> st = new Stack<ValueTuple<int, int>>();
          int[] dx = new[] { 1, -1, 0, 0 };
          int[] dy = new[] { 0, 0, 1, -1 };
          List<int> neighbors = new List<int>();

          Cell tmp = res[xStart, yStart];
          tmp.visited = true;
          res[xStart, yStart] = tmp;
          st.Push(new ValueTuple<int, int>(xStart, yStart));
          ++count;

          int x = xStart, y = yStart;
          while (count < xLen * yLen)
          {
               for (int i = 0; i < 4; ++i)
               {
                    if (x + dx[i] >= 0 && x + dx[i] < xLen && y + dy[i] >= 0 && y + dy[i] < yLen 
                        && !res[x + dx[i], y + dy[i]].visited)
                    {
                         neighbors.Add(i);
                    }
               }

               if (neighbors.Count > 0)
               {
                    int idx = neighbors[Random.Range(0, neighbors.Count)];
                    Cell nowCell = res[x, y];
                    Cell newCell = res[x + dx[idx], y + dy[idx]];
                    switch (idx)
                    {
                         case 0:    //south
                              nowCell.south = true;
                              newCell.north = true;
                              break;
                         case 1:    //north
                              nowCell.north = true;
                              newCell.south = true;
                              break;
                         case 2:    //east
                              nowCell.east = true;
                              newCell.west = true;
                              break;
                         case 3:    //west
                              nowCell.west = true;
                              newCell.east = true;
                              break;
                    }
                    newCell.visited = true;
                    res[x, y] = nowCell;
                    res[x + dx[idx], y + dy[idx]] = newCell;
                    st.Push(new ValueTuple<int, int>(x + dx[idx], y + dy[idx]));
                    ++count;
                    neighbors.Clear();
                    x = x + dx[idx];
                    y = y + dy[idx];
               }
               else
               {
                    x = st.Peek().Item1;
                    y = st.Peek().Item2;
                    st.Pop();
               }
          }

          return res;
     }
}
