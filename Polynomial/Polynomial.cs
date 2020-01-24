using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra
{
    public class Polynomial<T> : ICloneable, IComparable<Polynomial<T>>
        where T : struct, IComparable
    {
        private List<T> _coes;

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
            return new Polynomial<T>(_coes.ToList());
        }

        public int CompareTo(Polynomial<T> other)
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

        public static Polynomial<T> operator +(Polynomial<T> lhs, Polynomial<T> rhs)
        {
            throw new NotImplementedException();
        }

        public static Polynomial<T> operator -(Polynomial<T> lhs, Polynomial<T> rhs)
        {
            throw new NotImplementedException();
        }

        public static Polynomial<T> operator *(Polynomial<T> lhs, Polynomial<T> rhs)
        {
            throw new NotImplementedException();
        }

        public static Polynomial<T> operator /(Polynomial<T> lhs, Polynomial<T> rhs)
        {
            throw new NotImplementedException();
        }

        public static Polynomial<T> operator +(Polynomial<T> lhs, T rhs)
        {
            throw new NotImplementedException();
        }

        public static Polynomial<T> operator +(T lhs, Polynomial<T> rhs)
        {
            return rhs + lhs;
        }

        public static Polynomial<T> operator *(Polynomial<T> lhs, T rhs)
        {
            throw new NotImplementedException();
        }

        public static Polynomial<T> operator *(T lhs, Polynomial<T> rhs)
        {
            return rhs + lhs;
        }

        public static Polynomial<T> operator -(Polynomial<T> lhs, T rhs)
        {
            throw new NotImplementedException();
        }

        public static Polynomial<T> operator -(T lhs, Polynomial<T> rhs)
        {
            throw new NotImplementedException();
        }

        public static Polynomial<T> operator /(Polynomial<T> lhs, T rhs)
        {
            throw new NotImplementedException();
        }

        public static Polynomial<T> operator /(T lhs, Polynomial<T> rhs)
        {
            throw new NotImplementedException();
        }
    }
}
