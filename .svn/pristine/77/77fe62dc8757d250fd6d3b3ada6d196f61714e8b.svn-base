
#region Usings
using System;
using System.Collections.Generic;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System.Linq;

#endregion

namespace SmartEP.Utilities.DataTypes
{
    /// <summary>
    /// Used to count the number of times something is added to the list
    /// </summary>
    public class Bag<T> : ICollection<T>
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Bag()
        {
            Items = new Dictionary<T, int>();
        }

        #endregion

        #region ICollection<T> Members

        public virtual void Add(T item)
        {
            if (Items.ContainsKey(item))
                ++Items[item];
            else
                Items.Add(item, 1);
        }

        public virtual void Clear()
        {
            Items.Clear();
        }

        public virtual bool Contains(T item)
        {
            return Items.ContainsKey(item);
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="array">Not used</param>
        /// <param name="arrayIndex">Not used</param>
        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(this.Items.ToList().ToArray(x => x.Key), 0, array, arrayIndex, this.Count);
        }

        public virtual int Count
        {
            get { return Items.Count; }
        }

        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        public virtual bool Remove(T item)
        {
            return Items.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public virtual IEnumerator<T> GetEnumerator()
        {
            foreach (T Key in this.Items.Keys)
                yield return Key;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (T Key in this.Items.Keys)
                yield return Key;
        }

        #endregion

        #region Properties

        public virtual int this[T index]
        {
            get { return Items[index]; }
            set { Items[index] = value; }
        }

        /// <summary>
        /// Actual internal container
        /// </summary>
        protected virtual Dictionary<T, int> Items { get; set; }

        #endregion
    }
}