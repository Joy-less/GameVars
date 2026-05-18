#if NET
using System.Numerics;
#endif

namespace GameVars;

/// <summary>
/// Extensions for <see cref="IGameVar{T}"/>.
/// </summary>
public static class GameVarExtensions {
    extension(IGameVar<bool> GameVar) {
        /// <summary>
        /// Sets the value of the game var to the opposite of its current value (if false, sets to true, if true, sets to false).
        /// </summary>
        public void Toggle() {
            GameVar.Set(!GameVar.Get());
        }
    }
    extension(IGameVar<string> GameVar) {
        /// <summary>
        /// Sets the value of the game var to its current value with the given string appended.
        /// </summary>
        public void Add(string Value) {
            GameVar.Set(GameVar.Get() + Value);
        }
        /// <summary>
        /// Sets the value of the game var to its current value with a stringified version of the given object appended.
        /// </summary>
        public void Add(object? Value) {
            GameVar.Set(GameVar.Get() + Value);
        }
    }
    extension<T, TCollection>(IGameVar<TCollection> GameVar) where TCollection : ICollection<T> {
        /// <summary>
        /// Sets the value of the game var to its current value with the given item added to the collection.
        /// </summary>
        public void Add(T Value) {
            TCollection Collection = GameVar.Get();
            Collection.Add(Value);
            GameVar.Set(Collection);
        }
        /// <summary>
        /// Sets the value of the game var to its current value with the given item removed from the collection.
        /// </summary>
        public bool Remove(T Value) {
            TCollection Collection = GameVar.Get();
            bool Success = Collection.Remove(Value);
            GameVar.Set(Collection);
            return Success;
        }
        /// <summary>
        /// Sets the value of the game var to its current value with all items removed.
        /// </summary>
        public void Clear() {
            TCollection Collection = GameVar.Get();
            Collection.Clear();
            GameVar.Set(Collection);
        }
    }
#if NET
    extension<T>(IGameVar<T> GameVar) where T : IIncrementOperators<T> {
        /// <summary>
        /// Sets the value of the game var to its current value incremented by 1.
        /// </summary>
        public void Increment() {
            T Value = GameVar.Get();
            Value++;
            GameVar.Set(Value);
        }
    }
    extension<T>(IGameVar<T> GameVar) where T : IDecrementOperators<T> {
        /// <summary>
        /// Sets the value of the game var to its current value decremented by 1.
        /// </summary>
        public void Decrement() {
            T Value = GameVar.Get();
            Value--;
            GameVar.Set(Value);
        }
    }
    extension<T>(IGameVar<T> GameVar) where T : IAdditionOperators<T, T, T> {
        /// <summary>
        /// Sets the value of the game var to the given value added to the current value.
        /// </summary>
        public void Add(T Value) {
            GameVar.Set(GameVar.Get() + Value);
        }
    }
    extension<T>(IGameVar<T> GameVar) where T : ISubtractionOperators<T, T, T> {
        /// <summary>
        /// Sets the value of the game var to the given value subtracted from the current value.
        /// </summary>
        public void Subtract(T Value) {
            GameVar.Set(GameVar.Get() - Value);
        }
    }
    extension<T>(IGameVar<T> GameVar) where T : IMultiplyOperators<T, T, T> {
        /// <summary>
        /// Sets the value of the game var to the given value multiplied by the current value.
        /// </summary>
        public void Multiply(T Value) {
            GameVar.Set(GameVar.Get() * Value);
        }
    }
    extension<T>(IGameVar<T> GameVar) where T : IDivisionOperators<T, T, T> {
        /// <summary>
        /// Sets the value of the game var to the given value divided by the current value.
        /// </summary>
        public void Divide(T Value) {
            GameVar.Set(GameVar.Get() / Value);
        }
    }
    extension<T>(IGameVar<T> GameVar) where T : IModulusOperators<T, T, T> {
        /// <summary>
        /// Sets the value of the game var to the remainder of dividing the given value by the current value.
        /// </summary>
        public void Modulo(T Value) {
            GameVar.Set(GameVar.Get() % Value);
        }
    }
    extension<T>(IGameVar<T> GameVar) where T : IPowerFunctions<T> {
        /// <summary>
        /// Sets the value of the game var to the current value to the power of the given value.
        /// </summary>
        public void Pow(T Value) {
            GameVar.Set(T.Pow(GameVar.Get(), Value));
        }
    }
#endif
}