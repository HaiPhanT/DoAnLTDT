using System;
using System.IO;
using System.Collections.Generic;

namespace DoAnLTDT
{
    class Program
    {
        public struct AdjacencyMatrix
        {
            public int numberOfVertexes;
            public int[,] adjacencyMatrix;
        }

        public struct AdjacencyList
        {
            public int numberOfVertexs;
            public int[,] adjacencyList;
        }

        public static string GetFilePath(string fileName)
        {
            // Input file should place in the same directory as Program.cs
            string currentDirectory = Directory.GetCurrentDirectory();
            string[] splitPath = currentDirectory.Split('\\');
            List<string> splitPathToList = new List<string>(currentDirectory.Split('\\'));

            // Cut the path to bin directory
            for (int i = 1; i < 4; ++i)
            {
                splitPathToList.RemoveAt(splitPath.Length - i);
            }
            string filePath = Path.Combine(String.Join("\\", splitPathToList.ToArray()), fileName);
            return filePath;
        }

        public static int[,] ReadAL(StreamReader streamReader, int numberOfVertexs)
        {
            string buffer;
            int[,] adjacencyList = new int[numberOfVertexs, numberOfVertexs];

            // gan gia tri mac dinh = -1
            for (int k = 0; k < numberOfVertexs; ++k)
            {
                for (int o = 0; o < numberOfVertexs; ++o)
                {
                    adjacencyList[k, o] = -1;
                }
            }

            // voi moi dong trong AL
            for (int i = 0; i < numberOfVertexs; ++i)
            {
                buffer = streamReader.ReadLine();
                string[] line = buffer.Split(" ");
                // doc gia tri tuong ung va gan vao ket qua theo thu tu trong file
                for (int j = 0; j < line.Length; ++j)
                {
                    adjacencyList[i, j] = int.Parse(line[j]);
                }
            }

            return adjacencyList;
        }

        public static AdjacencyList[] ReadMultiAL(string filePath)
        {
            StreamReader streamReader = new StreamReader(filePath);

            // khoi tao danh sach AL
            int numberOfALs = int.Parse(streamReader.ReadLine());
            AdjacencyList[] result = new AdjacencyList[numberOfALs];

            // voi moi don do thi
            for (int i = 0; i < numberOfALs; ++i)
            {
                // doc va tao adjacency-list
                AdjacencyList al = new AdjacencyList
                {
                    numberOfVertexs = int.Parse(streamReader.ReadLine())
                };
                al.adjacencyList = ReadAL(streamReader, al.numberOfVertexs);
                result[i] = al;
            }
            streamReader.Close();

            return result;
        }

        public static int[,] ConvertALToAM(int[,] adjacencyList)
        {
            int length = adjacencyList.GetLength(0);
            int[,] adjacencyMatrix = new int[length, length];
            for (int i = 0; i < length; ++i)
            {
                // lay tu index = 1 vi index = 0 la so canh ke
                for (int j = 1; j < length; ++j)
                {
                    int vertexIndex = adjacencyList[i, j];
                    if (vertexIndex != -1)
                    {
                        adjacencyMatrix[i, vertexIndex] = 1;
                    }
                }
            }
            return adjacencyMatrix;
        }

        public static void Print2DArray<T>(T[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }


        static void Main(string[] args)
        {
            // Doc danh sach ke
            string filePath = GetFilePath("input.txt");
            AdjacencyList[] ALs = ReadMultiAL(filePath);

            // In danh sach AL va them vao AMs
            AdjacencyMatrix[] AMs = new AdjacencyMatrix[ALs.Length];
            for (int i = 0; i < ALs.Length; ++i)
            {
                int[,] am = ConvertALToAM(ALs[i].adjacencyList);
                AMs[i] = new AdjacencyMatrix { numberOfVertexes = am.GetLength(0), adjacencyMatrix = am };
                Console.WriteLine("AL");
                Print2DArray(ALs[i].adjacencyList);
                Console.WriteLine("AM");
                Print2DArray(am);
                Console.WriteLine("-----------------------");
            }
        }
    }
}
