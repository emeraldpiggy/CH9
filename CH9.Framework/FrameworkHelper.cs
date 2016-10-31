using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CH9.Framework
{
    public static class FrameworkHelper
    {
        public static void ForEach<T>(this IEnumerable<T> iterableCollection, Action<T> action)
        {
            T[] itr;
            if (iterableCollection is T[])
            {
                itr = (T[])iterableCollection;
            }
            else if (iterableCollection is IList<T>)
            {
                var list = iterableCollection as IList<T>;
                var cnt = list.Count;
                for (var i = 0; i < cnt; i++)
                {
                    action(list[i]);
                }

                return;
            }
            else if (iterableCollection is IList)
            {
                var list = iterableCollection as IList;
                var cnt = list.Count;
                for (var i = 0; i < cnt; i++)
                {
                    action((T)list[i]);
                }
                return;
            }
            else
            {
                // Convert to array which will get the enumerator
                itr = iterableCollection.ToArray();
            }

            foreach (T t in itr)
            {
                action(t);
            }
        }


        public static void SafeDispose<T>(ref T disposable, Action<T> onDisposing = null) where T : class, IDisposable
        {
            if (disposable == null)
            {
                return;
            }

            try
            {
                onDisposing?.Invoke(disposable);
                disposable.Dispose();
            }
            finally
            {
                disposable = null;
            }
        }

        public static bool AreEqual<T>(this T reference, T value)
        {
            return EqualityComparer<T>.Default.Equals(reference, value);
        }
    }
}