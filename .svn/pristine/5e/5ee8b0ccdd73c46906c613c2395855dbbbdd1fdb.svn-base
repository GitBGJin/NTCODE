#region Header

/**
 *
 * The following code can be found at:
 * http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp01212002.asp
 *
**/

#endregion Header

namespace SmartEP.Service.Core.Cached
{
    using System;
    using System.Collections;

    /// <summary>
    /// Gives us a handy way to modify a collection while we're iterating through it.
    /// </summary>
    public class IteratorIsolateCollection : IEnumerable
    {
        #region Fields

        IEnumerable _enumerable;

        #endregion Fields

        #region Constructors

        public IteratorIsolateCollection(IEnumerable enumerable)
        {
            _enumerable = enumerable;
        }

        #endregion Constructors

        #region Methods

        public IEnumerator GetEnumerator()
        {
            return new IteratorIsolateEnumerator(_enumerable.GetEnumerator());
        }

        #endregion Methods

        #region Nested Types

        internal class IteratorIsolateEnumerator : IEnumerator
        {
            #region Fields

            int currentItem;
            ArrayList items = new ArrayList();

            #endregion Fields

            #region Constructors

            internal IteratorIsolateEnumerator(IEnumerator enumerator)
            {
                while (enumerator.MoveNext() != false)
                {
                    items.Add(enumerator.Current);
                }
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
                currentItem = -1;
            }

            #endregion Constructors

            #region Properties

            public object Current
            {
                get
                {
                    return items[currentItem];
                }
            }

            #endregion Properties

            #region Methods

            public bool MoveNext()
            {
                currentItem++;
                if (currentItem == items.Count)
                    return false;

                return true;
            }

            public void Reset()
            {
                currentItem = -1;
            }

            #endregion Methods
        }

        #endregion Nested Types
    }
}