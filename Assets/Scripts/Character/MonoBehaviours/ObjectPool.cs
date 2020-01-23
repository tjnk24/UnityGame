using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    protected GameObject prefab;
    protected int initialPoolCount = 8;

    static public Dictionary<GameObject, ObjectPool> PoolInstanses = new Dictionary<GameObject, ObjectPool>();

    public List<PoolingObject> pool = new List<PoolingObject>();

    void Start()
    {
        for (int i = 0; i < initialPoolCount; i++)
        {
            PoolingObject newPoolObject = CreateNewPoolObject();
            pool.Add(newPoolObject);
        }
    }

    static public ObjectPool GetObjectPool(GameObject prefab, int initialPoolCount = 8)
    {
        ObjectPool objPool = null;
        //если в словаре PoolInstances нет буллетпула с такими префабами, то добавить буллетпул
        if(!PoolInstanses.TryGetValue(prefab, out objPool))
        {
            //пустой игровой обьект с именем "имяпрефаба + _Pool"
            GameObject obj = new GameObject(prefab.name + "_Pool");

            objPool = obj.AddComponent<ObjectPool>();
            objPool.prefab = prefab;
            objPool.initialPoolCount = initialPoolCount;

            PoolInstanses[prefab] = objPool;
        }

        return objPool;
    }

    protected PoolingObject CreateNewPoolObject()
    {
        PoolingObject newPoolObject = new PoolingObject();
        newPoolObject.instance = Instantiate(prefab);
        newPoolObject.instance.transform.SetParent(transform);
        newPoolObject.inPool = true;
        newPoolObject.SetReferences(this);
        newPoolObject.Sleep();
        return newPoolObject;
    }

    //класс-обертка для префаба, чтобы взаимодействовать с пулом объектов
    public PoolingObject Pop(Vector2 info)
    {
        //поиск свободного объекта в пуле и его возвращение
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].inPool)
            {
                pool[i].inPool = false;
                pool[i].WakeUp(info);
                return pool[i];
            }
        }

        //если объекта нету, то создание нового объекта и его возвращение
        PoolingObject newPoolObject = CreateNewPoolObject();
        pool.Add(newPoolObject);
        newPoolObject.inPool = false;
        newPoolObject.WakeUp(info);
        return newPoolObject;
    }

    public void Push(PoolingObject poolObject)
    {
        poolObject.inPool = true;
        poolObject.Sleep();
    }
}

public class PoolingObject
{
    public bool inPool;
    public GameObject instance;
    public ObjectPool objPool;
    public PoolingComponent projectile;

    public Transform transform;
    //public Animator animator;
    public Rigidbody2D rigidbody2D;
    //public SpriteRenderer spriteRenderer;

    public void SetReferences(ObjectPool pool)
    {
        objPool = pool;
        projectile = instance.GetComponent<PoolingComponent>();

        transform = instance.transform;
        //animator = instance.GetComponent<Animator>();
        rigidbody2D = instance.GetComponent<Rigidbody2D>();
        projectile.poolingObject = this;
        //spriteRenderer = instance.GetComponent<SpriteRenderer>();
    }
    public void WakeUp(Vector2 position)
    {
        transform.position = position;
        instance.SetActive(true);
    }
    public void Sleep()
    {
        instance.SetActive(false);
    }

    public void ReturnToPool()
    {
        PoolingObject thisObject = this;
        objPool.Push(thisObject);
    }
}
