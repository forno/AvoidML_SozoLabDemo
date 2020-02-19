using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Forno.Csv
{
    public class TextField : IEnumerator<string[]>
    {
        private StreamReader stream;
        private string path;
        private char separator;
        private Encoding encoding;

        public TextField(string filePath, char separator = ',', Encoding encoding = null)
        {
            path = filePath;
            this.separator = separator;
            this.encoding = encoding;
            stream = new StreamReader(filePath, encoding ?? Encoding.UTF8);
        }

        public string[] Current { get; private set; } = null;

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            var isend = stream.EndOfStream;
            if (isend) Current = null;
            else Current = stream.ReadLine().Split(separator);
            return !isend;
        }

        public void Reset()
        {
            stream.Dispose();
            stream = new StreamReader(path, encoding ?? Encoding.UTF8);
        }

        public void Dispose()
        {
            stream.Dispose();
        }
    }
}
