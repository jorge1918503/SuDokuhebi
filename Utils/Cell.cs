using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuDokuhebi.Utils
{
    class Cell
    {
        private bool purpleZone;
        private bool playerIn;
        private bool snakeIn;
        private bool snakeHead;
        private int row;
        private int col;

        public Cell() { }

        public Cell(bool purpleZone, bool playerIn, bool snakeIn, bool snakeHead, int row, int col)
        {
            this.purpleZone = purpleZone;
            this.playerIn = playerIn;
            this.snakeIn = snakeIn;
            this.snakeHead = snakeHead;
            this.row = row;
            this.col = col;
        }

        public Cell(bool purpleZone, int row, int col)
        {
            this.purpleZone = purpleZone;
            this.playerIn = false;
            this.snakeIn = false;
            this.snakeHead = false;
            this.row = row;
            this.col = col;
        }

        public bool PurpleZone { get => purpleZone; set => purpleZone = value; }
        public bool PlayerIn { get => playerIn; set => playerIn = value; }
        public bool SnakeIn { get => snakeIn; set => snakeIn = value; }
        public bool SnakeHead { get => snakeHead; set => snakeHead = value; }
        public int Row { get => row; set => row = value; }
        public int Col { get => col; set => col = value; }
    }
}
