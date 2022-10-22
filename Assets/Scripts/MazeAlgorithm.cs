using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class MazeAlgorithm
{
     static int[] dx = new[] { 1, -1, 0, 0 };
     static int[] dy = new[] { 0, 0, 1, -1 };

     // 打通右边
     private static void AcrossRight(Cell[,] res, int x, int y)
     {
          Cell cell = res[x, y];
          cell.east = true;
          res[x, y] = cell;

          cell = res[x, y + 1];
          cell.west = true;
          res[x, y + 1] = cell;
     }
     
     // 打通下面
     private static void AcrossDown(Cell[,] res, int x, int y)
     {
          Cell cell = res[x, y];
          cell.south = true;
          res[x, y] = cell;

          cell = res[x+ 1, y];
          cell.north = true;
          res[x + 1, y] = cell;
     }
     
    public static Cell[,] StackGenerate(int xLen, int yLen, int xStart, int yStart)
     {
          int count = 0;
          Cell[,] res = new Cell[xLen, yLen];
          Stack<ValueTuple<int, int>> st = new Stack<ValueTuple<int, int>>();
          
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
               else    //回溯
               {
                    x = st.Peek().Item1;
                    y = st.Peek().Item2;
                    st.Pop();
               }
          }

          return res;
     }

    public static Cell[,] HurtAndKill(int xLen, int yLen, int xStart, int yStart)
    {
         int count = 0;
         Cell[,] res = new Cell[xLen, yLen];
         
         List<int> neighbors = new List<int>();
         // 存放临近有访问过的点
         HashSet<ValueTuple<int, int>> hashSet = new HashSet<(int, int)>();

         Cell tmp = res[xStart, yStart];
         tmp.visited = true;
         res[xStart, yStart] = tmp;
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
                        hashSet.Add(new ValueTuple<int, int>(x + dx[i], y + dy[i]));
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
                   
                   hashSet.Remove(new ValueTuple<int, int>(x + dx[idx], y + dy[idx]));
                   neighbors.Clear();
                   
                   ++count;
                   x = x + dx[idx];
                   y = y + dy[idx];
              }
              else    //寻找新的起点
              {
                   ValueTuple<int, int> nextStart = hashSet.First();
                   hashSet.Remove(nextStart);

                   for (int i = 0; i < 4; ++i)
                   {
                        int newX = nextStart.Item1 + dx[i];
                        int newY = nextStart.Item2 + dy[i];
                        if (newX >= 0 && newX < xLen && newY >= 0 && newY < yLen && res[newX, newY].visited)
                        {
                             x = newX;
                             y = newY;
                             break;
                        }
                   }
              }
         }
         
         return res;
    }

    public static Cell[,] BinaryTreeGenerate(int xLen, int yLen)
    {
         Cell[,] res = new Cell[xLen, yLen];

         //从左上到右下
         for (int j = 0; j < yLen; ++j)
         {
              for (int i = 0; i < xLen; ++i)
              {
                   if (j + 1 < yLen && i + 1 < xLen)
                   {
                        int idx = Random.Range(0, 2);
                        if (idx == 0)
                             AcrossDown(res, i ,j);
                        else if (idx == 1)
                             AcrossRight(res, i, j);
                   }
                   else if (j + 1 < yLen)
                   {
                        AcrossRight(res, i, j);
                   }
                   else if (i + 1 < xLen)
                   {
                        AcrossDown(res, i ,j);
                   }
              }
         }

         return res;
    }
}
