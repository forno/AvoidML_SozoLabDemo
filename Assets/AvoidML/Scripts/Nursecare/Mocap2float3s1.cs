﻿using Forno.Csv;
using System;
using Unity.Mathematics;

namespace AvoidML.Nursecare
{
    public class Mocap2float3s : IDisposable
    {
        private TextField NursecareTextTield;

        public Mocap2float3s(string path)
        {
            NursecareTextTield = new TextField(path);
            // Discard header
            if (Constants.hasHeader)
                NursecareTextTield.MoveNext();
        }

        public void Dispose()
        {
            NursecareTextTield.Dispose();
        }

        public float3?[] GetData()
        {
            if (!NursecareTextTield.MoveNext())
                return null;
            float?[] fields = Array.ConvertAll<string, float?>(NursecareTextTield.Current, x =>
            {
                float v;
                if (float.TryParse(x, out v)) return v * Constants.convert2meter;
                else return null;
            });

            float3?[] values = new float3?[Forno.HelenHayes.Constants.positionCount];
            for(uint i = 0; i < values.Length; ++i) {
                // Convert coordinate system
                var v0 = fields[i * 3];
                var v1 = fields[i * 3 + 2];
                var v2 = fields[i * 3 + 1];

                if (v0.HasValue && v1.HasValue && v2.HasValue)
                    values[i] = new float3(v0.Value, v1.Value, v2.Value);
                else
                    values[i] = null;
            }
            return values;
        }
    }
}
