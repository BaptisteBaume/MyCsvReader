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
    /// Defines the <see cref="ICsvReader" />
    /// </summary>
    public interface ICsvReader
    {
        /// <summary>
        /// Retourne le nombre de ligne d'un document
        /// </summary>
        /// <returns></returns>
        int CountLines();

        /// <summary>
        /// Compte le nombre de colonne du document (inclus les cellues vides s'il y en a)
        /// </summary>
        /// <returns></returns>
        int CountColumns();

        /// <summary>
        /// Retourne le nombre de colonnes pour une ligne donnée.
        /// La ligne peut être trimmée.
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <param name="trim">The trim<see cref="bool"/></param>
        /// <returns></returns>
        int CountColumns(int lineNumber, bool trim = false);

        /// <summary>
        /// Retourne une ligne sous forme de chaîne de charactères.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="trim"></param>
        /// <returns></returns>
        string GetLine(int line, bool trim = false);

        /// <summary>
        /// Retourne une ligne sous forme de tableau
        /// </summary>
        /// <param name="line"></param>
        /// <param name="trim"></param>
        /// <returns></returns>
        string[] GetLineInArray(int line, bool trim = false);

        /// <summary>
        /// Retourne la valeur d'un champs à une position donnée.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        string GetValue(int line, int column);

        /// <summary>
        /// Retourne la liste des cellues où la valeur a été trouvée
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        List<IDictionary<int, int>> FindValues(string value);

        /// <summary>
        /// Retourne le nombre d'occurrence d'une valeur
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int GetNbOccurence(string value);

        /// <summary>
        /// Mappe une ligne dans un object donné
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lineNumber"></param>
        /// <param name="colsToMap"></param>
        /// <returns></returns>
        T MapLine<T>(int lineNumber, List<int> colsToMap = null) where T : class, new();

        /// <summary>
        /// Mappe les colonnes définies dans la propriété de l'object associée pour 1 ligne donnée.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lineNumber"></param>
        /// <param name="colsToMap"></param>
        /// <returns></returns>
        T MapLineTRestrict<T>(int lineNumber, Dictionary<int, string> colsToMap = null) where T : class, new();

        /// <summary>
        /// Mappe toutes les lignes du document dans une liste d'objet donné
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colsToMap"></param>
        /// <param name="skipHeader"></param>
        /// <param name="regions"></param>
        /// <returns></returns>
        List<T> MapAllLines<T>(List<int> colsToMap = null, bool skipHeader = false, List<IDictionary<int, int>> regions = null) where T : class, new();

        /// <summary>
        /// Mappe les colonnes définies dans la propriété de l'object associée.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colsToMap"></param>
        /// <param name="skipHeader"></param>
        /// <param name="regions"></param>
        /// <returns></returns>
        List<T> MapAllLinesTRestrict<T>(Dictionary<int, string> colsToMap, bool skipHeader = false, List<IDictionary<int, int>> regions = null) where T : class, new();

        /// <summary>
        /// Mappe toutes les lignes du document dans une liste d'objet donné puis sérialise au format Json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colsToMap">The colsToMap<see cref="List{int}"/></param>
        /// <param name="regions">The regions<see cref="List{IDictionary{int, int}}"/></param>
        /// <returns></returns>
        string ToJson<T>(List<int> colsToMap = null, List<IDictionary<int, int>> regions = null) where T : class, new();

        /// <summary>
        /// Mappe une ligne donnée dans un objet donné et le sérialize en Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="line"></param>
        /// <param name="colsToMap">The colsToMap<see cref="List{int}"/></param>
        /// <returns></returns>
        string ToJsonLine<T>(int line, List<int> colsToMap = null) where T : class, new();

        /// <summary>
        /// Mappe une ligne choisie dans objet et la sérialisz au format XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="line"></param>
        /// <param name="outputPath"></param>
        /// <param name="fileName"></param>
        void ToXmlLine<T>(int line, string outputPath, string fileName = null) where T : class, new();

        /// <summary>
        /// Mappe tout le document dans un objet donné et le sérialise en XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="outputPath"></param>
        /// <param name="fileName"></param>
        /// <param name="colsToMap">The colsToMap<see cref="List{int}"/></param>
        /// <param name="regions">The regions<see cref="List{IDictionary{int, int}}"/></param>
        void ToXml<T>(string outputPath, string fileName = null, List<int> colsToMap = null, List<IDictionary<int, int>> regions = null) where T : class, new();
    }
}
