using System.Windows;
using System;
using System.Windows.Controls;

namespace Gui
{
    public partial class MainWindow : Window
    {
        private static readonly char[] Board = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static bool IsPlayerTurn = true;
        private static bool GameOver = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Name.Substring(6), out int position))
            {
                if (Board[position - 1] != 'X' && Board[position - 1] != 'O')
                {
                    button.Content = IsPlayerTurn ? "X" : "O";
                    Board[position - 1] = IsPlayerTurn ? 'X' : 'O';
                    CheckForWin();
                    IsPlayerTurn = !IsPlayerTurn;
                    if (!GameOver && !IsPlayerTurn)
                    {
                        AIMove();
                        IsPlayerTurn = !IsPlayerTurn;
                        CheckForWin();
                    }
                }
            }
        }

        private void AIMove()
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

        private void MakeMove(int position, char symbol)
        {
            if (Board[position - 1] != 'X' && Board[position - 1] != 'O')
            {
                Button button = (Button)FindName("Button" + position);
                button.Content = symbol;
                Board[position - 1] = symbol;
            }
        }

        private int Minimax(char[] board, int depth, bool isMaximizing)
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

        private bool CheckWin(char[] board, char symbol)
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

        private bool IsBoardFull(char[] board)
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

        private void CheckForWin()
        {
            if (CheckWin(Board, 'X'))
            {
                MessageBox.Show("Player wins!");
                GameOver = true;
            }
            else if (CheckWin(Board, 'O'))
            {
                MessageBox.Show("Computer wins!");
                GameOver = true;
            }
            else if (IsBoardFull(Board))
            {
                MessageBox.Show("It's a draw!");
                GameOver = true;
            }
        }
    }
}
