# Pool

## Description

The **Pool** is a classic pooling system that you can use for any kind of MonoBehaviour.\
The interface used is **IPool** and the implementation is **PoolImpl**.

## Usage

To create a Pool, you can declare the interface IPool as a variable in your script, giving it the type of the object you want to pool :

```plaintext
IPool<MyPoolObject> _myPool;
```

Then you must create the object and give it the prefab you want to use, the number to instantiate and a MonoBehaviour that will be his parent, for example a PoolManager :

```plaintext
public class PoolManager : MonoBehaviour
{
   [SerializeField] MyPoolObject _myPoolObject;
   
   public IPool<MyPoolObject> MyPool { get; private set; }

   void Init ()
   {
      MyPool = new PoolImpl<MyPoolObject>(_myPoolObject, 10, this);
   }
}
```

Once the pool is created, there are a few methods that you can use :

```plaintext
event PoolEvent<T> OnObjectPicked;
event PoolEvent<T> OnObjectReturned;

T Prefab { get; }
int NumberToInstantiate { get; }

void Init();
T Pick();
void Return(T instance);
void ReturnDelayed(T instance, float delay);
void ReturnAll();
void Clear();
```

* **OnObjectPicked** is an event you can subscribe to, it's called every time an object has been picked from the pool, giving the object as parameter
* **OnObjectReturned** is an event you can subscribe to, it's called every time an object has been returned to the pool, giving the object as parameter
* **Prefab** represents the prefab the pool is using
* **NumberToInstantiate** represents the number of object that will be instantiated at Init()
* **Init** is used to setup the pool, called when creating the object, you will need to use it again if you call Clear()
* **Pick** is used to get an object from the pool, if no object is available, it will create a new one and return it
* **Return** is used to return an object to the pool
* **ReturnDelayed** is used to return an object to the pool with a delay (nice with ParticleSystems for example)
* **ReturnAll** is used to return all instances outside the pool to the pool
* **Clear** is used to destroy the pool, you will have to call Init() again to use the pool