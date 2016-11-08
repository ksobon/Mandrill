using System;
using System.Collections.Generic;
using D3jsLib;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Mandrill_Grasshopper.Components.PDF;
using System.Linq;

namespace Mandrill_Grasshopper.Utilities
{
    public static class Utilities
    {
        public static List<DataPoint2> Data2FromTree(List<string> headers, GH_Structure<GH_String> values)
        {
            List<DataPoint2> dataPoints = new List<DataPoint2>();
            for (int i = 0; i < values.Branches.Count; i++)
            {
                DataPoint2 dataPoint = new DataPoint2();
                dataPoint.Name = values.Branches[i][0].Value;
                Dictionary<string, double> dict = new Dictionary<string, double>();
                for (int j = 1; j < values.Branches[i].Count; j++)
                {
                    try
                    {
                        dict.Add(headers[j], Convert.ToDouble(values.Branches[i][j].Value));
                    }
                    catch
                    {
                        dict.Add(headers[j], 0);
                    }
                }
                dataPoint.Values = dict;
                dataPoints.Add(dataPoint);
            }
            return dataPoints;
        }

        public static List<GH_ValueListItem> EnumGetItems<TEnum>()
                where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("TEnum must be an Enumeration type");

            var res = from e in Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                      select new GH_ValueListItem() { Name = e.ToString(), Expression = e.ToString() };

            return res.ToList();
        }

    }
}
