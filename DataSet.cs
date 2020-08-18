using System.Collections.Generic;
namespace KMeans
{
    public class DataSet
    {
        public Dictionary<string, int[]> Data {get;set;}
        public Dictionary<int, List<string>> CentroidLabels {get;set;}
        public DataSet()
        {
            this.Data = new Dictionary<string, int[]>();
            this.CentroidLabels = new Dictionary<int, List<string>>();
        }
    }
}