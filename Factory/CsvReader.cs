//  ***************************************
//                                                   
//                     CsvReader
//                                                   
//  ***************************************
//
//  Copyright (c) 2020 All Rights Reserved

namespace CsvReader.Factory
{
    using CsvReader.Business;
    using CsvReader.Interfaces;
    using CsvReader.Options;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="MyCsvReader" />
    /// </summary>
    public class MyCsvReader
    {
        /// <summary>
        /// The GetCsvReader
        /// </summary>
        /// <param name="pathFile">The pathFile<see cref="string"/></param>
        /// <param name="csvReaderOption">The csvReaderOption<see cref="CsvReaderOption"/></param>
        /// <returns>The <see cref="ICsvReader"/></returns>
        public static ICsvReader GetCsvReader(string pathFile, CsvReaderOption csvReaderOption = null)
        {
            return new CsvReaderBusiness(pathFile, csvReaderOption);
        }
    }

    /// <summary>
    /// Defines the <see cref="MyCsvWriter" />
    /// </summary>
    public class MyCsvWriter
    {
        /// <summary>
        /// The GetCsvWriter
        /// </summary>
        /// <param name="csvReaderOption">The csvReaderOption<see cref="CsvReaderOption"/></param>
        /// <returns>The <see cref="ICsvWriter"/></returns>
        public static ICsvWriter GetCsvWriter(CsvReaderOption csvReaderOption = null)
        {
            return new CsvWriterBusiness(csvReaderOption);
        }
    }
}
