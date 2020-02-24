using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra
{
    public class Polynomial<T, C> : ICloneable, IComparable<Polynomial<T, C>>
        where T : struct, IComparable
        where C : IMathOperations<T>, new()
    {
        private List<T> _coes;
        private static readonly IMathOperations<T> OP = new C();

        public T this[int index]
        {
            get { return _coes[index]; }
            set
            {
                if (index >= _coes.Count)
                {
                    T[] tmp = new T[index + 1];
                    _coes.CopyTo(tmp);
                    _coes = tmp.ToList();
                }

                _coes[index] = value;
            }
        }

        public int Degree
        {
            get { return _coes.Count - 1; }
        }

        private Polynomial() { }

        public Polynomial(T[] ceos)
        {
            _coes = ceos.ToList();
        }

        public Polynomial(List<T> ceos)
        {
            _coes = ceos;
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("y = ");

            for (int i = 0; i < _coes.Count; i++)
            {
                if (_coes[i].CompareTo(default(T)) == 0) continue;
                
                string sign = (_coes[i].CompareTo(default(T))) == -1 ? " - " : " + ";
                T absNumber = OP.Abs(_coes[i]);
                
                builder.AppendFormat("{0}{1}{2}",
                    i > 0 ? absNumber.ToString() + "x" : _coes[i].ToString(),
                    i > 1 ? "^" + (i) : "",
                    i < (_coes.Count - 1) ? sign : "");
            }

            return builder.ToString();
        }
        public object Clone()
        {
            return new Polynomial<T, C>(_coes.ToList());
        }

        public int CompareTo(Polynomial<T, C> other)
        {
            if (other == null) return 1;

            if (_coes.Count > other._coes.Count) return 1;
            else if (_coes.Count < other._coes.Count) return -1;
            else
            {
                for (int i = _coes.Count - 1; i > 0; i--)
                {
                    int result = _coes[i].CompareTo(other._coes[i]);
                    if (result != 0) return result;
                }
            }
            return 0;
        }

        public T Calculate(T x)
        {
            T result = default;

            if (_coes.Count > 0)
                result = _coes[0];

            for (int i = 1; i < _coes.Count; i++)
                result = OP.Add(result, OP.Mul(_coes[i], OP.Pow(x, (double)i)));

            return result;
        }

        public static Polynomial<T, C> operator +(Polynomial<T, C> lhs, Polynomial<T, C> rhs)
        {
            var greaterPoly = lhs._coes.Count >= rhs._coes.Count ? lhs : rhs;
            var lowerPoly = lhs._coes.Count < rhs._coes.Count ? lhs : rhs;

            int maxCount = greaterPoly._coes.Count;
            int minCount = lowerPoly._coes.Count;

            List<T> nCoes = new List<T>(maxCount);
            T item = default;
            for (int i = 0; i < maxCount; i++)
            {
                if (i < minCount)
                {
                    item = OP.Add(greaterPoly[i], lowerPoly[i]);
                    nCoes.Add(item);
                    continue;
                }
                nCoes.Add(greaterPoly[i]);
            }
            return new Polynomial<T, C>(nCoes);
        }

        public static Polynomial<T,C> operator -(Polynomial<T,C> lhs, Polynomial<T,C> rhs)
        {
            int maxCount = Math.Max(lhs._coes.Count, rhs._coes.Count);
            int minCount = Math.Min(lhs._coes.Count, rhs._coes.Count);
            var greaterPoly = lhs._coes.Count >= rhs._coes.Count ? lhs : rhs;
            List<T> nCoes = new List<T>(maxCount);

            for (int i = 0; i < maxCount; i++)
            {
                if ( i < minCount)
                {
                    var item = OP.Sub(lhs[i], rhs[i]);
                    nCoes.Add(item);
                    continue;
                }
                nCoes.Add(greaterPoly[i]);
            }

            return new Polynomial<T, C>(nCoes);
        }

        public static Polynomial<T,C> operator *(Polynomial<T,C> lhs, Polynomial<T,C> rhs)
        {
            T[] coes = new T[lhs._coes.Count + rhs._coes.Count - 1];

            for (int i = 0; i < coes.Length; i++)
                coes[i] = default;

            int lCount = lhs._coes.Count;
            int rCount = rhs._coes.Count;

            for (int i = 0; i < lCount; i++)
                for (int j = 0; j < rCount; j++)
                    coes[i + j] = OP.Add(coes[i+j], OP.Mul(lhs[i], rhs[j]));

            return new Polynomial<T, C>(coes);
        }

        private static (List<T>, List<T>) SyntheticDivision(List<T> dividend, List<T> divisor)
        {
            List<T> output = dividend.ToList();

            output.Reverse();
            divisor = divisor.ToList();
            divisor.Reverse();

            T normalizer = divisor[0];

            for (int i = 0; i < dividend.Count() - (divisor.Count() - 1); i++)
            {
                output[i] = OP.Div(output[i], normalizer);

                T coef = output[i];
                if (coef.CompareTo(default(T)) != 0 )
                {
                    for (int j = 1; j < divisor.Count(); j++)
                        output[i + j] = OP.Add(output[i+j], OP.Neg(OP.Mul(divisor[j], coef)));
                }
            }

            int separator = output.Count() - (divisor.Count() - 1);
            output.Reverse();
            return (
                output.GetRange(output.Count() - separator, separator),
                output.GetRange(0, output.Count() - separator)
            );
        }

        public static (Polynomial<T,C>, Polynomial<T,C>) operator /(Polynomial<T,C> lhs, Polynomial<T,C> rhs)
        {
            (List<T> quotient, List<T> remainder) = SyntheticDivision(lhs._coes, rhs._coes);

            var q = new Polynomial<T, C>(quotient);
            var r = new Polynomial<T, C>(remainder);

            return (q, r);
        }

        public static Polynomial<T,C> operator +(Polynomial<T,C> lhs, T rhs)
        {
            Polynomial<T,C> polynomial = (lhs.Clone() as Polynomial<T,C>);

            polynomial[0] = OP.Add(lhs[0], rhs);

            return polynomial;
        }

        public static Polynomial<T,C> operator +(T lhs, Polynomial<T,C> rhs)
        {
            return rhs + lhs;
        }

        public static Polynomial<T,C> operator *(Polynomial<T,C> lhs, T rhs)
        {
            List<T> multipliedCoes = lhs._coes.Select(i => OP.Mul(i, rhs)).ToList();

            return new Polynomial<T,C>(multipliedCoes);
        }

        public static Polynomial<T,C> operator *(T lhs, Polynomial<T,C> rhs)
        {
            return rhs * lhs;
        }

        public static Polynomial<T,C> operator -(Polynomial<T,C> lhs, T rhs)
        {
            Polynomial<T,C> polynomial = (lhs.Clone() as Polynomial<T,C>);

            polynomial[0] = OP.Sub(lhs[0], rhs);

            return polynomial;
        }

        public static Polynomial<T,C> operator -(T lhs, Polynomial<T,C> rhs)
        {
            Polynomial<T, C> polynomial = (rhs.Clone() as Polynomial<T, C>);

            polynomial[0] = OP.Sub(lhs, rhs[0]);

            return polynomial;
        }

        public static Polynomial<T,C> operator /(Polynomial<T,C> lhs, T rhs)
        {
            if (rhs.CompareTo(default(T)) == 0) throw new DivideByZeroException();

            Polynomial<T,C> polynomial = ((lhs.Clone() as Polynomial<T,C>));
            polynomial._coes = lhs._coes.Select(i => OP.Div(i, rhs)).ToList();

            return polynomial;
        }

        public static Polynomial<T,C> operator /(T lhs, Polynomial<T,C> rhs)
        {

            Polynomial<T, C> polynomial = ((rhs.Clone() as Polynomial<T, C>));

            polynomial._coes = rhs._coes.Select( i => (i.CompareTo(default(T)) == 0) ? i : OP.Div(lhs, i)).ToList();

            return polynomial;
        }

        private static int Sign(double x)
        {
            return x < 0.0 ? -1 : x > 0.0 ? 1 : 0;
        }

        public void PrintRoots(double lowerBound, double upperBound, double step)
        {
            double x = lowerBound, 
                   ox = x;
            double y = (double)(object) Calculate((T)(object) x),
                   oy = y;
            int s = Sign(y), 
                os = s;

            for (; x <= upperBound; x += step)
            {
                s = Sign(y = (double)(object)Calculate((T)(object)x));
                if (s == 0)
                {
                    Console.WriteLine(x);
                }
                else if (s != os)
                {
                    var dx = x - ox;
                    var dy = y - oy;
                    var cx = x - dx * (y / dy);
                    Console.WriteLine("~{0}", cx);
                }

                ox = x;
                oy = y;
                os = s;
            }
        }
    }
}
