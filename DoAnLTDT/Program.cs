  
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace DoAnLTDT
{
    class Program
    {
        public struct AdjacencyMatrix
        {
            public int numberOfVertexes;
            public int[,] adjacencyMatrix;

            public int DegVertex { get; internal set; }
        }

        public struct AdjacencyList
        {
            public int numberOfVertexs;
            public int[,] adjacencyList;
        }

        public enum GrapTypes
        {
            EMTPY_GRAPH,
            CYCLE_GRAPH,
            BUTTERFLY_GRAPH,
            MOTH_GRAPH,
            STAR_GRAPH,
            WHEEL_GRAPH,
            BARBELL_GRAPH,
            FRIENDSHIP_GRAPH,
            K_PARTITE_GRAPH
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
            StreamReader streamReader = new StreamReader("input.txt");

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

        public static int CountEdges(AdjacencyMatrix g)
        {
            int totalEdges = 0;
            List<int[]> countedIndexes = new List<int[]>();
            int i, j;
            for (i = 0; i < g.numberOfVertexes; ++i)
            {
                for (j = 0; j < g.numberOfVertexes; ++j)
                {
                    // voi moi dinh (i,j)
                    // kiem tra xem cap dinh da duoc dem chua
                    // chi can [i,j] hoac [j,i] da ton tai trong mang countedIndexes nghia la cap dinh (i,j) da duoc dem
                    bool counted = countedIndexes.Where(item => (item[0] == i && item[1] == j) || item[0] == j && item[1] == i).ToArray().Length > 0;

                    int edges = g.adjacencyMatrix[i, j];

                    // neu cap (i,j) chua duoc dem va giua chung co canh noi thi them vao danh sach dinh da dem
                    if (!counted && edges > 0)
                    {
                        totalEdges += edges;
                        countedIndexes.Add(new int[] { i, j });
                    }
                }
            }
            return totalEdges;
        }
        private void CountDegVertex(AdjacencyMatrix g, ref int[] DegVertex)
        {
            for (int i = 0; i < g.numberOfVertexes; ++i)
            {
                int count = 0;
                for (int j = 0; j < g.numberOfVertexes; ++j)
                    if (g.adjacencyMatrix[i, j] != 0)
                    {
                        count += g.adjacencyMatrix[i, j];
                        if (i == j)
                            count += g.adjacencyMatrix[i, i];
                    }
                DegVertex[i] = count;
            }
        }

        public static void CheckForEmptyGraph(AdjacencyMatrix am, Dictionary<GrapTypes, dynamic> gt)
        {
            int numberOfEdges = CountEdges(am);
            if (numberOfEdges == 0)
            {
                gt[GrapTypes.EMTPY_GRAPH] = am.numberOfVertexes;
            }
        }
        public static void CheckForButterflyGraph(AdjacencyMatrix am, Dictionary<GrapTypes, dynamic> gt)
        {
            int numberOfVertex = am.numberOfVertexes;
            int numberOfEdges = CountEdges(am);
            if (numberOfEdges == 6 && numberOfVertex == 5)
            {
                gt[GrapTypes.BUTTERFLY_GRAPH] = am.numberOfVertexes;
            }
        }
        public static void CheckForStarGraph(AdjacencyMatrix am,Dictionary<GrapTypes, dynamic> gt)
        {
              int numberOfEdges = CountEdges(am);
            if (numberOfEdges == (am.numberOfVertexes-1))
            {
                gt[GrapTypes.STAR_GRAPH] = am.numberOfVertexes;
            }

        }

        public static string FormatKPartiteToString(List<List<int>> p)
        {
            string result = "";
            foreach (List<int> part in p)
            {
                string temp = "";
                for (int i = 0; i < part.Count; ++i)
                {
                    if (i == 0)
                    {
                        temp += "{";
                    }
                    temp += $"{i}";
                    if (i < part.Count - 1)
                    {
                        temp += ",";
                    }
                    if (i == part.Count - 1)
                    {
                        temp += "}";
                    }
                }
                result += $"{temp} ";
            }
            return result;
        }

        public static Dictionary<GrapTypes, dynamic> ConstrucDefaultTypeMapping()
        {
            Dictionary<GrapTypes, dynamic> graphTypeMapping = new Dictionary<GrapTypes, dynamic>
            {
                [GrapTypes.EMTPY_GRAPH] = 0,
                [GrapTypes.CYCLE_GRAPH] = 0,
                [GrapTypes.BUTTERFLY_GRAPH] = 0,
                [GrapTypes.MOTH_GRAPH] = 0,
                [GrapTypes.STAR_GRAPH] = 0,
                [GrapTypes.WHEEL_GRAPH] = 0,
                [GrapTypes.BARBELL_GRAPH] = 0,
                [GrapTypes.FRIENDSHIP_GRAPH] = 0,
                [GrapTypes.K_PARTITE_GRAPH] = new List<List<int>> { }
            };

            return graphTypeMapping;
        }

        public static void PrintResult(Dictionary<GrapTypes, dynamic> graphTypeMapping)
        {
            foreach (GrapTypes key in graphTypeMapping.Keys)
            {
                switch (key)
                {
                    case GrapTypes.EMTPY_GRAPH:
                        int k = graphTypeMapping[GrapTypes.EMTPY_GRAPH];
                        bool isEmptyGraph = k > 0;
                        string result = isEmptyGraph ? $"k = {k}" : "Khong";
                        Console.WriteLine($"1. Do thi trong: {result}");
                        break;

                    case GrapTypes.CYCLE_GRAPH:
                        k = graphTypeMapping[GrapTypes.CYCLE_GRAPH];
                        bool isCycleGraph = k > 0;
                        result = isCycleGraph ? $"k = {k}" : "Khong";
                        Console.WriteLine($"2. Do thi vong: {result}");
                        break;

                    case GrapTypes.BUTTERFLY_GRAPH:
                        bool isButterflyGraph = graphTypeMapping[GrapTypes.BUTTERFLY_GRAPH] > 0;
                        result = isButterflyGraph ? $"Co" : "Khong";
                        Console.WriteLine($"3. Do thi hinh con buom: {result}");
                        break;

                    case GrapTypes.MOTH_GRAPH:
                        bool isMothGraph = graphTypeMapping[GrapTypes.MOTH_GRAPH] > 0;
                        result = isMothGraph ? $"Co" : "Khong";
                        Console.WriteLine($"4. Do thi hinh con ngai: {result}");
                        break;

                    case GrapTypes.STAR_GRAPH:
                        k = graphTypeMapping[GrapTypes.STAR_GRAPH];
                        bool isStarGraph = k > 0;
                        result = isStarGraph ? $"k = {k}" : "Khong";
                        Console.WriteLine($"5. Do thi hinh sao: {result}");
                        break;

                    case GrapTypes.WHEEL_GRAPH:
                        k = graphTypeMapping[GrapTypes.WHEEL_GRAPH];
                        bool isWheelGraph = k > 0;
                        result = isWheelGraph ? $"k = {k}" : "Khong";
                        Console.WriteLine($"6. Do thi hinh banh xe: {result}");
                        break;

                    case GrapTypes.BARBELL_GRAPH:
                        k = graphTypeMapping[GrapTypes.BARBELL_GRAPH];
                        bool isBarbellGraph = k > 0;
                        result = isBarbellGraph ? $"k = {k}" : "Khong";
                        Console.WriteLine($"7. Do thi Barbell: {result}");
                        break;

                    case GrapTypes.FRIENDSHIP_GRAPH:
                        k = graphTypeMapping[GrapTypes.FRIENDSHIP_GRAPH];
                        bool isFriendshipGraph = k > 0;
                        result = isFriendshipGraph ? $"k = {k}" : "Khong";
                        Console.WriteLine($"8. Do thi tinh ban: {result}");
                        break;

                    case GrapTypes.K_PARTITE_GRAPH:
                        List<List<int>> p = graphTypeMapping[GrapTypes.K_PARTITE_GRAPH];
                        bool isKPartiteGraph = p.Count > 0;
                        if (!isKPartiteGraph)
                        {
                            Console.WriteLine($"9. Do thi k phan: Khong");
                            break;
                        }
                        result = $"k = {p.Count} {FormatKPartiteToString(p)}";
                        Console.WriteLine($"9. Do thi k phan: {result}");
                        break;

                    default:
                        Console.WriteLine($"Do thi khong duoc ho tro!");
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            // Doc danh sach ke
            string filePath = GetFilePath("input.txt");
            AdjacencyList[] ALs = ReadMultiAL(filePath);

            // In danh sach AL va them vao AMs
            Console.WriteLine("In danh sach ke(AL) va ma tran ke(AM)");
            Console.WriteLine();
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

            // kiem tra loai do thi cho tung AM
            Console.WriteLine("kiem tra loai do thi cho tung ma tran ke");
            Console.WriteLine();
            foreach (AdjacencyMatrix am in AMs)
            {
                Dictionary<GrapTypes, dynamic> graphTypeMapping = ConstrucDefaultTypeMapping();
                CheckForEmptyGraph(am, graphTypeMapping);
                CheckForButterflyGraph(am, graphTypeMapping);
                CheckForStarGraph(am, graphTypeMapping);
                PrintResult(graphTypeMapping);
                Console.WriteLine("-----------------------");
            }
        }
    }
}
