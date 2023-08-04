using System;
using System.Collections.Generic;
using System.Linq;
using ChessChallenge.API;
public class MyBot : IChessBot
{
    class RatedMove
    {
        public Move move { get; set; }
        public float Rating { get; set; }
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
        Console.WriteLine(x.move +"rating: " + x.Rating);
        return x.move;
    }

    private RatedMove RateMove(Move move,Board board)
    {
        float rating = 0;

        rating += IsUnderAttack(move,board);
        rating += IsCapture(move,board);
        rating += IsProtectedMove(move,board);
        rating += IsProtected(move, board);
        return new RatedMove()
        {
            move = move,
            Rating = rating
        };
    }

    private float IsCheck(Move move, Board board)
    {
        var kingSquare = board.GetKingSquare(!board.IsWhiteToMove);
        board.MakeMove(move);
        bool isCheck = board.SquareIsAttackedByOpponent(kingSquare);
        board.UndoMove(move);
        return isCheck ? 2 : 0;
    }
    private float IsUnderAttack(Move move, Board board)
    {
        var isAttacked = board.SquareIsAttackedByOpponent(move.StartSquare);
        return isAttacked ? 1 : 0;
    }

    private float IsProtected(Move move, Board board)
    {
        bool isProtected = false;
        bool skipped = board.TrySkipTurn();
        if (skipped)
        {
            isProtected = board.SquareIsAttackedByOpponent(move.StartSquare);
            board.UndoSkipTurn();
        }

        return isProtected ? -0.5f : 0;
    }
    private float IsProtectedMove(Move move, Board board)
    {
        board.MakeMove(move);
        var isAttacked = board.SquareIsAttackedByOpponent(move.TargetSquare);
        board.UndoMove(move);
        return isAttacked ? 1 : 0;
    }

    private float IsCapture(Move move, Board board)
    {
        var isCapture = !board.SquareIsAttackedByOpponent(move.TargetSquare);
        return isCapture ? 1 : 0;
    }
}
