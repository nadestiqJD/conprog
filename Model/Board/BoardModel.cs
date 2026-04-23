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

        private static readonly int _boardBorderPadding = 6;
        public int Width
        {
            get => _board.Width + _boardBorderPadding;
        }

        public int Height 
        {
            get => _board.Height + _boardBorderPadding;
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
