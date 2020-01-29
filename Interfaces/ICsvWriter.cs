//  ***************************************
//                                                   
//                     CsvReader
//                                                   
//  ***************************************
//
//  Copyright (c) 2020 All Rights Reserved

namespace CsvReader.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="ICsvWriter" />
    /// </summary>
    public interface ICsvWriter
    {
        /// <summary>
        /// Retourne un fichier CSV crée à partir d'une liste d'objet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pathFile"></param>
        /// <param name="datas"></param>
        /// <param name="fileName"></param>
        void WriteCSV<T>(string pathFile, List<T> datas, string fileName = null);
    }
}
