using Algebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPolynomial
{
    [TestClass]
    public class UnitPolynomialTest
    {
        [TestMethod]
        public void TestToString()
        {
            Polynomial<double, DoubleMathOperations> polynomial = new Polynomial<double, DoubleMathOperations>(new double[3] { 1, 2, 3 });

            Assert.AreEqual("y = 1 + 2x + 3x^2", polynomial.ToString());

            polynomial[1] = 0;

            Assert.AreEqual("y = 1 + 3x^2", polynomial.ToString());
        }
        [TestMethod]
        public void TestCompareTo()
        {
            var poly1 = new Polynomial<double, DoubleMathOperations>(new double[3] { 1, 2, 3 });
            var poly2 = new Polynomial<double, DoubleMathOperations>(new double[3] { 1, 2, 3 });

            // Are the same
            Assert.AreEqual(0, poly1.CompareTo(poly2));


            // poly1 is greater than poly2
            poly1[1] = 3;
            Assert.AreEqual(1, poly1.CompareTo(poly2));

            // poly2 is smaller than poly1
            Assert.AreEqual(-1, poly2.CompareTo(poly1));
        }

        [TestMethod]
        public void TestClone()
        {
            var poly1 = new Polynomial<double, DoubleMathOperations>(new double[3] { 1, 2, 3 });
            var poly2 = (poly1.Clone() as Polynomial<double, DoubleMathOperations>);

            Assert.AreEqual(0, poly1.CompareTo(poly2));
        }

        [TestMethod]
        public void TestResize()
        {
            var poly = new Polynomial<double, DoubleMathOperations>(new double[2] { 1, 2});
            poly[2] = 3;

            Assert.AreEqual("y = 1 + 2x + 3x^2", poly.ToString());
        }
    }
}
