using System;
using UnityEngine;

public enum GenerateType
{
     Stack, HurtAndKill, BinaryTree
}

public class MazeGenerator : MonoBehaviour
{
     [SerializeField] private int _xLen;    //行数
     [SerializeField] private int _yLen;    //列数
     [SerializeField] private int _xStart, _yStart;
     [SerializeField] private Transform _pos;   //左上角的位置
     [SerializeField] private GenerateType _generateType;

     private Cell[,] _cells;

     private void OnValidate()
     {
          _xStart = Mathf.Clamp(_xStart, 0, _xLen - 1);
          _yStart = Mathf.Clamp(_yStart, 0, _yLen - 1);
     }

     private void Start()
     {
          switch (_generateType)
          {
               case GenerateType.Stack:
                    _cells = MazeAlgorithm.StackGenerate(_xLen, _yLen, _xStart, _yStart);
                    break;
               case GenerateType.HurtAndKill:
                    _cells = MazeAlgorithm.HurtAndKill(_xLen, _yLen, _xStart, _yStart);
                    break;
               case GenerateType.BinaryTree:
                    _cells = MazeAlgorithm.BinaryTreeGenerate(_xLen, _yLen);
                    break;
               default:
                    Debug.LogWarning("Warn Select Finish");
                    break;
          }
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
}
