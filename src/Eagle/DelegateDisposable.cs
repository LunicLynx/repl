using System;

namespace Eagle
{
    public class DelegateDisposable : IDisposable
    {
        private readonly Action _dispose;

        public DelegateDisposable(Action dispose)
        {
            _dispose = dispose;
        }
        public void Dispose()
        {
            _dispose();
        }
    }
}