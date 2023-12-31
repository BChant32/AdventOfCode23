﻿using System.Collections;

internal class Program
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
    }
    private static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("day16_input.txt");
        int height = lines.Length, width = lines[0].Length;
        char[,] contrap = new char[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                contrap[i,j] = lines[i][j];
            }
        }

        int highscore = 0;
        for (int i = 0; i < height; i++)
        {
            BitArray[,] energised = energisedInit(height, width);
            processLaser(ref contrap, ref energised, height, width, Direction.Right, i, -1);
            int score = energised.Cast<BitArray>().Count(ba => ba.Cast<bool>().Any(b => b));
            if (score > highscore) highscore = score;
        }
        for (int i = 0; i < height; i++)
        {
            BitArray[,] energised = energisedInit(height, width);
            processLaser(ref contrap, ref energised, height, width, Direction.Left, i, width);
            int score = energised.Cast<BitArray>().Count(ba => ba.Cast<bool>().Any(b => b));
            if (score > highscore) highscore = score;
        }
        for (int j = 0; j < width; j++)
        {
            BitArray[,] energised = energisedInit(height, width);
            processLaser(ref contrap, ref energised, height, width, Direction.Down, -1, j);
            int score = energised.Cast<BitArray>().Count(ba => ba.Cast<bool>().Any(b => b));
            if (score > highscore) highscore = score;
        }
        for (int j = 0; j < width; j++)
        {
            BitArray[,] energised = energisedInit(height, width);
            processLaser(ref contrap, ref energised, height, width, Direction.Up, height, j);
            int score = energised.Cast<BitArray>().Count(ba => ba.Cast<bool>().Any(b => b));
            if (score > highscore) highscore = score;
        }
        Console.WriteLine(highscore);
    }

    private static BitArray[,] energisedInit(int height, int width)
    {
        BitArray[,] energisedBlank = new BitArray[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                energisedBlank[i, j] = new BitArray(4);
            }
        }
        return energisedBlank;
    }

    private static void drawContrap(char[,] contrap, BitArray[,] energised, int height, int width)
    {
        Console.Clear();
        for (int i = 0; i < height; i++)
        {
            Console.WriteLine(String.Join("", Enumerable.Range(0, width).Select(j => energised[i, j].Cast<bool>().Any(b => b) ? '#' : contrap[i, j])));
        }
        Thread.Sleep(200);
    }

    private static void processLaser(ref char[,] contrap, ref BitArray[,] energised, int height, int width, Direction curDir, int iCur, int jCur)
    {
        //drawContrap(contrap, energised, height, width);
        (int iNext, int jNext) = curDir switch
        {
            Direction.Left => (iCur, jCur - 1),
            Direction.Right => (iCur, jCur + 1),
            Direction.Up => (iCur - 1, jCur),
            Direction.Down => (iCur + 1, jCur),
        };
        if (iNext < 0 || jNext < 0 || iNext >= height || jNext >= width) return;
        if (energised[iNext, jNext][(int)curDir]) return;

        energised[iNext, jNext][(int)curDir] = true;
        if (contrap[iNext, jNext] == '.')
            processLaser(ref contrap, ref energised, height, width, curDir, iNext, jNext);
        else if (contrap[iNext, jNext] == '/')
        {
            switch (curDir)
            {
                case Direction.Left:
                    {
                        processLaser(ref contrap, ref energised, height, width, Direction.Down, iNext, jNext);
                        break;
                    }
                case Direction.Right:
                    {
                        processLaser(ref contrap, ref energised, height, width, Direction.Up, iNext, jNext);
                        break;
                    }
                case Direction.Up:
                    {
                        processLaser(ref contrap, ref energised, height, width, Direction.Right, iNext, jNext);
                        break;
                    }
                case Direction.Down:
                    {
                        processLaser(ref contrap, ref energised, height, width, Direction.Left, iNext, jNext);
                        break;
                    }
            }
        }
        else if (contrap[iNext, jNext] == '\\')
        {
            switch (curDir)
            {
                case Direction.Left:
                    {
                        processLaser(ref contrap, ref energised, height, width, Direction.Up, iNext, jNext);
                        break;
                    }
                case Direction.Right:
                    {
                        processLaser(ref contrap, ref energised, height, width, Direction.Down, iNext, jNext);
                        break;
                    }
                case Direction.Up:
                    {
                        processLaser(ref contrap, ref energised, height, width, Direction.Left, iNext, jNext);
                        break;
                    }
                case Direction.Down:
                    {
                        processLaser(ref contrap, ref energised, height, width, Direction.Right, iNext, jNext);
                        break;
                    }
            }
        }
        else if (contrap[iNext, jNext] == '-')
        {
            if (curDir == Direction.Left || curDir == Direction.Right)
                processLaser(ref contrap, ref energised, height, width, curDir, iNext, jNext);
            else
            {
                processLaser(ref contrap, ref energised, height, width, Direction.Left, iNext, jNext);
                processLaser(ref contrap, ref energised, height, width, Direction.Right, iNext, jNext);
            }
        }
        else if (contrap[iNext, jNext] == '|')
        {
            if (curDir == Direction.Up || curDir == Direction.Down)
                processLaser(ref contrap, ref energised, height, width, curDir, iNext, jNext);
            else
            {
                processLaser(ref contrap, ref energised, height, width, Direction.Up, iNext, jNext);
                processLaser(ref contrap, ref energised, height, width, Direction.Down, iNext, jNext);
            }
        }
    }
}