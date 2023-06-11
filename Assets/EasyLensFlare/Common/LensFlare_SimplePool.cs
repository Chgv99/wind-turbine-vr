using System;
using System.Collections;
using System.Collections.Generic;

namespace GR
{
    public sealed class LensFlare_SimplePool<T>
    {
        int mDefaultSize;
        Func<T> mCreateInstanceFunc;
        Queue<T> mSource;

        public int QueueCount { get { return mSource.Count; } }


        public LensFlare_SimplePool(int defaultSize, Func<T> createInstanceFunc)
        {
            mDefaultSize = defaultSize;
            mSource = new Queue<T>(mDefaultSize);

            mCreateInstanceFunc = createInstanceFunc;

            for (int i = 0; i < defaultSize; i++)
                mSource.Enqueue(mCreateInstanceFunc());
        }

        public T Spawn()
        {
            if (mSource.Count == 0)
                mSource.Enqueue(mCreateInstanceFunc());

            var peekItem = mSource.Dequeue();

            return peekItem;
        }

        public void Despawn(T item)
        {
            if (mSource.Count == mDefaultSize) return;

            mSource.Enqueue(item);
        }

        public Queue<T> GetInternalQueue()
        {
            return mSource;
        }
    }
}
