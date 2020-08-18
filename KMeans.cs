using System.Collections.Generic;
using System;

namespace KMeans
{
    public class KMeans
    {
        private int[] maxValueCentroid;
        private int[] minValueCentroid;
        public int MaxIterations {get;set;}
        public int Dimensions {get;set;}
        public int KClusters {get;set;}
        public int Iterations {get;set;}
        public int[][] Centroids {get; private set;}
        private int[][] oldCentroids;
        private bool balanced;
        private int maxPerCluster = 0;
        public KMeans(int kClusters, int dimensions, bool balanced = true, int maxIterations = 1000)
        {
            this.balanced = balanced;
            this.KClusters = kClusters;
            this.Dimensions = dimensions;
            this.Centroids = new int[kClusters][];
            for (int i =0; i<kClusters;i++)
            {
                this.Centroids[i] = new int[dimensions];
            }
            this.MaxIterations = maxIterations;
        }
        public void InitializeCentroids(DataSet dataSet)
        {
            maxValueCentroid = new int[this.Dimensions];
            minValueCentroid = new int[this.Dimensions];
            
            for (int i = 0; i<Dimensions;i++)
            {
                maxValueCentroid[i] = Int32.MinValue;
                minValueCentroid[i] = Int32.MaxValue;
            }
            
            for (int d = 0; d<this.Dimensions; d++)
            {
                foreach (int[] dataVector in dataSet.Data.Values)
                {
                    if (dataVector[d]>maxValueCentroid[d])
                    {
                        maxValueCentroid[d] = dataVector[d];
                    }
                    if (dataVector[d]<minValueCentroid[d])
                    {
                        minValueCentroid[d] = dataVector[d];
                    }
                }
            }
            Random r = new Random();
            for (int i = 0; i < this.KClusters; i++)
            {
                int[] centroid = new int[this.Dimensions];
                for (int j = 0; j < this.Dimensions; j++ )
                {
                    centroid[j] = r.Next(minValueCentroid[j],maxValueCentroid[j]);
                }
                this.Centroids[i] = centroid;
            }
        }
        private void CopyToOldCentroid()
        {
            for (int i =0; i<this.KClusters;i++)
            {
                for (int j =0; j< this.Dimensions; j++)
                {
                    this.oldCentroids[i][j] = this.Centroids[i][j];
                }
            }
        }
        public void CaluclateKMeans(DataSet dataSet)
        {
            if (balanced)
            {
                maxPerCluster = dataSet.Data.Count/this.KClusters;
            }
            // Initialize centroids randomly
            InitializeCentroids(dataSet);
            
            //Initialize book keeping vars.
            this.Iterations = 0;
            this.oldCentroids = new int[this.KClusters][];
            for (int i = 0; i<this.KClusters;i++)
            {
                oldCentroids[i] = new int[Dimensions];
            }
            while(ShouldNotStop())
            {
                // Save old centroids for convergence test. Book keeping.
                CopyToOldCentroid();
                Iterations++;
                //Assign labels to each datapoint based on centroids
                GetLabels(dataSet);
        
                // Assign centroids based on datapoint labels
                GetCentroids(dataSet);
            }
        }
        private bool ShouldNotStop()
        {
            bool centroidEqual = true;
            if (oldCentroids != null)
            {
                for (int i = 0; i <this.KClusters; i++)
                {
                    for (int j =0; j <this.Dimensions; j++ )
                    {
                        if (oldCentroids[i][j] != this.Centroids[i][j])
                        {
                            centroidEqual = false;
                        }
                    }
                }
            }
            else
            {
                centroidEqual = false;
            }
            return (this.Iterations < this.MaxIterations) && (!centroidEqual);
        }
        
        // Function: Get Centroids
        //# -------------
        // Returns k random centroids, each of dimension n.
        private void GetCentroids(DataSet dataSet)
        {
            // Each centroid is the geometric mean of the points that
            //# have that centroid's label. Important: If a centroid is empty (no points have
            //# that centroid's label) you should randomly re-initialize it.
            for (int centroidIndex = 0; centroidIndex< this.KClusters; centroidIndex++)
            {
                if (dataSet.CentroidLabels[centroidIndex].Count == 0)
                {
                    Random r = new Random();
                    for (int j = 0; j < this.Dimensions; j++ )
                    {
                        this.Centroids[centroidIndex][j] = r.Next(minValueCentroid[j],maxValueCentroid[j]);
                    }
                }
                else
                {
                    for (int i = 0; i < this.Dimensions; i++)
                    {
                        double mean = 0;
                        foreach (string label in dataSet.CentroidLabels[centroidIndex])
                        {
                            mean += dataSet.Data[label][i];
                        }
                        mean = mean/dataSet.CentroidLabels[centroidIndex].Count;
                        this.Centroids[centroidIndex][i] = Convert.ToInt32(mean);
                    }
                }
            }
        }
        private double GetDistance(int[] x, int[] y)
        {
            double distance = 0;
            for (int i = 0; i < x.Length; i++)
            {
                distance += Math.Pow((x[i] - y[i]), 2.0);
            }
            return Math.Sqrt(distance);
        }
        private void GetLabels(DataSet dataSet)
        {
            int closestCentroid = -1;
            dataSet.CentroidLabels = new Dictionary<int, List<string>>();
            int centroidIndex = 0;
            foreach (int[] c in this.Centroids)
            {
                dataSet.CentroidLabels[centroidIndex] = new List<string>();
                centroidIndex++;
            }
            
            foreach(string id in dataSet.Data.Keys)
            {
                double minDistance = Double.MaxValue;
                centroidIndex = 0;
                foreach (int[] c in this.Centroids)
                {
                    if (dataSet.CentroidLabels[centroidIndex].Count < this.maxPerCluster)
                    {
                        double tmpDistance = GetDistance(dataSet.Data[id], c);
                        if (tmpDistance < minDistance)
                        {
                            closestCentroid = centroidIndex;
                            minDistance = tmpDistance;
                        }
                    }
                    centroidIndex++;
                }
                dataSet.CentroidLabels[closestCentroid].Add(id);
            }
        }
    }
}