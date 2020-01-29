//  ***************************************
//                                                   
//                     CsvReader
//                                                   
//  ***************************************
//
//  Copyright (c) 2020 All Rights Reserved

namespace CsvReader.Business
{
    using CsvReader.Extensions;
    using CsvReader.Interfaces;
    using CsvReader.Options;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="CsvWriterBusiness" />
    /// </summary>
    public class CsvWriterBusiness : ICsvWriter
    {
        /// <summary>
        /// Defines the _csvReaderOption
        /// </summary>
        private CsvReaderOption _csvReaderOption;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvWriterBusiness"/> class.
        /// </summary>
        /// <param name="csvReaderOption">The csvReaderOption<see cref="CsvReaderOption"/></param>
        public CsvWriterBusiness(CsvReaderOption csvReaderOption = null)
        {
            if (csvReaderOption != null)
            {
                _csvReaderOption = csvReaderOption;
            }
            else
            {
                _csvReaderOption = new CsvReaderOption();
            }
        }

        /// <summary>
        /// The WriteCSV
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pathFile">The pathFile<see cref="string"/></param>
        /// <param name="datas">The datas<see cref="List{T}"/></param>
        /// <param name="fileName">The fileName<see cref="string"/></param>
        public void WriteCSV<T>(string pathFile, List<T> datas, string fileName = null)
        {
            string filePath = Path.Combine(pathFile, string.Format("{0}.csv", fileName == null ? _csvReaderOption.CsvFileName : fileName));

            var props = typeof(T).GetProperties();

            List<string> lines = new List<string>();

            if (_csvReaderOption.WriteHeader)
            {
                StringBuilder sbHearder = new StringBuilder();
                foreach (PropertyInfo propInfo in props)
                    sbHearder.Append(string.Format("{0}{1}", propInfo.Name.ToCapitalize(_csvReaderOption.CapitalizeHeader), _csvReaderOption.Separator));
                lines.Add(sbHearder.ToString());
            }

            for (int i = 0; i < datas.Count; i++)
            {
                StringBuilder sb = new StringBuilder();

                foreach (PropertyInfo propInfo in props)
                    sb.Append(string.Format("{0}{1}", propInfo.GetValue(datas.ElementAt(i)), _csvReaderOption.Separator));

                lines.Add(sb.ToString());
            }

            StringBuilder sb2 = new StringBuilder();

            for (int index = 0; index < lines.Count; index++)
                sb2.AppendLine(string.Join(_csvReaderOption.EndOfLine, lines.ElementAt(index)));
            try
            {
                File.WriteAllText(filePath, sb2.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
