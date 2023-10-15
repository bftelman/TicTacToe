namespace Tui.Core
{
    internal class Game
    {
        private static readonly char[] Board = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static bool IsPlayerTurn = true;
        private static bool GameOver = false;

        private static void DrawBoard()
        {
            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}  |  {1}  |  {2}", Board[0], Board[1], Board[2]);
            Console.WriteLine("_____|_____|_____ ");
            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}  |  {1}  |  {2}", Board[3], Board[4], Board[5]);
            Console.WriteLine("_____|_____|_____ ");
            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}  |  {1}  |  {2}", Board[6], Board[7], Board[8]);
            Console.WriteLine("     |     |      ");

            Console.WriteLine("\n\n");
        }

        private static void MakeMove(int position, char symbol)
        {
            if (Board[position - 1] != 'X' && Board[position - 1] != 'O')
            {
                Board[position - 1] = symbol;
                DrawBoard();
            }
            else
            {
                Console.WriteLine("Invalid move. Please try again.");
                if (IsPlayerTurn)
                {
                    PlayerMove();
                }
                else
                {
                    AIMove();
                }
            }
        }

        private static void PlayerMove()
        {
            Console.Write("Please enter a cell number: ");
            var input = Console.ReadLine();
            if (int.TryParse(input, out int position))
            {
                MakeMove(position, 'X');
            }
            else
            {
                Console.WriteLine("Invalid input. Please try again.");
                PlayerMove();
            }
        }

        private static void AIMove()
        {
            var bestScore = int.MinValue;
            var bestMove = 0;

            for (int i = 0; i < 9; i++)
            {
                if (Board[i] != 'X' && Board[i] != 'O')
                {
                    char temp = Board[i];
                    Board[i] = 'O';
                    int score = Minimax(Board, 0, false);
                    Board[i] = temp;

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = i;
                    }
                }
            }
            MakeMove(bestMove + 1, 'O');
        }

        private static int Minimax(char[] board, int depth, bool isMaximizing)
        {
            if (CheckWin(board, 'X')) return -10;
            if (CheckWin(board, 'O')) return 10;
            if (IsBoardFull(board)) return 0;

            if (isMaximizing)
            {
                int bestScore = int.MinValue;
                for (int i = 0; i < 9; i++)
                {
                    if (board[i] != 'X' && board[i] != 'O')
                    {
                        char temp = board[i];
                        board[i] = 'O';
                        int score = Minimax(board, depth + 1, false);
                        board[i] = temp;
                        bestScore = Math.Max(score, bestScore);
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                for (int i = 0; i < 9; i++)
                {
                    if (board[i] != 'X' && board[i] != 'O')
                    {
                        char temp = board[i];
                        board[i] = 'X';
                        int score = Minimax(board, depth + 1, true);
                        board[i] = temp;
                        bestScore = Math.Min(score, bestScore);
                    }
                }
                return bestScore;
            }
        }

        private static bool CheckWin(char[] board, char symbol)
        {
            return (board[0] == symbol && board[1] == symbol && board[2] == symbol) ||
                   (board[3] == symbol && board[4] == symbol && board[5] == symbol) ||
                   (board[6] == symbol && board[7] == symbol && board[8] == symbol) ||
                   (board[0] == symbol && board[3] == symbol && board[6] == symbol) ||
                   (board[1] == symbol && board[4] == symbol && board[7] == symbol) ||
                   (board[2] == symbol && board[5] == symbol && board[8] == symbol) ||
                   (board[0] == symbol && board[4] == symbol && board[8] == symbol) ||
                   (board[2] == symbol && board[4] == symbol && board[6] == symbol);
        }

        private static bool IsBoardFull(char[] board)
        {
            foreach (char cell in board)
            {
                if (cell != 'X' && cell != 'O')
                {
                    return false;
                }
            }
            return true;
        }

        private static void CheckForWin()
        {
            if (CheckWin(Board, 'X'))
            {
                Console.WriteLine("Player wins!");
                GameOver = true;
            }
            else if (CheckWin(Board, 'O'))
            {
                Console.WriteLine("Computer wins!");
                GameOver = true;
            }
            else if (IsBoardFull(Board))
            {
                Console.WriteLine("It's a draw!");
                GameOver = true;
            }
        }

        public static void Run()
        {
            Console.WriteLine("Game has started...\n");
            DrawBoard();
            Console.WriteLine("\n");
            while (!GameOver)
            {
                if (IsPlayerTurn)
                {
                    PlayerMove();
                    CheckForWin();
                    IsPlayerTurn = false;
                }
                else
                {
                    AIMove();
                    CheckForWin();
                    IsPlayerTurn = true;
                }
            }
        }
    }
}
