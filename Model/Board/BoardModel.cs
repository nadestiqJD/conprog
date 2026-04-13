using Data.Ball;
using Data.Board;
using Model.Ball;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Board
{
    public class BoardModel : ObservableObject, IBoardModel
    {
        private readonly IBoard _board;
        public int Width
        {
            get => _board.Width;
        }

        public int Height 
        {
            get => _board.Height;
        }

        public BoardModel(IBoard board)
        {
            _board = board;
            _board.DimensionsChanged += HandleDimensionsChanged;
        }

        private void HandleDimensionsChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(Width));
            RaisePropertyChanged(nameof(Height));
        }
    }
}
