using System;
using System.Diagnostics;
using System.Threading;

public class VectorAddition
{
    private const int SIZE = 100000000;
    private static Random _random = new Random();

    public static void Main(string[] args)
    {
        int[] v1 = new int[SIZE];
        int[] v2 = new int[SIZE];
        int[] v3 = new int[SIZE];

        // adding random numbers in the vector
        FillVectors(v1, v2);

       
        Stopwatch stopwatch = Stopwatch.StartNew();
        SequentialAdd(v1, v2, v3);
        stopwatch.Stop();
        Console.WriteLine($"Sequential time: {stopwatch.ElapsedMilliseconds} ms");

        
        stopwatch.Restart();
        ParallelAdd(v1, v2, v3, 4);
        stopwatch.Stop();
        Console.WriteLine($"Parallel time (4 threads): {stopwatch.ElapsedMilliseconds} ms");
    }

    private static void FillVectors(int[] v1, int[] v2)
    {
        for (int i = 0; i < SIZE; i++)
        {
            v1[i] = _random.Next(100);
            v2[i] = _random.Next(100);
        }
    }

    private static void SequentialAdd(int[] v1, int[] v2, int[] v3)
    {
        for (int i = 0; i < SIZE; i++)
        {
            v3[i] = v1[i] + v2[i];
        }
    }

    private static void ParallelAdd(int[] v1, int[] v2, int[] v3, int numThreads)
    {
        int chunkSize = SIZE / numThreads;
        Thread[] threads = new Thread[numThreads];

        // starting threads with the start and end index
        for (int i = 0; i < numThreads; i++)
        {
            int start = i * chunkSize;
            int end = (i + 1) * chunkSize;
            if (i == numThreads - 1)
            {
                end = SIZE; 
            }
            threads[i] = new Thread(() => ParallelAddTask(v1, v2, v3, start, end));
            threads[i].Start();
        }

        // Ending all threads
        foreach (Thread thread in threads)
        {
            thread.Join();
        }
    }

    private static void ParallelAddTask(int[] v1, int[] v2, int[] v3, int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            v3[i] = v1[i] + v2[i];
        }
    }
}
