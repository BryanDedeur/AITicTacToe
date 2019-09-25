using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public static class Board
{
    public static char emptyPosition = '_';

    public struct move
    {
        public int score;
        public ushort depth;
        //public char player;
        public ushort x;
        public ushort y;
    }

    public static string Encode(char[,] board)
    {
        string encodedBoard = "";
        for (ushort x = 0; x < 3; x++)
        {
            for (ushort y = 0; y < 3; y++)
            {
                encodedBoard += board[x, y];
            }
        }
        return encodedBoard;
    }

    public static char OppositePlayer(char player)
    {
        if (player == 'X')
        {
            return 'O';
        }
        else if (player == 'O')
        {
            return 'X';
        }
        return '_';
    }
    
    public static bool IsValidMove(char[,] board, char player, ushort x, ushort y)
    {
        if (!(x < 3 && y < 3))
            return false;
        if (board[x, y] != emptyPosition)
            return false;
        return true;
    }
    
    public static string Visualize(char[,] board)
    {
        string output = "";
        for (ushort x = 0; x < 3; x++)
        {
            string line = "";
            for (ushort y = 0; y < 3; y++)
            {
                line += board[y, x];
            }
            output += line + '\n';
        }
        return output;
    }

    public static char[,] MirorBoard(char[,] board)
    {
        char[,] newBoard = new char[3,3];
        for (ushort x = 0; x < 3; x++)
        {
            for (ushort y = 0; y < 3; y++)
            {
                newBoard[x, y] = board[x,y];
            }
        }
        return newBoard;
    }

    public static int AvailableMovesCount(char[,] board)
    {
        int counter = 0;
        for (ushort x = 0; x < 3; x++)
        {
            for (ushort y = 0; y < 3; y++)
            {
                if (board[x, y] == emptyPosition)
                {
                    counter += 1;
                }
            }
        }

        return counter;
    }
    

    // checks the current move and returns true or false if win detected
    public static bool CheckForWin(char[,] board, char player, ushort moveX, ushort moveY)
    {
/*        // not possible to win with only 4 moves on board
        if (AvailableMovesCount(board) > 5) 
            return false;*/
    
        // check win scenarios only on verticle and horizontal line of current move
        int count = 0;
        // vertical to current move
        for (ushort y = 0; y < 3; y++)
            if (board[moveX, y] == player)
                count += 1;
            else
                break;

        if (count > 2)
        {
            return true;
        }
        else
        {
            count = 0;
        }
    
        // horizontal to current move
        for (ushort x = 0; x < 3; x++)
            if (board[x, moveY] == player)
                count += 1;
            else
                break;
    
        if (count > 2)
            return true;

        // diagonals if move is in diagonal space
        if (Mathf.Abs(moveX - moveY) == 0 || Mathf.Abs(moveX - moveY) == 2)
        {
            if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
                return true;
            if (board[2, 0] == player && board[1, 1] == player && board[0, 2] == player)
                return true;
        }
        return false;
    }

    public static char GetWinner(char[,] board, char player, ushort moveX, ushort moveY)
    {
        ushort count = 0;
        for (ushort y = 0; y < 3; y++)
            if (board[moveX, y] == player)
                count += 1;
            else
                break;

        if (count > 2)
            return player;
        
        count = 0;
    
        for (ushort x = 0; x < 3; x++)
            if (board[x, moveY] == player)
                count += 1;
            else
                break;
    
        if (count > 2)
            return player;

        if (Mathf.Abs(moveX - moveY) == 0 || Mathf.Abs(moveX - moveY) == 2)
        {
            if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
                return player;
            if (board[2, 0] == player && board[1, 1] == player && board[0, 2] == player)
                return player;
        }
        return emptyPosition;
    }



    public static bool TerminalState(char[,] board, char player, ushort moveX, ushort moveY )
    {
        if (AvailableMovesCount(board) == 0)
        {
            return true;
        }

        if (CheckForWin(board, player, moveX, moveY))
        {
            return true;
        }

        return false;
    }
}
