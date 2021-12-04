using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csv_Creator
{
    class CsvFile
    {
        public string Name { get; private set; }
        public string ParentFolderPath { get; private set; }
        public string[] OneLine { get; private set; }
        public string[] IsDynamics { get; private set; }
        public int RowCount { get; private set; }
        public int ColCount { get; private set; }

        public CsvFile(string name, string folder, string[] oneLine, string[] isDynamic, int rowCount, int colCount)
        {
            Name = name;
            ParentFolderPath = folder;
            OneLine = oneLine;
            IsDynamics = isDynamic;
            RowCount = rowCount;
            ColCount = colCount;
        }

        public string GetCsvFilePath()
        {
            return Path.Combine(ParentFolderPath, Name);
        }

        public bool ExistCsvFile()
        {
            return File.Exists(GetCsvFilePath());
        }
    }
}
