using System;

namespace Minesweeper
{
    public class Program
    {
        private const int rows = 10;
        private const int columns = 10;
        private const int mines = 10;
        private static char[,] grid = new char[rows, columns];
        private static bool[,] revealed = new bool[rows, columns];
        private static bool[,] flagged = new bool[rows, columns];
        private static int remainingCells;
        private static int remainingMines;
        private static int lives;
        private static bool gameOver;

        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Minesweeper!");
            Console.WriteLine("Press 1 to play Classic Minesweeper");
            Console.WriteLine("Press 2 to play Minesweeper with 3 lives");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    PlayClassicMinesweeper();
                    break;
                case "2":
                    PlayMinesweeperWithLives();
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    Main(args);
                    break;
            }
        }

        public static void PlayClassicMinesweeper()
        {
            InitializeGrid();
            GenerateMines();

            while (!gameOver)
            {
                DisplayGrid();
                Console.Write("Enter a row number: ");
                int row = int.Parse(Console.ReadLine());
                Console.Write("Enter a column number: ");
                int col = int.Parse(Console.ReadLine());

                if (IsMine(row, col))
                {
                    Console.WriteLine("Game Over. You stepped on a mine!");
                    gameOver = true;
                }
                else
                {
                    RevealCell(row, col);
                    CheckWin();
                }
            }

            Console.WriteLine("Thank you for playing!");
            Console.ReadLine();
        }

        public static void PlayMinesweeperWithLives()
        {
            InitializeGrid();
            GenerateMines();
            lives = 3;

            while (!gameOver)
            {
                Console.WriteLine($"Lives remaining: {lives}");
                DisplayGrid();
                Console.Write("Enter a row number: ");
                int row = int.Parse(Console.ReadLine());
                Console.Write("Enter a column number: ");
                int col = int.Parse(Console.ReadLine());

                if (IsMine(row, col))
                {
                    lives--;

                    if (lives == 0)
                    {
                        Console.WriteLine("Game Over. You stepped on a mine!");
                        gameOver = true;
                    }
                    else
                    {
                        Console.WriteLine($"You stepped on a mine! Lives remaining: {lives}");
                        RevealGrid();
                    }
                }
                else
                {
                    RevealCell(row, col);
                    CheckWin();
                }
            }

            Console.WriteLine("Thank you for playing!");
            Console.ReadLine();
        }

        public static void InitializeGrid()
        {
            remainingCells = rows * columns;
            remainingMines = mines;
            gameOver = false;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    grid[i, j] = '.';
                    revealed[i, j] = false;
                }
            }
        }

        public static void GenerateMines()
        {
            Random random = new Random();

            while (remainingMines > 0)
            {
                int row = random.Next(rows);
                int col = random.Next(columns);

                if (!IsMine(row, col))
                {
                    grid[row, col] = 'M';
                    remainingMines--;
                }
            }
        }

        public static bool IsMine(int row, int col)
        {
            return grid[row, col] == 'M';
        }

        public static void RevealGrid()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (IsMine(i, j))
                    {
                        grid[i, j] = 'M';
                    }
                    else
                    {
                        int count = CountNearbyMines(i, j);
                        grid[i, j] = count > 0 ? count.ToString()[0] : '.';
                    }
                }
            }
        }

        public static void RevealCell(int row, int col)
        {
            if (row >= 0 && row < rows && col >= 0 && col < columns && !revealed[row, col])
            {
                revealed[row, col] = true;
                remainingCells--;

                if (CountNearbyMines(row, col) == 0)
                {
                    RevealCell(row - 1, col - 1);
                    RevealCell(row - 1, col);
                    RevealCell(row - 1, col + 1);
                    RevealCell(row, col - 1);
                    RevealCell(row, col + 1);
                    RevealCell(row + 1, col - 1);
                    RevealCell(row + 1, col);
                    RevealCell(row + 1, col + 1);
                }
            }
        }

        public static int CountNearbyMines(int row, int col)
        {
            int count = 0;

            if (IsMine(row - 1, col - 1)) count++;
            if (IsMine(row - 1, col)) count++;
            if (IsMine(row - 1, col + 1)) count++;
            if (IsMine(row, col - 1)) count++;
            if (IsMine(row, col + 1)) count++;
            if (IsMine(row + 1, col - 1)) count++;
            if (IsMine(row + 1, col)) count++;
            if (IsMine(row + 1, col + 1)) count++;

            return count;
        }

        public static void DisplayGrid()
        {
            Console.WriteLine();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (revealed[i, j])
                    {
                        Console.Write($"{grid[i, j]} ");
                    }
                    else
                    {
                        Console.Write(". ");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public static void CheckWin()
        {
            if (remainingCells == mines)
            {
                Console.WriteLine("Congratulations! You won!");
                gameOver = true;
            }
        }
    }
}