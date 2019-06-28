using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapRouting
{
    class MapRouting
    {
        public long w = 0;
        public int vertices;
        public Dictionary<Tuple<Double, Double>, int> map;
        public Dictionary<int, Tuple<Double, Double>> antimap;
        public List<Tuple<Double, int>>[] AdjList;
        public List<Tuple<Double, int>>[] DistList;
        public List<Double>[] Queries;
        public Double Radius;
        public List<Tuple<int, double>> sources, destinations;
        public Tuple<double, double> source, destination;
        public Double INF = Double.MaxValue;
        public Double[] time;
        public int[] parent;
        public List<int> final_path;
        public Double[] distance;
        //public SortedSet<Tuple<Double, int>> Q;
        public PriorityQueue p;
        public Tuple<Double, int> temp_pair;
        public List<string> lines;
        public int num;
        public double Vehicle_Distance;
        public double Walking_Distance;
        public void Dijkstra(int source)
        {   //initialization of the data structures needed
            int dest = AdjList.Count() - 1;
            time = new Double[AdjList.Count()];
            distance = new Double[AdjList.Count()];
            parent = new int[AdjList.Count()];
            //Q = new SortedSet<Tuple<Double, int>>();
            p = new PriorityQueue();
            for (int i = 0; i < AdjList.Count(); i++)//loop on every vertex 
            {   //theta(V)
                if (i == source)
                {
                    //time and distance to source vertex = zero
                    time[i] = 0;
                    distance[i] = 0;
                    parent[i] = i;
                    //construct a tuple that consists of the node number and time
                    temp_pair = new Tuple<Double, int>(0, i);
                    //Q.Add(temp_pair);//theta(logE)
                    p.Enqueue(temp_pair);
                }
                else
                {
                    //set time to the rest of the vertices to infinity
                    time[i] = INF;
                    //construct a tuple that consists of the node number and time
                    temp_pair = new Tuple<Double, int>(time[i], i);
                }
            }
            //first for loop o(v)
            //while (Q.Count() != 0)//while the queue is not empty
            while(p.Count>0)
            {//o(v)
                //select the node with the minimum time
                Tuple<Double, int> ans = p.Dequeue();
                //remove the node with the minimum time
                //if we reached the desired destination, end the dijkstra 
                if (ans.Item2 == dest)
                    break;
                int i = 0;
                foreach (Tuple<Double, int> x in AdjList[ans.Item2])//loop on each neighbour of the selected minimum node
                {   //o(neighbours x logv)
                    //if we found a shorter path to the neighbour, update it with the new path
                    if (time[x.Item2] > time[ans.Item2] + x.Item1)
                    {
                        //set the time with the updated value
                        time[x.Item2] = time[ans.Item2] + x.Item1;
                        //set the distance with the updated value
                        distance[x.Item2] = distance[ans.Item2] + DistList[ans.Item2].ElementAt(i).Item1;
                        //save the parent of the node 
                        parent[x.Item2] = ans.Item2;
                        //add the new updated node to the queue
                        p.Enqueue(new Tuple<Double, int>(time[x.Item2], x.Item2));
                    }
                    i++;
                }
                //v x neighbours=e
                //e'logv
            }
        }
        public void FillMap()
        {//theta(E)
            //map = new Dictionary<Tuple<double, double>, int>();
            antimap = new Dictionary<int, Tuple<double, double>>();
            //store all the lines in the  file in an array of strings called lines
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Salma Emad\Desktop\be el ui\MapRouting\MapRouting\map2.txt");
            //store the number of vertices from the file
            vertices = int.Parse(lines[0]);
            for (int i = 1; i <= vertices; i++)//theta(v)
            {
                //split each line into string
                string[] tokens = lines[i].Split(' ');
                //set the coordinates of a node as a key in the map and it's number as a value
                //map.Add(new Tuple<Double, Double>(Double.Parse(tokens[1]), Double.Parse(tokens[2])), int.Parse(tokens[0]));
                //set the number of a node as a key and it's coordinates as it's value
                antimap.Add(int.Parse(tokens[0]), new Tuple<Double, Double>(Double.Parse(tokens[1]), Double.Parse(tokens[2])));
            }
            //store the number of edges
            int edges = int.Parse(lines[vertices + 1]);
            AdjList = new List<Tuple<double, int>>[vertices+2];
            DistList = new List<Tuple<double, int>>[vertices+2];
            for (int i = 0; i < AdjList.Length; i++)//v
            {
                //initialize each element with a new list
                AdjList[i] = new List<Tuple<double, int>>();
            }
            for (int i = 0; i < DistList.Length; i++)//v
            {
                //initialize each element with a new list
                DistList[i] = new List<Tuple<double, int>>();
            }
            for (int i = 1; i <= edges; i++)//theta(v)
            {
                string[] tokens = lines[i + vertices + 1].Split(' ');
                //calculate the time from the velocity and distance (time=distance/velocity)
                Double time = (Double.Parse(tokens[2])) / ((Double.Parse(tokens[3])));
                //convert the time into minutes
                time = time * 60;
                //connect the vertices with the given edge
                AdjList[int.Parse(tokens[0])].Add(new Tuple<Double, int>(time, int.Parse(tokens[1])));
                //it's an undirected graph so we add it on both sides
                AdjList[int.Parse(tokens[1])].Add(new Tuple<Double, int>(time, int.Parse(tokens[0])));
                //just like adjlist but instead of setting the edge as time we are setting it as distance
                DistList[int.Parse(tokens[0])].Add(new Tuple<Double, int>(Double.Parse(tokens[2]), int.Parse(tokens[1])));
                //it's an undirected graph so we add it on both sides 
                DistList[int.Parse(tokens[1])].Add(new Tuple<Double, int>(Double.Parse(tokens[2]), int.Parse(tokens[0])));
            }
        }
        public void ReadQuery()
        { //this function is used for reading all the queries from queries.txt file
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Salma Emad\Desktop\be el ui\MapRouting\MapRouting\queries2.txt");
            num = int.Parse(lines[0]); //parsing the input line in which the number of queries is saved in variable num
            Queries = new List<Double>[num]; //creating queries list and initialize it depending on the number of queries (num)
            for (int i = 0; i < Queries.Length; i++)
            {
                Queries[i] = new List<Double>();
            }
            for (int i = 1; i <= num; i++)
            {
                string[] tokens = lines[i].Split(' ');//parsing the input query line to add it in the list
                for (int j = 0; j < 5; j++)
                {
                    Queries[i - 1].Add(Double.Parse(tokens[j]));
                }
                
            }
        }
        public void SourceDestinationFinder()
        {//this function is used to find all sources and destinations
            
            FillMap();
            ReadQuery();
            lines = new List<string>((vertices+2)*num); //used to write in the output file
            //for every query we will consider that the source location is the source node
            //and the destination location is the destination node

            for (int i = 0; i < Queries.Length; i++)
            {
                
                //
                //var watch = System.Diagnostics.Stopwatch.StartNew();
                //
                sources = new List<Tuple<int, double>>();//list of all possible sources
                destinations = new List<Tuple<int, double>>();//list of all possible destinations
                source = new Tuple<double, double>(Queries[i][0], Queries[i][1]); //the source location
                destination = new Tuple<double, double>(Queries[i][2], Queries[i][3]);//the destination location
                Radius = Queries[i][4];
                Radius = Radius / 1000; //conevert the given radius into km
                //add the source location into the map and the anti-map
                antimap.Add(vertices,source);
                //map.Add(source,vertices);
                //add the destination location into the map and the anti-map
                antimap.Add(vertices + 1, destination);
                //map.Add(destination, vertices + 1);
                for (int j = 0; j < vertices; j++)
                {
                    //using euclidean theorem for checking all the possible sources nodes within the given radius
                    double x = Math.Pow(Math.Abs(source.Item1 - antimap[j].Item1), 2);
                    double y = Math.Pow(Math.Abs(source.Item2 - antimap[j].Item2), 2);
                    double temp = Math.Sqrt(x + y);

                    if (temp <= Radius)
                    {
                        //connect the source location with all possible source nodes
                        //and considering the weight of the distlist as distance
                        DistList[vertices].Add(new Tuple<double,int>(temp,j));
                        temp = (temp * 60) / 5;
                        Tuple<int, double> t = new Tuple<int, double>(j, temp);
                       
                        AdjList[vertices].Add(new Tuple<double,int>(temp,j));
                        sources.Add(t);
                    }
                    //using euclidean theorem for cheking all the possible destinations nodes within the given radius
                    x = Math.Pow(Math.Abs(destination.Item1 - antimap[j].Item1), 2);
                    y = Math.Pow(Math.Abs(destination.Item2 - antimap[j].Item2), 2);
                    temp = Math.Sqrt(x + y);

                    if (temp <= Radius)
                    {
                        //connect the source location with all possible destination nodes
                        //and considering the weight of the distlist as distance
                        DistList[j].Add(new Tuple<double,int>(temp,vertices+1));
                        temp = (temp * 60) / 5;
                        
                        AdjList[j].Add(new Tuple<double, int>(temp, vertices + 1));
                        destinations.Add(new Tuple<int, double>(j, temp));
                    }
                }
                //calling dijkstra(source location)
                var watch = System.Diagnostics.Stopwatch.StartNew();
                Dijkstra(vertices);
                watch.Stop();
                w += watch.ElapsedMilliseconds;

                final_path = new List<int>();
                int current_vertex=AdjList.Count()-1;
                while (current_vertex!= vertices)
                {
                    if (parent[current_vertex] != vertices)
                    {
                        final_path.Add(parent[current_vertex]);
                    }
                    current_vertex = parent[current_vertex];
                }
                //watch.Stop();
                //w += watch.ElapsedMilliseconds;
                string text="";
                for (int fp = final_path.Count() - 1; fp >= 0; fp--)
                {
                    text += (final_path[fp].ToString() + ' ');
                    
                }
                lines.Add(text);
                text =Math.Round((Double) time[time.Count() - 1],2).ToString() + " mins";//creating string text to write the total time in the file
                                                                                     //from time array where the last index is the total calculated time
                lines.Add(text);
                text =Math.Round((Double) distance[distance.Count() - 1],2).ToString() + " Km"; //write in the output file the total distance from distance array where the last index is the total distance
                lines.Add(text);

                //calculating the vehicle distance by subtracting the distance from source node to source location from the total distance and write it in the file
                Vehicle_Distance = Math.Abs(distance[final_path[0]] - distance[final_path[final_path.Count() - 1]]);
                //calculating the walking distance by subtracting the vehicle distance from the total distance and write it in the file
                Walking_Distance = Math.Abs(distance[distance.Count() - 1] - Vehicle_Distance);
                text =Math.Round((Double)Walking_Distance,2).ToString()+" km";
                lines.Add(text);

                text = Math.Round((Double)Vehicle_Distance, 2).ToString() + " Km";
                lines.Add(text);
                
                antimap.Remove(vertices);
                antimap.Remove(vertices + 1);
                //map.Remove(source);
                //map.Remove(destination);
                while(AdjList[vertices].Count!=0)
                {
                    AdjList[vertices].RemoveAt(AdjList[vertices].Count() - 1);
                    DistList[vertices].RemoveAt(DistList[vertices].Count() - 1);
                }
                //save el 7aga f file f nfs el 7ta
                //remove el 2 nodes mn el map w antimap w adjlist w distlist
                for(int m=0;m<destinations.Count();m++)
                {
                    int tmp = destinations[m].Item1;
                    DistList[tmp].RemoveAt(DistList[tmp].Count() - 1);
                    AdjList[tmp].RemoveAt(AdjList[tmp].Count() - 1);

                }
                lines.Add(" ");
                //System.IO.File.WriteAllLines(@"C:\Users\Salma Emad\Desktop\Team 20\MapRouting\MapRouting\SFOutput.txt", lines);
            }
           lines.Add(w.ToString()+" ms");
           lines.Add(" ");
           //System.IO.File.WriteAllLines(@"C:\Users\Gamela\Downloads\Compressed\Team 20\Team 20\MapRouting\MapRouting\SFOutput.txt", lines);
        }
    }
}
