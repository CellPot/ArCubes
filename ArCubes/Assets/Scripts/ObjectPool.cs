using System.Collections.Generic;

public class ObjectPool<T>
{
    private readonly Stack<T> objectsPool;
    private readonly System.Func<T> objectFactory;

    public ObjectPool(System.Func<T> objectFactory, int initialSize)
    {
        this.objectFactory = objectFactory;
        objectsPool = new Stack<T>(initialSize);
        for (var i = 0; i < initialSize; i++)
            objectsPool.Push(objectFactory());
    }

    public Stack<T> ObjectsPool => objectsPool;

    public T GetObjectFromPool()
    {
        if (objectsPool.Count == 0) objectsPool.Push(objectFactory());

        return objectsPool.Pop();
    }

    public void ReturnObjectToPool(T obj) =>
        objectsPool.Push(obj);
}