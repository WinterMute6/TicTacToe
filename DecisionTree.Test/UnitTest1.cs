using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DecisionTree;

namespace DecisionTree.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IsWinTest()
        {
            var history1 = new List<Move>();
            history1.Add(new Move(1,true));
            history1.Add(new Move(2, true));
            history1.Add(new Move(3, true));

            /* history.Add(new Move(1, true));
             history.Add(new Move(1, true));*/

            Assert.IsTrue(Program.IsWin(history1, true));

        }
        [TestMethod]
        public void IsNotWinTest()
        {
            var history1 = new List<Move>();
            history1.Add(new Move(1, true));
            history1.Add(new Move(2, true));
            history1.Add(new Move(3, false));

            /* history.Add(new Move(1, true));
             history.Add(new Move(1, true));*/

            Assert.IsFalse(Program.IsWin(history1, true));

        }

        [TestMethod]
        public void IsDiagonalWin()
        {
            var history1 = new List<Move>();
            history1.Add(new Move(1, true));
            history1.Add(new Move(5, true));
            history1.Add(new Move(9, true));

            /* history.Add(new Move(1, true));
             history.Add(new Move(1, true));*/

            Assert.IsTrue(Program.IsWin(history1, true));
        }

        [TestMethod]
        public void IsMixedWin()
        {
            var history1 = new List<Move>();
            history1.Add(new Move(1, true));
            history1.Add(new Move(2, true));
            history1.Add(new Move(3, true));

            history1.Add(new Move(7, false));
            history1.Add(new Move(8, false));
            history1.Add(new Move(9, false));

            /* history.Add(new Move(1, true));
             history.Add(new Move(1, true));*/

            Assert.IsTrue(Program.IsWin(history1, true));
            Assert.IsTrue(Program.IsWin(history1, false));
        }

    }
}
