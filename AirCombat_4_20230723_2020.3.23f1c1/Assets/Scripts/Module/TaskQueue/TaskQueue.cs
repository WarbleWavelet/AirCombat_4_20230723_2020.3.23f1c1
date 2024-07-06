using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskQueue
{
    private int _addValueTimes;
    private int _id;
    private Action<object[]> _onComplete;
    private readonly Queue<Action<TaskQueue, int>> _taskQueue;
    private object[] _values;

    public TaskQueue()
    {
        _taskQueue = new Queue<Action<TaskQueue, int>>();
        ResetData();
    }


    #region 辅助 public   


    public void Add(Action<TaskQueue, int> task)
    {
        _taskQueue.Enqueue(task);
    }

    public void Execute(Action<object[]> complete)
    {
        _onComplete = complete;
        _values = new object[_taskQueue.Count];

        while (_taskQueue.Count > 0)
        {
            _id++;
            var task = _taskQueue.Dequeue();
            task.DoIfNotNull(this, _id);
        }

        ResetData();
    }

    public void AddValue(int id, object value)
    {
        _addValueTimes++;
        _values[id] = value;
        JudgeComplete();
    }
    #endregion


    #region 辅助
    private void ResetData()
    {
        _id = -1;
        _addValueTimes = 0;
    }


    private void JudgeComplete()
    {
        if (_addValueTimes == _values.Length)
        {
            _onComplete.DoIfNotNull(_values);
        }
        else if (_addValueTimes > _values.Length)
        {
            Debug.LogError("AddValue执行次数过多");
        }
    }
    #endregion
}

public class TaskQueue<T>
{
    private int _addValueTimes;
    private int _id;
    private Action<T[]> _onComplete;
    private readonly Queue<Action<TaskQueue<T>, int>> _taskQueue;
    private T[] _values;

    public TaskQueue()
    {
        _taskQueue = new Queue<Action<TaskQueue<T>, int>>();
        _id = -1;
        _addValueTimes = 0;
    }


    #region 辅助 public
    public void Add(Action<TaskQueue<T>, int> task)
    {
        _taskQueue.Enqueue(task);
    }

    public void Execute(Action<T[]> complete)
    {
        _onComplete = complete;
        _values = new T[_taskQueue.Count];

        while (_taskQueue.Count > 0)
        {
            _id++;
            var task = _taskQueue.Dequeue();
            task.DoIfNotNull(this, _id);
        }
    }

    public void AddValue(int id, T value)
    {
        _addValueTimes++;
        _values[id] = value;
        JudgeComplete();
    }
    #endregion



    private void JudgeComplete()
    {
        if (_addValueTimes == _values.Length)
        {
             _onComplete.DoIfNotNull(_values);
        }
        else if (_addValueTimes > _values.Length)
        {
            Debug.LogError("AddValue执行次数过多");
        }
    }
}