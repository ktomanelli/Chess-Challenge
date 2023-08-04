using System;
using System.Collections.Generic;
using System.Linq;
using ChessChallenge.API;
public class MyBot : IChessBot
{
    class RatedMove
    {
        public Move move { get; set; }
        public int Rating { get; set; }
    }

    private List<RatedMove> ratedMoves {get; set; }
        
    public Move Think(Board board, Timer timer)
    {
        Move[] moves = board.GetLegalMoves();
        ratedMoves = new List<RatedMove>();
        foreach (var move in moves)
        {
            ratedMoves.Add(RateMove(move,board));
        }

        RatedMove x =  ratedMoves.MaxBy(x => x.Rating);
        Console.WriteLine("Move: " + x.move +"rating: " + x.Rating);
        return x.move;
    }

    private RatedMove RateMove(Move move,Board board)
    {
        int rating = 0;
        if (IsSafeCapture(move,board))
        {
            rating++;
        }
        if(IsProtected(move,board))
        {
            rating++;
        }
        
        return new RatedMove()
        {
            move = move,
            Rating = rating
        };
    }

    private bool IsProtected(Move move, Board board)
    {
        board.MakeMove(move);
        var isAttacked = board.SquareIsAttackedByOpponent(move.TargetSquare);
        board.UndoMove(move);
        return isAttacked;
    }

    private bool IsSafeCapture(Move move, Board board)
    {
        return move.IsCapture && !board.SquareIsAttackedByOpponent(move.TargetSquare);
    }
}
