using System.Collections.Generic;

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
        get => Value[index];
        set
        {
            if(CheckIndex(index))
            {
                if (!Value[index].Equals(value))
                {
                    Value[index] = value;
                    onModifyElement?.Invoke(value);
                }
            }
        }
    }

    public virtual void Add(T element)
    {
        Value.Add(element);
        onAddValue?.Invoke(element);
    }

    public virtual void Insert(int index, T element)
    {
        if (CheckIndex(index))
        {
            Value.Insert(index, element);
            onAddValue?.Invoke(element);  

        }
    }

    public virtual void InsertRange(int index, IEnumerable<T> elements)
    {
        if (CheckIndex(index))
        {       
            foreach (T element in elements)
            {
                Value.Insert(index, element);
                onAddValue?.Invoke(element);
                index++;
            }

        }
    }

    public virtual void Remove(T element)
    {
        if (Value.Exists(e => e.Equals(element)))
        {

            Value.Remove(element);
            onRemoveValue?.Invoke(element);
        }
    }

    public virtual void RemoveAt(int index)
    {
        if (CheckIndex(index))
        {
            T removedItem = Value[index];
            Value.RemoveAt(index);
            onRemoveValue?.Invoke(removedItem);

        }
    }

    public virtual void RemoveRange(int index, int count)
    {
        if (CheckIndex(index))
        {
            List<T> removedItems = Value.GetRange(index, count);

            foreach (T removedItem in removedItems)
            {
                if(Value.Exists(e => e.Equals(removedItem)))
                {
                    Value.RemoveAt(index);
                    onRemoveValue?.Invoke(removedItem);
                    index++;

                }
            }
        }
    }

    private bool CheckIndex(int index)
    {
        if(index > -1 && index <= Value.Count - 1)
        {
            return true;
        }
        else
        {
            throw new System.ArgumentOutOfRangeException("index");
        }
    }
}
