using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleOfShips
{
    public class Ship
    {
        static public void Create(char[,] gameBoard, int x, int y, int d, int size)
        {
            if (d == 2)
            {
                for (int i = x; i < x + size; i++)
                {
                    gameBoard[i, y] = '*';
                }
            }
            else if (d == 1)
            {
                for (int j = y; j < y + size; j++)
                {
                    gameBoard[x, j] = '*';
                }
            }
        }
    }
}
