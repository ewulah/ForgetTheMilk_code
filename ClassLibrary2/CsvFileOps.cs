using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.IO;

namespace ForgetTheMilkTests
{
    // class to store one CSV row
    public class CsvRow : List<string>
    {
        public string LineText { get; set; }
    }

    //class to write data to a CSV file
    public class CsvFileWriter : StreamWriter
    {
        public CsvFileWriter(Stream stream) : base(stream)
        {
        }

        public CsvFileWriter(string filename) : base(filename)
        {
        }

        // Writes a single row to a CSV file
        // <param name="row">The row to be written</param>
        public void WriteRow(CsvRow row)
        {
            StringBuilder builder = new StringBuilder();
            bool firstColumn = true;
            foreach (string value in row)
            {
                //add separator if this isn't the first value
                if (!firstColumn)
                {
                    builder.Append(',');
                }
                //implement special handling for values that contain comma or quote
                //enclose in quotes and double up and double quotes
                if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
                    builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                else
                    builder.Append(value);
                firstColumn = false;
            }
            row.LineText = builder.ToString();
            WriteLine(row.LineText);
        }
    }

    // Class to read data from a CSV file
    class CsvFileReader : StreamReader
    {
        public CsvFileReader(Stream stream) : base(stream)
        {
        }

        public CsvFileReader(string filename) : base(filename)
        {
        }

        // Reads a row of data from a CSV file
        // <param name="row"></param>
        // <returns></returns>
        public bool ReadRow(CsvRow row)
        {
            row.LineText = ReadLine();
            if (String.IsNullOrEmpty(row.LineText))
                return false;

            int pos = 0;
            int rows = 0;

            while (pos < row.LineText.Length)
            {
                string value;

                //Special handling for quoted fields
                if (row.LineText[pos] == '"')
                {
                    //Skip initial quote
                    pos++;

                    //Parse quoted value
                    int start = pos;
                    while (pos < row.LineText.Length)
                    {
                        //Test for quote character
                        if (row.LineText[pos] == '"')
                        {
                            //Found one
                            pos++;

                            //If two quotes together, keep one; otherwise, indicates end of value
                            if (pos >= row.LineText.Length || row.LineText[pos] != '"')
                            {
                                pos--;
                                break;
                            }
                        }
                        pos++;
                    }
                    value = row.LineText.Substring(start, pos - start);
                    value = value.Replace("\"\"", "\"");
                }
                else
                {
                    //Parse unquoted value
                    int start = pos;
                    while (pos < row.LineText.Length && row.LineText[pos] != ',')
                        pos++;
                    value = row.LineText.Substring(start, pos - start);
                }

                //Add field to list
                if (rows < row.Count)
                    row[rows] = value;
                else
                    row.Add(value);
                rows++;

                //Eat up to and including next comma
                while (pos < row.LineText.Length && row.LineText[pos] != ',')
                    pos++;
                if (pos < row.LineText.Length)
                    pos++;
            }

            //Delete any unused items
            while (row.Count > rows)
                row.RemoveAt(rows);

            //Return true if any columns read
            return (row.Count > 0);
        }
    }

}
