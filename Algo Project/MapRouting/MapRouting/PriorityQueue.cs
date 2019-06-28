using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapRouting
{
    class PriorityQueue
    {
        public List<Tuple<double,int>> list;
        public int Count { get { return list.Count; } }

        public PriorityQueue()
        {
            list = new List<Tuple<double,int>>();
        }

        public PriorityQueue(int count)
        {
            list = new List<Tuple<double, int>>(count);
        }


        public void Enqueue(Tuple<double,int> x)
        {
            list.Add(x);
            int i = Count - 1;

            while (i > 0)
            {
                int p = (i - 1) / 2;
                if (list[p].Item1 <= x.Item1) break;

                list[i] = list[p];
                i = p;
            }

            if (Count > 0) list[i] = x;
        }

        public Tuple<double, int> Dequeue()
        {
            Tuple<double, int> min = Peek();
            Tuple<double, int> root = list[Count - 1];
            list.RemoveAt(Count - 1);

            int i = 0;
            while (i * 2 + 1 < Count)
            {
                int a = i * 2 + 1;
                int b = i * 2 + 2;
                int c = b < Count && list[b].Item1 < list[a].Item1 ? b : a;

                if (list[c].Item1 >= root.Item1) break;
                list[i] = list[c];
                i = c;
            }

            if (Count > 0) list[i] = root;
            return min;
        }

        public Tuple<double, int> Peek()
        {
            if (Count == 0) throw new InvalidOperationException("Queue is empty.");
            return list[0];
        }

        public void Clear()
        {
            list.Clear();
        }
    }
}
