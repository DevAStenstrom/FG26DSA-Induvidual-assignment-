using System;
using System.Collections.Generic;

namespace FrontierDispatch.Core
{
    public class MinPriorityQueue
    {
        private readonly List<Location> LocationsPriorityList = new List<Location>();
        public int Count => LocationsPriorityList.Count;
        
        public void Enqueue(Location location)
        {
            LocationsPriorityList.Add(location);
            SiftUp(LocationsPriorityList.Count - 1);
        }
        
        public Location PeekNext()
        {
            if (LocationsPriorityList.Count == 0)
            {
                throw new InvalidOperationException("The dispatch queue is empty.");
            }

            return LocationsPriorityList[0];
        }
        
        public Location DequeueNext()
        {
            if (LocationsPriorityList.Count == 0)
            {
                throw new InvalidOperationException("The dispatch queue is empty.");
            }

            Location minimum = LocationsPriorityList[0];
            int lastIndex = LocationsPriorityList.Count - 1;

            LocationsPriorityList[0] = LocationsPriorityList[lastIndex];
            LocationsPriorityList.RemoveAt(lastIndex);

            if (LocationsPriorityList.Count > 0)
            {
                SiftDown(0);
            }

            return minimum;
        }
        
        private void SiftUp(int index)
        {
            if (index == 0)
            {
                return;
            }
            
            int parentIndex = ParentIndex(index);
            if (LocationsPriorityList[index].Priority < LocationsPriorityList[parentIndex].Priority)
            {
                Swap(parentIndex,index);
                SiftUp(parentIndex);
            }

        }
        
        private void SiftDown(int index)
        {
            if (index >= LocationsPriorityList.Count)
            {
                return;
            }

            int leftChildIndex = LeftChildIndex(index);
            int rightChildIndex = RightChildIndex(index);
            
            int smallest = index;
            if (leftChildIndex < LocationsPriorityList.Count && LocationsPriorityList[leftChildIndex].Priority < LocationsPriorityList[smallest].Priority )
            {
                smallest = leftChildIndex;
            }
            if (rightChildIndex < LocationsPriorityList.Count && LocationsPriorityList[rightChildIndex].Priority < LocationsPriorityList[smallest].Priority )
            {
                smallest = rightChildIndex;
            }

            if (smallest != index)
            {
                Swap(smallest,index);
                SiftDown(smallest);
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
            Location temp = LocationsPriorityList[a];
            LocationsPriorityList[a] = LocationsPriorityList[b];
            LocationsPriorityList[b] = temp;
        }
    }
}