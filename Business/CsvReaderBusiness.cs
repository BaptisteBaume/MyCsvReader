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
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.Json;
    using System.Xml.Serialization;

    /// <summary>
    /// Defines the <see cref="CsvReaderBusiness" />
    /// </summary>
    public class CsvReaderBusiness : ICsvReader
    {
        /// <summary>
        /// Defines the _datas
        /// </summary>
        private string[] _datas;

        /// <summary>
        /// Defines the _csvReaderOption
        /// </summary>
        private CsvReaderOption _csvReaderOption;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvReaderBusiness"/> class.
        /// </summary>
        public CsvReaderBusiness()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvReaderBusiness"/> class.
        /// </summary>
        /// <param name="pathFile">The pathFile<see cref="string"/></param>
        /// <param name="csvReaderOption">The csvReaderOption<see cref="CsvReaderOption"/></param>
        public CsvReaderBusiness(string pathFile, CsvReaderOption csvReaderOption = null)
        {

            if (csvReaderOption != null)
            {
                _csvReaderOption = csvReaderOption;
            }
            else
            {
                _csvReaderOption = new CsvReaderOption();
            }
            _datas = File.ReadAllLines(pathFile);
        }

        /// <summary>
        /// The CountLines
        /// </summary>
        /// <returns>The <see cref="int"/></returns>
        public int CountLines()
        {
            return _datas.Length;
        }

        /// <summary>
        /// The CountColumns
        /// </summary>
        /// <returns>The <see cref="int"/></returns>
        public int CountColumns()
        {

            int maxSize = 0;

            for (int x = 0; x < _datas.Length; x++)
            {
                var size = _datas[x].Split(_csvReaderOption.Separator).Length;
                if (size > maxSize) maxSize = size;
            }
            return maxSize;
        }

        /// <summary>
        /// The CountColumns
        /// </summary>
        /// <param name="lineNumber">The lineNumber<see cref="int"/></param>
        /// <param name="trim">The trim<see cref="bool"/></param>
        /// <returns>The <see cref="int"/></returns>
        public int CountColumns(int lineNumber, bool trim = false)
        {
            if (!trim)
            {
                return _datas[lineNumber.Humanize(_csvReaderOption.Humanize)].Split(_csvReaderOption.Separator).Length;
            }
            return TrimEndCellWithNoValue(_datas[lineNumber].Split(_csvReaderOption.Separator)).Length;
        }

        /// <summary>
        /// The GetLine
        /// </summary>
        /// <param name="line">The line<see cref="int"/></param>
        /// <param name="trim">The trim<see cref="bool"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string GetLine(int line, bool trim = false)
        {
            if (!trim)
                return _datas[line.Humanize(_csvReaderOption.Humanize)];
            return string.Join(_csvReaderOption.Separator, TrimEndCellWithNoValue(_datas[line.Humanize(_csvReaderOption.Humanize)].Split(_csvReaderOption.Separator)));
        }

        /// <summary>
        /// The GetLineInArray
        /// </summary>
        /// <param name="line">The line<see cref="int"/></param>
        /// <param name="trim">The trim<see cref="bool"/></param>
        /// <returns>The <see cref="string[]"/></returns>
        public string[] GetLineInArray(int line, bool trim = false)
        {
            if (!trim)
                return _datas[line.Humanize(_csvReaderOption.Humanize)].Split(_csvReaderOption.Separator);
            return TrimEndCellWithNoValue(_datas[line.Humanize(_csvReaderOption.Humanize)].Split(_csvReaderOption.Separator));
        }

        /// <summary>
        /// The GetValue
        /// </summary>
        /// <param name="line">The line<see cref="int"/></param>
        /// <param name="column">The column<see cref="int"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string GetValue(int line, int column)
        {
            return GetLineInArray(line)[column.Humanize(_csvReaderOption.Humanize)];
        }

        /// <summary>
        /// The FindValues
        /// </summary>
        /// <param name="value">The value<see cref="string"/></param>
        /// <returns>The <see cref="List{IDictionary{int, int}}"/></returns>
        public List<IDictionary<int, int>> FindValues(string value)
        {
            return SearchValue(value);
        }

        /// <summary>
        /// The GetNbOccurence
        /// </summary>
        /// <param name="value">The value<see cref="string"/></param>
        /// <returns>The <see cref="int"/></returns>
        public int GetNbOccurence(string value)
        {
            return SearchValue(value).Count();
        }

        /// <summary>
        /// The MapLine
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lineNumber">The lineNumber<see cref="int"/></param>
        /// <param name="colsToMap">The colsToMap<see cref="List{int}"/></param>
        /// <returns>The <see cref="T"/></returns>
        public T MapLine<T>(int lineNumber, List<int> colsToMap = null) where T : class, new()
        {
            T dest = new T();
            // Les propriétés de l'objet à mapper
            var props = typeof(T).GetProperties();

            int cpt = 0;
            string[] line = GetLineInArray(lineNumber);

            /*
            if (_csvReaderOption.SkipFirstCol)
            {
                List<int> colsToMapWithSkipFirst = new List<int>();
                int nbCols = CountColumns();
                for (int i = 0; i < nbCols; i++)
                {
                    if (i > 0)
                        colsToMapWithSkipFirst.Add(i);
                }
                line = ReduceLine(line, colsToMapWithSkipFirst);
            }
            */
            // Si colsToMap ne garder que les colonnes à mapper
            if (colsToMap != null)
            {
                line = ReduceLine(line, colsToMap);
            }
            int nbColToMap = line.Length;

            foreach (PropertyInfo propInfo in props)
            {
                try
                {
                    string valToTest = line[cpt].SuperTrim();
                    if (cpt == nbColToMap)
                    {
                        return dest;
                    }
                    else
                    {
                        CheckPropertyType(propInfo, ref dest, valToTest);
                    }

                }
                catch (Exception e)
                {
                    // Variables utiles pour le débug.
                    var point = cpt;
                    var val = line[cpt];
                    var li = lineNumber;
                    throw;
                }

                cpt++;
            }

            return dest;
        }

        /// <summary>
        /// The MapLineTRestrict
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lineNumber">The lineNumber<see cref="int"/></param>
        /// <param name="colsToMap">The colsToMap<see cref="Dictionary{int, string}"/></param>
        /// <returns>The <see cref="T"/></returns>
        public T MapLineTRestrict<T>(int lineNumber, Dictionary<int, string> colsToMap) where T : class, new()
        {
            T dest = new T();
            // Les propriétés de l'objet à mapper
            var props = typeof(T).GetProperties();

            int cpt = 0;
            string[] line = GetLineInArray(lineNumber);

            if (_csvReaderOption.SkipFirstCol)
            {
                List<int> colsToMapWithSkipFirst = new List<int>();
                int nbCols = CountColumns();
                for (int i = 0; i < nbCols; i++)
                {
                    if (i > 0)
                        colsToMapWithSkipFirst.Add(i);
                }
                line = ReduceLine(line, colsToMapWithSkipFirst);
            }
            // Si colsToMap ne garder que les colonnes à mapper

            if (colsToMap != null)
            {
                // Toutes les clés dnas une liste
                List<int> colsToReduce = new List<int>();

                for (int i = 0; i < colsToMap.Count; i++)
                {
                    colsToReduce.Add(colsToMap.ElementAt(i).Key);
                }
                line = ReduceLine(line, colsToReduce);
            }
            int nbColToMap = line.Length;


            for (int i = 0; i < nbColToMap; i++)
            {
                string valToTest = line[i].SuperTrim();

                if (colsToMap.ElementAt(i).Value.ToUpper().Contains("&"))
                {
                    var properties = colsToMap.ElementAt(i).Value.ToUpper().Split("&");
                    foreach (var prop in properties)
                    {
                        CheckOverObjectProperties(ref props, ref dest, prop, valToTest);
                    }
                }
                else
                {
                    CheckOverObjectProperties(ref props, ref dest, colsToMap.ElementAt(i).Value.ToUpper(), valToTest);
                }

            }

            return dest;
        }

        /// <summary>
        /// The MapAllLines
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colsToMap">The colsToMap<see cref="List{int}"/></param>
        /// <param name="skipHeader">The skipHeader<see cref="bool"/></param>
        /// <param name="regions">The regions<see cref="List{IDictionary{int, int}}"/></param>
        /// <returns>The <see cref="List{T}"/></returns>
        public List<T> MapAllLines<T>(List<int> colsToMap = null, bool skipHeader = false, List<IDictionary<int, int>> regions = null) where T : class, new()
        {
            var t = _csvReaderOption;
            int startingPoint = 0;
            if (skipHeader || _csvReaderOption.SkipHeader)
            {
                if (_csvReaderOption.Humanize)
                {
                    startingPoint = 2;
                }
                else
                {
                    startingPoint = 1;
                }

            }

            List<T> listObject = new List<T>();

            try
            {
                if (regions != null)
                {
                    for (int i = 0; i < regions.Count; i++)
                    {
                        for (int j = regions[i].Keys.First(); j <= regions[i].Values.First(); j++)
                        {
                            listObject.Add(MapLine<T>(j, colsToMap));
                        }
                    }
                }
                else
                {
                    for (int i = startingPoint; i < _datas.Length; i++)
                    {
                        listObject.Add(MapLine<T>(i, colsToMap));
                    }
                }

            }
            catch (Exception e)
            {
                throw;
            }
            return listObject;
        }

        /// <summary>
        /// The MapAllLinesTRestrict
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colsToMap">The colsToMap<see cref="Dictionary{int, string}"/></param>
        /// <param name="skipHeader">The skipHeader<see cref="bool"/></param>
        /// <param name="regions">The regions<see cref="List{IDictionary{int, int}}"/></param>
        /// <returns>The <see cref="List{T}"/></returns>
        public List<T> MapAllLinesTRestrict<T>(Dictionary<int, string> colsToMap, bool skipHeader = false, List<IDictionary<int, int>> regions = null) where T : class, new()
        {
            var t = _csvReaderOption;
            int startingPoint = 0;
            if (skipHeader || _csvReaderOption.SkipHeader)
            {
                if (_csvReaderOption.Humanize)
                {
                    startingPoint = 2;
                }
                else
                {
                    startingPoint = 1;
                }

            }

            List<T> listObject = new List<T>();

            try
            {
                if (regions != null)
                {
                    for (int i = 0; i < regions.Count; i++)
                    {
                        for (int j = regions[i].Keys.First(); j <= regions[i].Values.First(); j++)
                        {
                            listObject.Add(MapLineTRestrict<T>(j, colsToMap));
                        }
                    }
                }
                else
                {
                    for (int i = startingPoint; i < _datas.Length; i++)
                    {
                        listObject.Add(MapLineTRestrict<T>(i, colsToMap));
                    }
                }

            }
            catch (Exception e)
            {
                throw;
            }
            return listObject;
        }

        /// <summary>
        /// The ToJson
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colsToMap">The colsToMap<see cref="List{int}"/></param>
        /// <param name="regions">The regions<see cref="List{IDictionary{int, int}}"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string ToJson<T>(List<int> colsToMap = null, List<IDictionary<int, int>> regions = null) where T : class, new()
        {

            return JsonSerializer.Serialize<List<T>>(MapAllLines<T>(colsToMap: colsToMap, regions: regions));
        }

        /// <summary>
        /// The ToJsonLine
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="line">The line<see cref="int"/></param>
        /// <param name="colsToMap">The colsToMap<see cref="List{int}"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string ToJsonLine<T>(int line, List<int> colsToMap = null) where T : class, new()
        {
            return JsonSerializer.Serialize<T>(MapLine<T>(line, colsToMap));
        }

        /// <summary>
        /// The ToXmlLine
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="line">The line<see cref="int"/></param>
        /// <param name="outputPath">The outputPath<see cref="string"/></param>
        /// <param name="fileName">The fileName<see cref="string"/></param>
        public void ToXmlLine<T>(int line, string outputPath, string fileName = null) where T : class, new()
        {
            var lineN = MapLine<T>(line);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextWriter writer = new StreamWriter(Path.Combine(outputPath, string.IsNullOrEmpty(fileName) ? _csvReaderOption.XmlFileName : fileName));
            try
            {
                serializer.Serialize(writer, lineN);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// The ToXml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="outputPath">The outputPath<see cref="string"/></param>
        /// <param name="fileName">The fileName<see cref="string"/></param>
        /// <param name="colsToMap">The colsToMap<see cref="List{int}"/></param>
        /// <param name="regions">The regions<see cref="List{IDictionary{int, int}}"/></param>
        public void ToXml<T>(string outputPath, string fileName = null, List<int> colsToMap = null, List<IDictionary<int, int>> regions = null) where T : class, new()
        {
            var alllineN = MapAllLines<T>(colsToMap: colsToMap, regions: regions);

            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            TextWriter writer = new StreamWriter(Path.Combine(outputPath, string.Format("{0}.xml", string.IsNullOrEmpty(fileName) ? _csvReaderOption.XmlFileName : fileName)));
            try
            {
                serializer.Serialize(writer, alllineN);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// The CheckOverObjectProperties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="props">The props<see cref="PropertyInfo[]"/></param>
        /// <param name="dest">The dest<see cref="T"/></param>
        /// <param name="propertyToCheck">The propertyToCheck<see cref="string"/></param>
        /// <param name="valueToTest">The valueToTest<see cref="string"/></param>
        private void CheckOverObjectProperties<T>(ref PropertyInfo[] props, ref T dest, string propertyToCheck, string valueToTest)
        {
            foreach (PropertyInfo propInfo in props)
            {

                if (propInfo.Name.ToUpper().Equals(propertyToCheck))
                {
                    CheckPropertyType(propInfo, ref dest, valueToTest);
                }
            }
        }

        /// <summary>
        /// The CheckPropertyType
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propInfo">The propInfo<see cref="PropertyInfo"/></param>
        /// <param name="dest">The dest<see cref="T"/></param>
        /// <param name="valToTest">The valToTest<see cref="string"/></param>
        private void CheckPropertyType<T>(PropertyInfo propInfo, ref T dest, string valToTest)
        {

            if (propInfo.PropertyType == typeof(Int32))
            {
                propInfo.SetValue(dest, Convert.ToInt32(valToTest));
            }
            else
            if (propInfo.PropertyType == typeof(UInt32))
            {
                propInfo.SetValue(dest, Convert.ToUInt32(valToTest));
            }
            else
            if (propInfo.PropertyType == typeof(Nullable<Int32>))
            {

                if (string.IsNullOrEmpty(valToTest))
                {
                    propInfo.SetValue(dest, null);
                }
                else
                {
                    propInfo.SetValue(dest, Convert.ToInt32(valToTest));
                }
            }
            else
            if (propInfo.PropertyType == typeof(Int16))
            {

                propInfo.SetValue(dest, Convert.ToInt16(valToTest));
            }
            else
            if (propInfo.PropertyType == typeof(UInt16))
            {

                propInfo.SetValue(dest, Convert.ToUInt16(valToTest));
            }
            else
            if (propInfo.PropertyType == typeof(Int64))
            {

                propInfo.SetValue(dest, Convert.ToInt64(valToTest));
            }
            else
            if (propInfo.PropertyType == typeof(UInt64))
            {

                propInfo.SetValue(dest, Convert.ToUInt64(valToTest));
            }
            else
            if (propInfo.PropertyType == typeof(float))
            {

                propInfo.SetValue(dest, float.Parse(valToTest, CultureInfo.InvariantCulture.NumberFormat));
            }
            else
            if (propInfo.PropertyType == typeof(byte))
            {

                propInfo.SetValue(dest, Convert.ToByte(valToTest));
            }
            else
            if (propInfo.PropertyType == typeof(sbyte))
            {

                propInfo.SetValue(dest, Convert.ToSByte(valToTest));
            }
            else
            if (propInfo.PropertyType == typeof(double))
            {
                propInfo.SetValue(dest, Convert.ToDouble(valToTest));
            }
            else
            if (propInfo.PropertyType == typeof(Nullable<double>))
            {
                if (string.IsNullOrEmpty(valToTest))
                {
                    propInfo.SetValue(dest, null);
                }
                else
                {

                    propInfo.SetValue(dest, valToTest.Pow());

                }
            }
            else
            if (propInfo.PropertyType == typeof(decimal))
            {
                propInfo.SetValue(dest, Convert.ToDecimal(valToTest));
            }
            else
            if (propInfo.PropertyType == typeof(Nullable<decimal>))
            {

                if (string.IsNullOrEmpty(valToTest))
                {
                    propInfo.SetValue(dest, null);
                }
                else
                {
                    propInfo.SetValue(dest, Convert.ToDecimal(valToTest));
                }
            }
            else
            if (propInfo.PropertyType == typeof(DateTime))
            {

                propInfo.SetValue(dest, Convert.ToDateTime(valToTest));
            }
            else
            if (propInfo.PropertyType == typeof(Nullable<DateTime>))
            {
                if (string.IsNullOrEmpty(valToTest))
                {
                    propInfo.SetValue(dest, null);
                }
                else
                {
                    propInfo.SetValue(dest, Convert.ToDateTime(valToTest));
                }
            }
            else
            if (propInfo.PropertyType == typeof(String))
            {
                propInfo.SetValue(dest, valToTest);
            }
            else
            if (propInfo.PropertyType == typeof(Boolean))
            {

                propInfo.SetValue(dest, Convert.ToBoolean(valToTest));
            }
            else
            if (propInfo.PropertyType == typeof(Char))
            {

                propInfo.SetValue(dest, Convert.ToChar(valToTest));
            }
            else
            if (propInfo.PropertyType == typeof(Nullable<char>))
            {
                if (string.IsNullOrEmpty(valToTest))
                {
                    propInfo.SetValue(dest, null);
                }
                else
                {
                    propInfo.SetValue(dest, Convert.ToChar(valToTest));
                }
            }
        }

        /// <summary>
        /// The ReduceLine
        /// </summary>
        /// <param name="line">The line<see cref="string[]"/></param>
        /// <param name="colsToMap">The colsToMap<see cref="List{int}"/></param>
        /// <returns>The <see cref="string[]"/></returns>
        private string[] ReduceLine(string[] line, List<int> colsToMap)
        {
            string[] newLine = new string[colsToMap.Count];

            if (_csvReaderOption.Humanize)
            {
                // Passe d'une indexation humaine à une indexaton programmatique
                colsToMap = colsToMap.Select(x => x -= 1).ToList();
            }

            for (int i = 0; i < colsToMap.Count; i++)
            {
                newLine[i] = line[colsToMap.ElementAt(i)];
            }
            return newLine;
        }

        /// <summary>
        /// The SearchValue
        /// </summary>
        /// <param name="value">The value<see cref="string"/></param>
        /// <returns>The <see cref="List{IDictionary{int, int}}"/></returns>
        private List<IDictionary<int, int>> SearchValue(string value)
        {
            List<IDictionary<int, int>> positions = new List<IDictionary<int, int>>();

            for (int x = 0; x < _datas.Length; x++)
            {
                var line = GetLineInArray(x);

                // x = ligne
                var val = line.Where(x => x.Equals(value));

                // y = colonne
                if (val.Count() > 0)
                {
                    for (int y = 0; y < line.Count(); y++)
                    {
                        if (line[y].Equals(value))
                        {
                            IDictionary<int, int> xy = new Dictionary<int, int>();
                            xy.Add(new KeyValuePair<int, int>(x, y));
                            positions.Add(xy);
                        }
                    }
                }
            }
            return positions;
        }

        /// <summary>
        /// The TrimEndCellWithNoValue
        /// </summary>
        /// <param name="aLine">The aLine<see cref="string[]"/></param>
        /// <returns>The <see cref="string[]"/></returns>
        private string[] TrimEndCellWithNoValue(string[] aLine)
        {
            List<string> tempNewLine = new List<string>();

            for (int j = aLine.Length; j > 0; j--)
            {
                if ((aLine[j - 1] != "" || !string.IsNullOrEmpty(aLine[j - 1]))
                || (tempNewLine.Count > 0 && ((aLine[j - 1] == "" || string.IsNullOrEmpty(aLine[j - 1])))))
                {
                    tempNewLine.Add(aLine[j - 1]);
                }
            }

            int size = tempNewLine.Count;
            string[] returnNewLine = new string[size];
            int sizeNewLine = size;
            // Remettre la ligne dans l'ordre
            for (int i = 0; i < size; i++)
            {
                returnNewLine[--sizeNewLine] = tempNewLine[i];
            }

            return returnNewLine;
        }

        /// <summary>
        /// Converts a file from one encoding to another.
        /// </summary>
        /// <param name="sourcePath">the file to convert</param>
        /// <param name="destPath">the destination for the converted file</param>
        /// <param name="sourceEncoding">the original file encoding</param>
        /// <param name="destEncoding">the encoding to which the contents should be converted</param>
        public static void ConvertFileEncoding(String sourcePath, String destPath,
                                               Encoding sourceEncoding, Encoding destEncoding)
        {

            // If the destination's parent doesn't exist, create it.
            String parent = Path.GetDirectoryName(Path.GetFullPath(destPath));
            if (!Directory.Exists(parent))
            {
                Directory.CreateDirectory(parent);
            }

            // If the source and destination encodings are the same, just copy the file.
            if (sourceEncoding == destEncoding)
            {
                File.Copy(sourcePath, destPath, true);
                return;
            }

            // Convert the file.
            String tempName = null;
            try
            {
                tempName = Path.GetTempFileName();
                // using (StreamReader sr = new StreamReader(sourcePath, sourceEncoding, false))
                using (StreamReader sr = new StreamReader(sourcePath))
                {
                    using (StreamWriter sw = new StreamWriter(tempName, false, destEncoding))
                    {
                        int charsRead;
                        char[] buffer = new char[128 * 1024];
                        while ((charsRead = sr.ReadBlock(buffer, 0, buffer.Length)) > 0)
                        {
                            sw.Write(buffer, 0, charsRead);
                        }
                    }
                }
                File.Delete(destPath);
                File.Move(tempName, destPath);
            }
            finally
            {
                File.Delete(tempName);
            }
        }
    }
}
