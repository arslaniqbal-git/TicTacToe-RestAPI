using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicTacToe.Models;

namespace TicTacToe.Utilities
{
    public static class MinimaxUtility
    {
        static char client = 'x', server = 'o';


        /// <summary>
        /// This function is used to determine if there moves available on the board
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public static Boolean isMovesLeft(char[,] board)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (board[i, j] == '_')
                        return true;
            return false;
        }


        /// <summary>
        /// Evaulator function returning score against the move
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int evaluate(char[,] b)
        {
            for (int row = 0; row < 3; row++)
            {
                if (b[row, 0] == b[row, 1] &&
                    b[row, 1] == b[row, 2])
                {
                    if (b[row, 0] == server)
                        return +10;
                    else if (b[row, 0] == client)
                        return -10;
                }
            } 
            for (int col = 0; col < 3; col++)
            {
                if (b[0, col] == b[1, col] &&
                    b[1, col] == b[2, col])
                {
                    if (b[0, col] == server)
                        return +10;

                    else if (b[0, col] == client)
                        return -10;
                }
            } 
            if (b[0, 0] == b[1, 1] && b[1, 1] == b[2, 2])
            {
                if (b[0, 0] == server)
                    return +10;
                else if (b[0, 0] == client)
                    return -10;
            }

            if (b[0, 2] == b[1, 1] && b[1, 1] == b[2, 0])
            {
                if (b[0, 2] == server)
                    return +10;
                else if (b[0, 2] == client)
                    return -10;
            }
            return 0;
        }

        /// <summary>
        /// Minimax algorithm which checks all possible moves and returns the value
        /// </summary>
        /// <param name="board"></param>
        /// <param name="depth"></param>
        /// <param name="isMax"></param>
        /// <returns></returns>
        public static int minimax(char[,] board, int depth, Boolean isMax)
        {
            int score = evaluate(board);
            if (score == 10)
                return score;
            if (score == -10)
                return score;
            if (isMovesLeft(board) == false)
                return 0; 
            if (isMax)
            {
                int best = -1000; 
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    { 
                        if (board[i, j] == '_')
                        { 
                            board[i, j] = server; 
                            best = Math.Max(best, minimax(board,depth + 1, !isMax));
                            board[i, j] = '_';
                        }
                    }
                }
                return best;
            }
            else
            {
                int best = 1000; 
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (board[i, j] == '_')
                        { 
                            board[i, j] = client; 
                            best = Math.Min(best, minimax(board,
                                            depth + 1, !isMax));
                            board[i, j] = '_';
                        }
                    }
                }
                return best;
            }
        }


        /// <summary>
        /// This function returns the best possible move
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public static Move findBestMove(char[,] board)
        {
            int bestVal = -1000;
            Move bestMove = new Move();
            bestMove.row = -1;
            bestMove.col = -1;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == '_')
                    {
                        board[i, j] = server;
                        int moveVal = minimax(board, 0, false); 
                        board[i, j] = '_';
                        if (moveVal > bestVal)
                        {
                            bestMove.row = i;
                            bestMove.col = j;
                            bestVal = moveVal;
                        }
                    }
                }
            }
            return bestMove;
        }

        /// <summary>
        /// This function updates the board as per the input move
        /// </summary>
        /// <param name="move"></param>
        /// <param name="board"></param>
        /// <param name="isClient"></param>
        public static void UpdateBoard(Move move, char[,] board, bool isClient)
        {
            if (board[move.row, move.col] == '_')
            {
                if (isClient)
                {
                    board[move.row, move.col] = client;
                }
                else
                {
                    board[move.row, move.col] = server;
                }
            }
        }

        /// <summary>
        /// This function determines the winner 
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public static string DeterminWinner(char[,] board)
        {
            string message = CheckRows(board);
            if (message == null)
            {
                message = CheckCols(board);
            }
            if (message == null)
            {
                message = CheckDiagonals(board);
            }
            if (message == null)
            {
                message = CheckAntiDiagonals(board);
            }
            return message;
        }

        /// <summary>
        /// Helper function for DeterminWinner function
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        private static string CheckRows(char[,] board)
        {
            int countforClient = 0;
            int countforServer = 0;
            string msg = null;
            for (int i = 0; i < 3; i++)
            {
                countforClient = 0;
                countforServer = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == client)
                    {
                        countforClient++;
                    }
                    if (board[i, j] == server)
                    {
                        countforServer++;
                    }
                    if (countforClient == 3)
                    {
                        msg =  "Client Wins !!";
                        break;
                    }
                    if (countforServer == 3)
                    {
                        msg = "Server Wins !!";
                        break;
                    }
                }
            }
            return msg;
        }

        /// <summary>
        /// Helper function for DeterminWinner function
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        private static string CheckCols(char[,] board)
        {
            int countforClient = 0;
            int countforServer = 0;
            string msg = null;
            for (int i = 0; i < 3; i++)
            {
                countforClient = 0;
                countforServer = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (board[j, i] == client)
                    {
                        countforClient++;
                    }
                    if (board[j, i] == server)
                    {
                        countforServer++;
                    }

                    if (countforClient == 3)
                    {

                        return "Client Wins !!";
                        
                    }
                    if (countforServer == 3)
                    {
                        return "Server Wins !!";
                        
                    }
                }
            }
            return msg;
        }


        /// <summary>
        /// Helper function for DeterminWinner function
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        private static string CheckDiagonals(char[,] board)
        {
            int countforClient = 0;
            int countforServer = 0;
            string msg = null;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i == j && board[j, i] == client)
                    {
                        countforClient++;
                    }

                    if (i == j && board[j, i] == server)
                    {
                        countforServer++;
                    }

                    if (countforClient == 3)
                    {
                        msg = "Client Wins !!";
                        break;
                        
                    }
                    if (countforServer == 3)
                    {
                        msg = "Server Wins !!";
                        break;
                    }
                }
            }
            return msg;
        }

        /// <summary>
        /// Helper function for DeterminWinner function
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        private static string CheckAntiDiagonals(char[,] board)
        {
            int countforClient = 0;
            int countforServer = 0;
            string msg = null;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i+j == 2 && board[i, j] == client)
                    {
                        countforClient++;
                    }

                    if (i+j == 2 && board[i, j] == server)
                    {
                        countforServer++;
                    }

                    if (countforClient == 3)
                    {
                        msg = "Client Wins !!";
                        break;

                    }
                    if (countforServer == 3)
                    {
                        msg = "Server Wins !!";
                        break;
                    }
                }
            }
            return msg;
        }


    }
}