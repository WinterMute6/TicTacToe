using System;
using System.Collections.Generic;
using System.Linq;
namespace DecisionTree
{
    public class Program
    {


        // main function
        public static void Main(string[] args)
        {

            List<int> AllMoves = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var root = new Node();
            BuildBranches(root, AllMoves);
            Console.WriteLine("Who goes first? Enter 'Man' or any key for machine:");
            var choice = Console.ReadLine();
            if (choice.ToLower() == "man")
            {
                HumanGoesFirst(root);
            }
            else
            {
                ComputerGoesFirst(root);
            }
        }

        public static void HumanGoesFirst(Node root)
        {
            var currentLevel = root;
            while(true)
            {
                if(currentLevel == null)
                {
                    break;
                }
                int number;
                while(true)
                {
                    Console.WriteLine("Your move?");
                    var yourMove = Console.ReadLine();

                    if(Int32.TryParse(yourMove, out number))
                    {
                        currentLevel = currentLevel.ChildNodes.Where(x => x.MoveHistory.Last().Cell == number).SingleOrDefault();
                        if(currentLevel == null)
                        {
                            Console.WriteLine("Invalid move. This Move is Taken");
                            continue;
                        }

                        break;
                    }
                    Console.WriteLine("Invalid Move.");
                }
                if(currentLevel.ChildNodes.Count() == 0)
                {
                    Console.WriteLine("This game is a tie.");
                    currentLevel = null;
                    break;
                }
                currentLevel = GetNextMove(currentLevel);
                var cell = currentLevel.MoveHistory.Last().Cell;
                Console.WriteLine($"My move is {cell}.");
                if(currentLevel.IsWin)
                {
                    Console.WriteLine("I win.");
                    break;
                }
            }
        }
        public static void ComputerGoesFirst(Node root)
        {
            var currentLevel = root;
            while (true)
            {
                if (currentLevel == null)
                {
                    break;
                }
                currentLevel = GetNextMove(currentLevel);
                var cell = currentLevel.MoveHistory.Last().Cell;
                Console.WriteLine($"My Move Is {cell}.");
                if (currentLevel.IsWin)
                {
                    Console.WriteLine("I won.");
                    break;
                }
                if (currentLevel.ChildNodes.Count() == 0)
                {
                    Console.WriteLine("This game is a tie.");
                    break;
                }
                int number;
                while (true)
                {
                    Console.WriteLine("Your move?");
                    var yourMove = Console.ReadLine();
                    if (Int32.TryParse(yourMove, out number))
                    {
                        currentLevel = currentLevel.ChildNodes.Where(x => x.MoveHistory.Last().Cell == number).SingleOrDefault();
                        if (currentLevel == null)
                        {
                            Console.WriteLine("Move is Taken.");
                            continue;
                        }
                        break;
                    }
                    Console.WriteLine("Invalid move.");
                }
            }
        }

        public static Node GetNextMove(Node currentLevel)
        {
            var winningMove =  currentLevel.ChildNodes.Where(x => x.IsWin).FirstOrDefault();
            if (winningMove != null)
                return winningMove;
            var minOrMax = currentLevel.MoveHistory.Count() == 0 || !currentLevel.MoveHistory.Last().Turn;
            return Random(currentLevel.ChildNodes.Where(x => x.Score ==  (minOrMax? currentLevel.ChildNodes.Max(y => y.Score): currentLevel.ChildNodes.Min(y => y.Score))).ToList());
        }
        private static Node Random(List<Node> list)
        {
            var random = new System.Random();
            int index = random.Next(list.Count());
            return list[index];
        }

        public static void BuildBranches(Node parentNode, List<int> allMoves)
        {
            var takenMoves = parentNode.MoveHistory;
            var availableCells = GetAvailableMoves(allMoves, takenMoves.Select(x=>x.Cell).ToList());
            
            foreach (int cell in availableCells)
            {
                var child = new Node();
                child.MoveHistory = parentNode.MoveHistory.Select(m=>m).ToList();
                bool turn;
                if(parentNode.MoveHistory.Count() == 0)
                {
                    turn = true;
                }
                else
                {
                    var lastMove = parentNode.MoveHistory.Last();
                    turn = !lastMove.Turn;
                }
                
                child.MoveHistory.Add(new Move(cell, turn));
                parentNode.AddChild(child);

                if (IsWin(child.MoveHistory, turn))
                {
                    child.IsWin = true;
                    child.Score = turn? 1: -1;
                    
                    return;
                }
   
                BuildBranches(child, allMoves);

                if (child.MoveHistory.Count() > 0 && child.ChildNodes.Count() > 0
                    && child.ChildNodes.Any(x=>x.Score==-1)
                    )
                {

                }

                if (child.MoveHistory.Count() > 0 && child.ChildNodes.Count() > 0)
                {
                    var previousMove = child.MoveHistory.Last();
                    child.Score = !previousMove.Turn ?
                        child.ChildNodes.Max(x => x.Score) :
                        child.ChildNodes.Min(x => x.Score);
                }
            }
        }

        
        public static bool IsWin(List<Move> moveHistory, bool turn)
        {
            var winningPositions = new[] {
                new [] { 1, 2, 3 },
                new [] { 4, 5, 6 },
                new [] { 7, 8, 9 },
                new [] { 1, 4, 7 },
                new [] { 2, 5, 8 },
                new [] { 3, 6, 9 },
                new [] { 1, 5, 9 },
                new [] { 3, 5, 7 },
            };

            var ourMoves = moveHistory.Where(x => x.Turn == turn).Select(x=> x.Cell);
            
            foreach(var position in winningPositions)
            {
                if (position.All(x => ourMoves.Any(y => y == x)))
                    return true;
            }
            return false;
            
        }

        public static List<int> GetAvailableMoves(List<int> allMoves, List<int> takenMoves)
        {
            return allMoves.Except(takenMoves).ToList();

        }
    }

        

        public class Move
    {
        public Move(int cell, bool turn)
        {
            Cell = cell;
            Turn = turn;
        }

        public int Cell { get; set; }
        public bool Turn { get; set; }
    }

    public class Node
    {
        public Node()
        {
            MoveHistory = new List<Move>();
            ChildNodes = new List<Node>();
        }

     
        public void AddChild(Node child)
        {
            ChildNodes.Add(child);
        }
        public int Score { get; set; } 
        public bool IsWin { get; set; }
        public List<Move> MoveHistory { get; set; }
        public string NodeName { get; set; }
        public Node ParentNode { get; set; }
        public List<Node> ChildNodes { get; set; }
    }
}