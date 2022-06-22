using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace ConwaysGameOfLife.Processing
{
    public class GameProcessing
    {
        public GameProcessing(List<KeyValuePair<string, StringValues>> requestData)
        {
            for (var i = 0; i < worldSize; i++)
            {
                for (var j = 0; j < worldSize; j++)
                {
                    currentWorld[i, j] = false;
                    newWorld[i, j] = false;
                }
            }

            if (requestData == null) return;

            for (int row = 0; row < worldSize; row++)
                for(int col = 0; col < worldSize; col++)
                {
                    var data = int.Parse(requestData[row].Value);
                    if((data & (1 << col)) != 0)
                    {
                        currentWorld[row, col] = true;
                    }
                }

            return;
        }

        public static int[] GetRandomData()
        {
            int[] data = new int[30];
            Random random = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 0; i < GameProcessing.worldSize; i++)
            {
                data[i] = random.Next(int.MinValue, int.MaxValue);
            }

            return data;
        }

        public JsonResult GetNextStep()
        {
            int[] result = new int[worldSize];

            for (var row = 0; row < worldSize; row++)
            {
                for (var col = 0; col < worldSize; col++)
                {
                    int neighbors = GetNeighbors(row, col);
                    if (neighbors == 3 || currentWorld[row, col] == true && neighbors == 2)
                    {
                        newWorld[row, col] = true;
                    }
                    else
                    {
                        newWorld[row, col] = false;
                    }
                }
            }

            for (var row = 0; row < worldSize; row++)
            {
                for (var col = 0; col < worldSize; col++)
                {
                    if (newWorld[row, col])
                    {
                        result[row] |= (1 << col);
                    }
                }
            }

            return new JsonResult(result);
        }

        private int GetNeighbors(int row, int col)
        {
            int result = 0;

            if (currentWorld[Increment(row), Increment(col)]) result++;
            if (currentWorld[row,            Increment(col)]) result++;
            if (currentWorld[Decrement(row), Increment(col)]) result++;
            if (currentWorld[Decrement(row), col])            result++;
            if (currentWorld[Decrement(row), Decrement(col)]) result++;
            if (currentWorld[row,            Decrement(col)]) result++;
            if (currentWorld[Increment(row), Decrement(col)]) result++;
            if (currentWorld[Increment(row), col])            result++;

            return result;
        }
        private int Increment(int value) => value == worldSize - 1 ? 0 : value + 1;
        private int Decrement(int value) => value == 0 ? worldSize - 1 : value - 1;

        public const int worldSize = 30;
        private bool[,] currentWorld = new bool[worldSize, worldSize];
        private bool[,] newWorld = new bool[worldSize, worldSize];
    }
}
