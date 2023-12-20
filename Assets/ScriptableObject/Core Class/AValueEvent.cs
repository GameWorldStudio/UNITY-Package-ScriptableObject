using UnityEngine;

public abstract class AValueEvent<T> : ScriptableObject
{
    public delegate void OnValueChange(T value);
    public event OnValueChange onValueChange;

    [SerializeField] private T value;

    public T Value
    {
        get => this.value;
        set
        {
            this.value = value;
            onValueChange?.Invoke(this.value);
        }
    }
}
