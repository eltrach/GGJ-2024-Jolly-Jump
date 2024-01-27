namespace VTemplate
{
    public delegate void PoolEvent<T>(T t);
    
    
    public interface IPool<T>
    {
        /// <summary>
        /// Event you can subscribe to, it's called every time an object has been picked from the pool, giving the object as parameter
        /// </summary>
        event PoolEvent<T> OnObjectPicked;
        /// <summary>
        /// Event you can subscribe to, it's called every time an object has been returned to the pool, giving the object as parameter
        /// </summary>
        event PoolEvent<T> OnObjectReturned;
        
        /// <summary>
        /// Represents the prefab the pool is using, setup in the constructor
        /// </summary>
        T Prefab { get; }
        /// <summary>
        /// Represents the number of object that will be instantiated at Init()
        /// </summary>
        int NumberToInstantiate { get; }

        /// <summary>
        /// Used to setup the pool, it must be called before using the pool
        /// </summary>
        void Init();
        /// <summary>
        /// Used to get an object from the pool, if no object is available, it will create a new one
        /// </summary>
        /// <returns>The pool instance</returns>
        T Pick();
        /// <summary>
        /// Used to return an object to the pool
        /// </summary>
        /// <param name="instance">Instance to return</param>
        void Return(T instance);
        /// <summary>
        /// Used to return an object to the pool with a delay
        /// </summary>
        /// <param name="instance">Instance to return</param>
        /// <param name="delay">Delay before the object is returned</param>
        void ReturnDelayed(T instance, float delay);
        /// <summary>
        /// Used to return all instances outside the pool to the pool
        /// </summary>
        void ReturnAll();
        /// <summary>
        /// Used to destroy the pool, you will have to call Init() again to use the pool
        /// </summary>
        void Clear();
    }
}