using UnityEngine;

namespace Utils.SerializableUtils
{
    [System.Serializable]
    public struct SerializableNullable<T> where T : struct
    {
        [SerializeField] private T value;
        [SerializeField] private bool hasValue;
        
        public T Value
        {
            get
            {
                if (!HasValue)
                    throw new System.InvalidOperationException("Serializable nullable object must have a value.");
                
                return value;
            }
        }

        public bool HasValue => hasValue;

        public SerializableNullable(bool hasValue, T value)
        {
            this.value = value;
            this.hasValue = hasValue;
        }

        private SerializableNullable(T value)
        {
            this.value = value;
            this.hasValue = true;
        }

        public static implicit operator SerializableNullable<T>(T value)
        {
            return new SerializableNullable<T>(value);
        }

        public static implicit operator SerializableNullable<T>(T? value)
        {
            return value.HasValue ? new SerializableNullable<T>(value.Value) : new SerializableNullable<T>();
        }

        public static implicit operator T?(SerializableNullable<T> value)
        {
            return value.HasValue ? (T?) value.Value : null;
        }
    }
}