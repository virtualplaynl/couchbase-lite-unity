using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Couchbase.Lite.Support;
using UnityEngine;

public class UnityTaskScheduler : MonoBehaviour
{
    Thread mainThread;
    Queue<Task> taskQueue;
    internal MainThreadTaskScheduler MainThreadTaskScheduler;

    void Awake()
    {
        mainThread = Thread.CurrentThread;
        taskQueue = new Queue<Task>();
    }

    public bool OnMainThread => mainThread.Equals(Thread.CurrentThread);

    public void Queue(Task task)
    {
        taskQueue.Enqueue(task);
    }

    public IEnumerable<Task> GetTasks()
    {
        return taskQueue;
    }

    void FixedUpdate()
    {
        while(taskQueue.Count > 0) {
            MainThreadTaskScheduler.Execute(taskQueue.Dequeue());
        }
    }
}
