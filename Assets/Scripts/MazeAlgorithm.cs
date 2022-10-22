using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class MazeAlgorithm
{
     static int[] dx = new[] { 1, -1, 0, 0 };
     static int[] dy = new[] { 0, 0, 1, -1 };

     //检测周围是否有未访问点
     private static bool CheckNeighbors(Cell[,] cells, ValueTuple<int, int> pos)
     {
          for (int i = 0; i < 4; ++i)
          {
               if (!cells[pos.Item1 + dx[i], pos.Item2 + dy[i]].visited) return true;
          }

          return false;
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
}
