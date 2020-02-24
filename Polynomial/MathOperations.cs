using System;

namespace Algebra
{

    public struct DoubleMathOperations : IMathOperations<double>
    {
        public double Add(double a, double b) => a + b;

        public double Div(double a, double b) => a / b;

        public double Mul(double a, double b) => a * b;

        public double Sub(double a, double b) => a - b;

        public double Pow(double x, double y) => Math.Pow(x, y);

        public double Neg(double x) => -x;

        public double Abs(double x) => Math.Abs(x);

    }
}