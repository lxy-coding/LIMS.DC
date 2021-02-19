using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.DC.Service
{
    public class TaskHelper
    {
        private static ConcurrentDictionary<Action, Task> _DicTask;

        private static CancellationTokenSource _CTS;

        private static Task _MainTask=null;

        public static void Init()
        {
            _DicTask = new ConcurrentDictionary<Action, Task>();
            Run();
        }

        private static void Run()
        {
            _CTS = new CancellationTokenSource();
            _MainTask=Task.Run(()=> {
                while (!_CTS.IsCancellationRequested)
                {
                    Dictionary<Action, Task> dic = new Dictionary<Action, Task>();
                    foreach (KeyValuePair<Action, Task> item in _DicTask)
                    {
                        if (item.Value == null || item.Value.IsCompleted)
                        {
                            dic[item.Key] = Task.Run(item.Key);
                        }
                    }
                    foreach (var item in dic)
                    {
                        _DicTask[item.Key] = item.Value;
                    }
                    Thread.Sleep(1000);
                }
            },_CTS.Token);
        }

        public static bool RegisterTask(Action action)
        {
            return _DicTask.TryAdd(action, null);
        }

        public static bool UnRegisterTask(Action action)
        {
            Task task;
            return _DicTask.TryRemove(action,out task);
        }


        public static void Close()
        {
            if(_CTS!=null)
            {
                _CTS.Cancel();
                if (_MainTask != null)
                {
                    _MainTask.Wait(1000);
                }
                if(_DicTask!=null)
                {
                    _DicTask.Clear();
                }
            }
        }
    }
}
