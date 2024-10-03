using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ScriptableClass.Core
{
      public abstract class AListEvent<T> : AValueEvent<List<T>>, IList<T>
      {
            public event Action<T> OnAddValue;
            public event Action<T> OnRemoveValue;
            public event Action<T> OnModifyElement;

            public int Count => Value.Count;
            public bool IsReadOnly => false;

            public T this[int index]
            {
                  get => Value[index];
                  set
                  {
                        if (CheckIndex(index) && !Value[index].Equals(value))
                        {
                              Value[index] = value;
                              OnModifyElement?.Invoke(value);
                        }
                  }
            }

            // --------------------------------------------------------
            // Méthodes d'insertions
            // --------------------------------------------------------

            public virtual void Add(T item)
            {
                  Value.Add(item);
                  OnAddValue?.Invoke(item);
            }

            public virtual void Insert(int index, T item)
            {
                  if (CheckIndex(index, true))
                  {
                        Value.Insert(index, item);
                        OnAddValue?.Invoke(item);
                  }
            }

            public virtual void InsertRange(int index, IEnumerable<T> elements)
            {
                  if (CheckIndex(index, true))
                  {
                        IEnumerable<T> collection = elements as T[] ?? elements.ToArray();
                        Value.InsertRange(index, collection);

                        foreach (T element in collection)
                        {
                              OnAddValue?.Invoke(element);
                        }
                  }
            }

            // --------------------------------------------------------
            // Méthodes d'extractions
            // --------------------------------------------------------

            public void Clear()
            {
                  var removedElements = new List<T>(Value);
                  Value.Clear();

                  foreach (T element in removedElements)
                  {
                        OnRemoveValue?.Invoke(element);
                  }
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                  Value.CopyTo(array, arrayIndex);
            }

            public bool Remove(T item)
            {
                  bool removed = Value.Remove(item);

                  if (removed)
                  {
                        OnRemoveValue?.Invoke(item);
                  }
                  return removed;
            }

            public virtual void RemoveAt(int index)
            {
                  if (CheckIndex(index))
                  {
                        T removedItem = Value[index];
                        Value.RemoveAt(index);
                        OnRemoveValue?.Invoke(removedItem);
                  }
            }

            public virtual void RemoveRange(int index, int count)
            {
                  if (CheckIndex(index))
                  {
                        List<T> removedItems = Value.GetRange(index, count);
                        Value.RemoveRange(index, count);

                        foreach (T removedItem in removedItems)
                        {
                              OnRemoveValue?.Invoke(removedItem);
                        }
                  }
            }

            // --------------------------------------------------------
            // Méthodes helpers
            // --------------------------------------------------------

            public bool Contains(T item)
            {
                  return Value.Contains(item);
            }

            public IEnumerator<T> GetEnumerator()
            {
                  return Value.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                  return GetEnumerator();
            }

            public int IndexOf(T item)
            {
                  return Value.IndexOf(item);
            }

            private bool CheckIndex(int index, bool allowEqualCount = false)
            {
                  int maxIndex = allowEqualCount ? Value.Count : Value.Count - 1;

                  if (index >= 0 && index <= maxIndex)
                  {
                        return true;
                  }
                  else
                  {
                        throw new ArgumentOutOfRangeException(nameof(index));
                  }
            }
      }
}