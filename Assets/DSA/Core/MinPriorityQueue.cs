using System;
using System.Collections.Generic;

namespace FrontierDispatch.Core
{
    public class MinPriorityQueue<T>
    {
        private readonly List<Entry<T>> _entries = new List<Entry<T>>();

        public int Count => _entries.Count;

        public void Enqueue(T value, int priority)
        {
            _entries.Add(new Entry<T>(value, priority));
            SiftUp(_entries.Count - 1);
        }

        public Entry<T> Peek()
        {
            if (_entries.Count == 0)
            {
                throw new InvalidOperationException("The dispatch queue is empty.");
            }

            return _entries[0];
        }

        public Entry<T> Dequeue()
        {
            if (_entries.Count == 0)
            {
                throw new InvalidOperationException("The dispatch queue is empty.");
            }

            Entry<T> minimum = _entries[0];
            int lastIndex = _entries.Count - 1;
            _entries[0] = _entries[lastIndex];
            _entries.RemoveAt(lastIndex);

            if (_entries.Count > 0)
            {
                SiftDown(0);
            }

            return minimum;
        }

        private void SiftUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = ParentIndex(index);
                if (_entries[index].Priority >= _entries[parentIndex].Priority)
                {
                    return;
                }

                Swap(parentIndex, index);
                index = parentIndex;
            }
        }

        private void SiftDown(int index)
        {
            while (index < _entries.Count)
            {
                int leftChildIndex = LeftChildIndex(index);
                int rightChildIndex = RightChildIndex(index);
                int smallest = index;

                if (leftChildIndex < _entries.Count &&
                    _entries[leftChildIndex].Priority < _entries[smallest].Priority)
                {
                    smallest = leftChildIndex;
                }

                if (rightChildIndex < _entries.Count &&
                    _entries[rightChildIndex].Priority < _entries[smallest].Priority)
                {
                    smallest = rightChildIndex;
                }

                if (smallest == index)
                {
                    return;
                }

                Swap(smallest, index);
                index = smallest;
            }
        }

        private static int ParentIndex(int index)
        {
            return (index - 1) / 2;
        }

        private static int LeftChildIndex(int index)
        {
            return 2 * index + 1;
        }

        private static int RightChildIndex(int index)
        {
            return 2 * index + 2;
        }

        private void Swap(int a, int b)
        {
            Entry<T> temp = _entries[a];
            _entries[a] = _entries[b];
            _entries[b] = temp;
        }

    }
}
