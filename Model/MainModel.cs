using Data.Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class MainModel
    {
        public int BallCount { get; set;} = 10;

        public string Message { get; set;}

        public IBoard Board { get; set;} = new RectangularBoard();
    }
}
