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
        private static readonly IMathOperations<T> _calculator = new C();

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
                
                var sign = (_coes[i].CompareTo(default(T))) == -1 ? " - " : " + ";
                
                builder.AppendFormat("{0}{1}{2}",
                    i > 0 ? _coes[i].ToString() + "x" : _coes[i].ToString(),
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
                result = _calculator.Add(result, _calculator.Mul(_coes[i], _calculator.Pow(x, (double)i)));

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
                    item = _calculator.Add(greaterPoly[i], lowerPoly[i]);
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
                    var item = _calculator.Sub(lhs[i], rhs[i]);
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
                    coes[i + j] = _calculator.Add(coes[i+j], _calculator.Mul(lhs[i], rhs[j]));

            return new Polynomial<T, C>(coes);
        }

        public static Polynomial<T,C> operator /(Polynomial<T,C> lhs, Polynomial<T,C> rhs)
        {
            throw new NotImplementedException();
        }

        public static Polynomial<T,C> operator +(Polynomial<T,C> lhs, T rhs)
        {
            Polynomial<T,C> polynomial = (lhs.Clone() as Polynomial<T,C>);

            polynomial[0] = _calculator.Add(lhs[0], rhs);

            return polynomial;
        }

        public static Polynomial<T,C> operator +(T lhs, Polynomial<T,C> rhs)
        {
            return rhs + lhs;
        }

        public static Polynomial<T,C> operator *(Polynomial<T,C> lhs, T rhs)
        {
            List<T> multipliedCoes = lhs._coes.Select(i => _calculator.Mul(i, rhs)).ToList();

            return new Polynomial<T,C>(multipliedCoes);
        }

        public static Polynomial<T,C> operator *(T lhs, Polynomial<T,C> rhs)
        {
            return rhs * lhs;
        }

        public static Polynomial<T,C> operator -(Polynomial<T,C> lhs, T rhs)
        {
            Polynomial<T,C> polynomial = (lhs.Clone() as Polynomial<T,C>);

            polynomial[0] = _calculator.Sub(lhs[0], rhs);

            return polynomial;
        }

        public static Polynomial<T,C> operator -(T lhs, Polynomial<T,C> rhs)
        {
            Polynomial<T, C> polynomial = (rhs.Clone() as Polynomial<T, C>);

            polynomial[0] = _calculator.Sub(lhs, rhs[0]);

            return polynomial;
        }

        public static Polynomial<T,C> operator /(Polynomial<T,C> lhs, T rhs)
        {
            if (rhs.CompareTo(default(T)) == 0) throw new DivideByZeroException();

            Polynomial<T,C> polynomial = ((lhs.Clone() as Polynomial<T,C>));
            polynomial._coes = lhs._coes.Select(i => _calculator.Div(i, rhs)).ToList();

            return polynomial;
        }

        public static Polynomial<T,C> operator /(T lhs, Polynomial<T,C> rhs)
        {

            Polynomial<T, C> polynomial = ((rhs.Clone() as Polynomial<T, C>));

            polynomial._coes = rhs._coes.Select( i => (i.CompareTo(default(T)) == 0) ? i : _calculator.Div(lhs, i)).ToList();

            return polynomial;
        }
    }
}
