using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra
{
    public class Polynomial<T,C> : ICloneable, IComparable<Polynomial<T,C>>
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
                if (_coes[i].CompareTo(default(T)) == 0 ) continue;

                builder.AppendFormat("{0}{1}{2}", 
                    i > 0 ? _coes[i].ToString() + "x" : _coes[i].ToString(),
                    i > 1 ? "^"+(i) : "",
                    i < (_coes.Count - 1)? " + " : "");
            }

            return builder.ToString();
        }
        public object Clone()
        {
            return new Polynomial<T,C>(_coes.ToList());
        }

        public int CompareTo(Polynomial<T,C> other)
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
            throw new NotImplementedException();
        }

        public static Polynomial<T,C> operator +(Polynomial<T,C> lhs, Polynomial<T,C> rhs)
        {
            throw new NotImplementedException();
        }

        public static Polynomial<T,C> operator -(Polynomial<T,C> lhs, Polynomial<T,C> rhs)
        {
            throw new NotImplementedException();
        }

        public static Polynomial<T,C> operator *(Polynomial<T,C> lhs, Polynomial<T,C> rhs)
        {
            throw new NotImplementedException();
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
