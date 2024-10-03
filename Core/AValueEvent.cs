using System;
using UnityEngine;

namespace ScriptableClass.Core
{
      public abstract class AValueEvent<T> : ScriptableObject
      {
            public event Action<T> OnValueChange;

            [SerializeField] private T value;

            public T Value
            {
                  get => this.value;
                  set
                  {
                        this.value = value;
                        OnValueChange?.Invoke(this.value);
                  }
            }
      }
}