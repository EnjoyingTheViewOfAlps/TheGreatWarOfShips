using BattleOfShips;
using System;
using System.Threading;

namespace SeaBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            char[,] board = new char[10, 10];
            char[,] aiBoard = new char[10, 10];
            char[,] aioBoardP = new char[10, 10];

            Ship ship = new Ship();

            int fourIndex = 1;
            int threeIndex = 2;
            int twoIndex = 3;
            int oneIndex = 4;

            bool player = true;

            while (CountOfShips(oneIndex, twoIndex, threeIndex, fourIndex))
            {
                Console.WriteLine("Вам нужно расставить корабли. Напишите какой корабль хотите поставить");
                string type = Console.ReadLine();
                switch (type)
                {
                    case "1":
                        PlaceShip(board, 1);
                        oneIndex--;
                        break;

                    case "2":
                        PlaceShip(board, 2);
                        twoIndex--;
                        break;

                    case "3":
                        PlaceShip(board, 3);
                        threeIndex--;
                        break;

                    case "4":
                        PlaceShip(board, 4);
                        fourIndex--;
                        break;
                }
            }

            InitializeAIBoard(aiBoard);

            while (CountOfShips(oneIndex, twoIndex, threeIndex, fourIndex))
            {
                PlaceAIShips(aiBoard, ref oneIndex, 1);
                PlaceAIShips(aiBoard, ref twoIndex, 2);
                PlaceAIShips(aiBoard, ref threeIndex, 3);
                PlaceAIShips(aiBoard, ref fourIndex, 4);
            }

            bool t = false;

            while (true)
            {
                player = PlayerTurn(aioBoardP, aiBoard, board, ref oneIndex, ref twoIndex, ref threeIndex, ref fourIndex, ref t);
                if (!player) break;

                t = ComputerTurn(board);
            }
        }

        static bool CountOfShips(int x, int y, int z, int v)
        {
            if (x == 0 && y == 0 && z == 0 && v == 0)
            {
                return false;
            }
            return true;
        }

        static void Print(char[,] c)
        {
            Console.WriteLine("   A B C D E F G H I J");
            Console.WriteLine("   -------------------");
            for (int i = 0; i < 10; i++)
            {
                Console.Write((i) + "| ");
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(c[i, j] + "  ");
                }
                Console.WriteLine();
            }
        }

        static void PlaceShip(char[,] board, int size)
        {
            Console.WriteLine($"Напишите первую координату и направление (либо вправо(1), либо вниз(2)) для корабля размером {size}");
            string input = Console.ReadLine().ToUpper();
            int row = input[1] - '0';
            int col = input[0] - 'A';
            int direction = Convert.ToInt32(Console.ReadLine());

            Console.Clear();

            if (CanPlaceShip(row, col, size, direction, board))
            {
                Ship.Create(board, row, col, direction, size);
                FillSurroundings(row, col, size, direction, board);
            }
            else
            {
                Console.WriteLine("Нельзя повставить корабль");
                PlaceShip(board, size); // Попробовать снова разместить корабль
            }

            Print(board);
        }

        static void InitializeAIBoard(char[,] aiBoard)
        {
            Random rnd = new Random();
            int oneIndex = 4;
            int twoIndex = 3;
            int threeIndex = 2;
            int fourIndex = 1;

            while (CountOfShips(oneIndex, twoIndex, threeIndex, fourIndex))
            {
                PlaceAIShips(aiBoard, ref oneIndex, 1);
                PlaceAIShips(aiBoard, ref twoIndex, 2);
                PlaceAIShips(aiBoard, ref threeIndex, 3);
                PlaceAIShips(aiBoard, ref fourIndex, 4);
            }
        }

        static void PlaceAIShips(char[,] aiBoard, ref int shipIndex, int shipSize)
        {
            Random rnd = new Random();
            int row = rnd.Next(10);
            int col = rnd.Next(10);
            int direction = rnd.Next(1, 2);

            if (CheckAdjacentShips(row, col, aiBoard) == false && CanPlaceShip(row, col, shipSize, direction, aiBoard))
            {
                Ship.Create(aiBoard, row, col, direction, shipSize);
                shipIndex--;
            }
            else
            {
                PlaceAIShips(aiBoard, ref shipIndex, shipSize);
            }
        }

        static bool PlayerTurn(char[,] aioBoardP, char[,] aiBoard, char[,] board, ref int oneIndex, ref int twoIndex, ref int threeIndex, ref int fourIndex, ref bool t)
        {
            Console.WriteLine("Напишите, куда хотите выстрелить");
            string input = Console.ReadLine().ToUpper();
            int row = input[1] - '0';
            int col = input[0] - 'A';

            if (Shoot(row, col, aiBoard))
            {
                aioBoardP[row, col] = 'X';
                Print(aioBoardP);
                if (isWin(board))
                {
                    Console.WriteLine("Победил игрок");
                    Thread.Sleep(3000);
                    Environment.Exit(0);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        static bool CheckAdjacentShips(int x, int y, char[,] grid)
        {
            for (int i = Math.Max(0, x - 1); i <= Math.Min(10 - 1, x + 1); i++)
            {
                for (int j = Math.Max(0, y - 1); j <= Math.Min(10 - 1, y + 1); j++)
                {
                    if (grid[i, j] == '*' && (i != x || j != y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static bool ComputerTurn(char[,] board)
        {
            Random rnd = new Random();
            int row = rnd.Next(10);
            int col = rnd.Next(10);

            if (Shoot(row, col, board))
            {
                board[row, col] = 'X';
                Print(board);

                if (isWin(board))
                {
                    Console.WriteLine("Победил компьютер");
                    Thread.Sleep(3000);
                    Environment.Exit(0);
                }

                bool t = true;
                while (t)
                {
                    row = rnd.Next(row - 1, row + 1);
                    col = rnd.Next(col - 1, col + 1);

                    if (row >= 0 && row <= 9 && col >= 0 && col <= 9)
                    {
                        if (Shoot(row, col, board))
                        {
                            board[row, col] = 'X';
                            Print(board);

                            if (isWin(board))
                            {
                                Console.WriteLine("Победил компьютер");
                                Thread.Sleep(3000);
                                Environment.Exit(0);
                            }
                            continue;
                        }
                        else
                        {
                            t = false;
                            return true;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        bool Shoot(int row, int col, char[,] b)
        {
            if (b[row, col] == ' ' || b[row, col] == '~')
            {
                b[row, col] = 'O';
                return false;
            }
            else if (b[row, col] == '*')
            {
                b[row, col] = 'X'; 
                return true;
            }
            return false;
        }


        static bool isWin(char[,] b)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (b[i, j] == '*')
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static bool CanPlaceShip(int x, int y, int size, int direction, char[,] gameBoard)
        {
            if (direction == 2)
            { 
               
                if (x + size > 10)
                    return false;

                for (int i = x; i < x + size; i++)
                {
                    if (gameBoard[i, y] != '\0')
                        return false;
                }
            }
            else if (direction == 1)
            {
                if (y + size > 10)
                    return false;

                for (int j = y; j < y + size; j++)
                {
                    if (gameBoard[x, j] != '\0')
                        return false;
                }
            }

            return true;
        }

        static void FillSurroundings(int x, int y, int size, int direction, char[,] gameBoard)
        {
            for (int i = x - 1; i <= x + size; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i >= 0 && i < 10 && j >= 0 && j < 10 && gameBoard[i, j] == '\0')
                    {
                        gameBoard[i, j] = '~';
                    }
                }
            }

            if (direction == 2)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (x - 1 >= 0 && x - 1 < 10 && j >= 0 && j < 10 && gameBoard[x - 1, j] == '\0')
                    {
                        gameBoard[x - 1, j] = '~';
                    }

                    if (x + size < 10 && x + size >= 0 && j >= 0 && j < 10 && gameBoard[x + size, j] == '\0')
                    {
                        gameBoard[x + size, j] = '~';
                    }
                }
            }
            else if (direction == 1)
            {
                for (int i = x - 1; i <= x + 1; i++)
                {
                    if (i >= 0 && i < 10 && y - 1 >= 0 && y - 1 < 10 && gameBoard[i, y - 1] == '\0')
                    {
                        gameBoard[i, y - 1] = '~';
                    }

                    if (i >= 0 && i < 10 && y + size < 10 && y + size >= 0 && gameBoard[i, y + size] == '\0')
                    {
                        gameBoard[i, y + size] = '~';
                    }
                }
            }
        }
    }
}

