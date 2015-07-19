// -----------------------------------------------------------------------
// <copyright file="PerspexSingleItemList.cs" company="Steven Kirk">
// Copyright 2014 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Implements the <see cref="IPerspexReadOnlyList{T}"/> interface for single items.
    /// </summary>
    /// <typeparam name="T">The type of the single item.</typeparam>
    /// <remarks>
    /// Classes such as Border can only ever have a single logical child, but they need to 
    /// implement a list of logical children in their ILogical.LogicalChildren property using the
    /// <see cref="IPerspexReadOnlyList{T}"/> interface. This class facilitates that
    /// without creating an actual <see cref="PerspexList{T}"/>.
    /// </remarks>
    public class PerspexSingleItemList<T> : IPerspexList<T> where T : class
    {
        private T item;

        /// <summary>
        /// An empty <see cref="PerspexSingleItemList{T}"/>.
        /// </summary>
        public static readonly IPerspexReadOnlyList<T> Empty = new PerspexSingleItemList<T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PerspexSingleItemList{T}"/> class.
        /// </summary>
        public PerspexSingleItemList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerspexSingleItemList{T}"/> class.
        /// </summary>
        /// <param name="item">The initial item.</param>
        public PerspexSingleItemList(T item)
        {
            this.item = item;
        }

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public int Count
        {
            get { return this.SingleItem != null ? 1 : 0; }
        }

        /// <summary>
        /// Gets or sets the object's single item. Null represents an empty list.
        /// </summary>
        public T SingleItem
        {
            get
            {
                return this.item;
            }

            set
            {
                NotifyCollectionChangedEventArgs e = null;
                bool countChanged = false;

                if (value == null && this.item != null)
                {
                    e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, this.item, 0);
                    this.item = null;
                    countChanged = true;
                }
                else if (value != null && this.item == null)
                {
                    e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, this.item, 0);
                    this.item = value;
                    countChanged = true;
                }
                else
                {
                    e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, this.item, 0);
                    this.item = value;
                }

                if (e != null && this.CollectionChanged != null)
                {
                    this.CollectionChanged(this, e);
                }

                if (countChanged && this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Count"));
                }
            }
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return this.item;
            }

            set
            {
                // TODO: Implement
            }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return null; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return false; }
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set { this[index] = (T)value; }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>The iterator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return Enumerable.Repeat(this.SingleItem, this.Count).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>The iterator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection<T>)this).CopyTo((T[])array, index);
        }

        int IList.Add(object value)
        {
            ((ICollection<T>)this).Add((T)value);
            return 0;
        }

        void IList.Clear()
        {
            ((ICollection<T>)this).Clear();
        }

        bool IList.Contains(object value)
        {
            return ((ICollection<T>)this).Contains((T)value);
        }

        int IList.IndexOf(object value)
        {
            return ((IList<T>)this).IndexOf((T)value);
        }

        void IList.Insert(int index, object value)
        {
            ((IList<T>)this).Insert(index, (T)value);
        }

        void IList.Remove(object value)
        {
            ((ICollection<T>)this).Remove((T)value);
        }

        void IList.RemoveAt(int index)
        {
            ((IList<T>)this).RemoveAt(index);
        }

        void ICollection<T>.Add(T item)
        {
            if (this.SingleItem != null)
            {
                throw new InvalidOperationException("Collection can only contain a single item");
            }

            this.SingleItem = item;
        }

        void ICollection<T>.Clear()
        {
            this.SingleItem = null;
        }

        bool ICollection<T>.Contains(T item)
        {
            return this.SingleItem != null && this.SingleItem == item;
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            if (this.SingleItem != null)
            {
                array[arrayIndex] = this.SingleItem;
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            if (this.SingleItem == item)
            {
                this.SingleItem = null;
                return true;
            }

            return false;
        }

        int IList<T>.IndexOf(T item)
        {
            return this.SingleItem != null && this.SingleItem == item ? 0 : -1;
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        void IPerspexList<T>.AddRange(IEnumerable<T> items)
        {
            foreach (var i in items)
            {
                ((IList<T>)this).Add(i);
            }
        }

        void IPerspexList<T>.RemoveAll(IEnumerable<T> items)
        {
            foreach (var i in items)
            {
                ((IList<T>)this).Remove(i);
            }
        }
    }
}