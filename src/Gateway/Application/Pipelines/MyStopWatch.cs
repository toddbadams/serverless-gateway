using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gateway.Application.Pipelines
{
    public class MyStopWatch
    {
        private Stopwatch _watch;

        public MyStopWatch()
        {
            _watch = new Stopwatch();
        }

        public void Start() => _watch.Start();

        public void Stop() => _watch.Stop();

        public void Log(string msg) => Console.WriteLine(string.Format(msg, _watch.ElapsedMilliseconds));

        public static async Task LogMs(string msg, Func<Task<HttpResponseMessage>> action)
        {
            var w = new MyStopWatch();
            w.Start();
            await action();
            w.Stop();
            w.Log(msg);
        }
    }
}