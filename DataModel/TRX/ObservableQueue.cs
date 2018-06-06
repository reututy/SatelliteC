﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.TRX
{
    public class ObservableQueue<Frame>
    {
        public Queue<Frame> queue = new Queue<Frame>();
        public ObservableCollection<Frame> queueCollection = new ObservableCollection<Frame>();

        public void enqueue(Frame frame)
        {
            queue.Enqueue(frame);
            queueCollection.Add(frame);
        }

        public Frame dequeue()
        {
            Frame frame = queue.Dequeue();
            queueCollection.Remove(frame);
            return frame;
        }

        public Frame peek()
        {
            return queue.Peek();
        }

        public int count()
        {
            return queue.Count;
        }

        public void clear()
        {
            queue.Clear();
            queueCollection.Clear();
        }
    }
}
