using System;
using System.Collections.Generic;

namespace Osiris.Tfs.Monitor.Models.Utilities
{
    public static class HashSetHelper<T>
    {
        class Wrapper<TValue> : IEqualityComparer<T>
        {
            private readonly Func<T, TValue> func;
            private readonly IEqualityComparer<TValue> comparer;
            public Wrapper(Func<T, TValue> func,
                IEqualityComparer<TValue> comparer)
            {
                this.func = func;
                this.comparer = comparer ?? EqualityComparer<TValue>.Default;
            }
            public bool Equals(T x, T y)
            {
                return comparer.Equals(func(x), func(y));
            }

            public int GetHashCode(T obj)
            {
                return comparer.GetHashCode(func(obj));
            }
        }
        public static HashSet<T> Create<TValue>(Func<T, TValue> func)
        {
            return new HashSet<T>(new Wrapper<TValue>(func, null));
        }
        public static HashSet<T> Create<TValue>(Func<T, TValue> func,
            IEqualityComparer<TValue> comparer)
        {
            return new HashSet<T>(new Wrapper<TValue>(func, comparer));
        }
    }
}