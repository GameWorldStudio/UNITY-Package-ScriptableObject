using System.Collections.Generic;

namespace ScriptableClass.Core
{
      public abstract class AListEvent<T> : AValueEvent<List<T>>
      {
            public delegate void OnAddValue(T value);
            public event OnAddValue onAddValue;

            public delegate void OnRemoveValue(T value);
            public event OnRemoveValue onRemoveValue;

            public delegate void OnModifyElement(T value);
            public event OnModifyElement onModifyElement;

            public T this[int index]
            {
                  get => this.Value[index];
                  set
                  {
                        if (CheckIndex(index) && !this.Value[index].Equals(value))
                        {
                              this.Value[index] = value;
                              onModifyElement?.Invoke(value);
                        }
                  }
            }

            public virtual void Add(T element)
            {
                  this.Value.Add(element);
                  onAddValue?.Invoke(element);
            }

            public virtual void Insert(int index, T element)
            {
                  if (CheckIndex(index))
                  {
                        this.Value.Insert(index, element);
                        onAddValue?.Invoke(element);
                  }
            }

            public virtual void InsertRange(int index, IEnumerable<T> elements)
            {
                  if (CheckIndex(index))
                  {
                        foreach (T element in elements)
                        {
                              this.Value.Insert(index, element);
                              onAddValue?.Invoke(element);
                              index++;
                        }
                  }
            }

            public virtual void Remove(T element)
            {
                  if (this.Value.Exists(e => e.Equals(element)))
                  {
                        this.Value.Remove(element);
                        onRemoveValue?.Invoke(element);
                  }
            }

            public virtual void RemoveAt(int index)
            {
                  if (CheckIndex(index))
                  {
                        T removedItem = this.Value[index];
                        this.Value.RemoveAt(index);
                        onRemoveValue?.Invoke(removedItem);
                  }
            }

            public virtual void RemoveRange(int index, int count)
            {
                  if (CheckIndex(index))
                  {
                        List<T> removedItems = this.Value.GetRange(index, count);

                        foreach (T removedItem in removedItems)
                        {
                              if (this.Value.Exists(e => e.Equals(removedItem)))
                              {
                                    this.Value.RemoveAt(index);
                                    onRemoveValue?.Invoke(removedItem);
                                    index++;
                              }
                        }
                  }
            }

            private bool CheckIndex(int index)
            {
                  if (index > -1 && index <= this.Value.Count - 1)
                  {
                        return true;
                  }
                  else
                  {
                        throw new System.ArgumentOutOfRangeException(nameof(index));
                  }
            }
      }
}