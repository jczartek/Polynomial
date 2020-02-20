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

        [TestMethod]
        public void TestCalculate()
        {
            var poly = new Polynomial<double, DoubleMathOperations>(new double[3] { 1, 2, 1 });

            double result = poly.Calculate(10);

            Assert.AreEqual(121, result);

            poly = new Polynomial<double, DoubleMathOperations>(new double[3] { 1, 2, 5 });

            result = poly.Calculate(10);


            Assert.AreEqual(521, result);
        }

        [TestMethod]
        public void TestPlusOperator()
        {
            var poly1 = new Polynomial<double, DoubleMathOperations>(new double[4] { 11, 0, -7, 5 });
            var poly2 = new Polynomial<double, DoubleMathOperations>(new double[4] { 0, -4, 6, 2 });

            var result = poly1 + poly2;

            var poly3 = new Polynomial<double, DoubleMathOperations>(new double[4] { 11, -4, -1, 7 });

            result = poly2 + poly1;

            poly3 = new Polynomial<double, DoubleMathOperations>(new double[4] { 11, -4, -1, 7 });

            Assert.AreEqual(result.CompareTo(poly3), 0);

            result = poly3 + 11;

            Assert.AreEqual(result.CompareTo(new Polynomial<double, DoubleMathOperations>(new double[4] { 22, -4, -1, 7 })), 0);

            result = 11 + poly3;

            Assert.AreEqual(result.CompareTo(new Polynomial<double, DoubleMathOperations>(new double[4] { 22, -4, -1, 7 })), 0);

        }
    }
}
