using System;
using Unity.Mathematics;

namespace AvoidML.Nursecare
{
    public class Mocap2float3s : IDisposable
    {
        private CSV.TextField NursecareTextTield;

        public Mocap2float3s(string path)
        {
            NursecareTextTield = new CSV.TextField(path);
            NursecareTextTield.MoveNext();
        }

        public void Dispose()
        {
            NursecareTextTield.Dispose();
        }

        public float3[] GetData()
        {
            if (!NursecareTextTield.MoveNext())
                return null;
            float?[] fields = Array.ConvertAll<string, float?>(NursecareTextTield.Current, x =>
            {
                float v;
                if (float.TryParse(x, out v)) return v / 1000; // convert to meter from millimeter
                else return null;
            });

            float3[] values = new float3[29];
            for(uint i = 0; i < values.Length; ++i) {
                values[i] = new float3(fields[i * 3] ?? 0, fields[i * 3 + 2] ?? 0, fields[i * 3 + 1] ?? 0); // Convert coordinate system
            }
            return values;
        }
    }
}
