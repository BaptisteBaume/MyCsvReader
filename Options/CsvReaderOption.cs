//  ***************************************
//                                                   
//                     CsvReader
//                                                   
//  ***************************************
//
//  Copyright (c) 2020 All Rights Reserved

namespace CsvReader.Options
{
    using CsvReader.Extensions;
    using System;

    /// <summary>
    /// Defines the <see cref="CsvReaderOption" />
    /// </summary>
    public class CsvReaderOption
    {
        /// <summary>
        /// Defines the _separator
        /// </summary>
        private string _separator = ";";

        /// <summary>
        /// Defines the _humanize
        /// </summary>
        private bool _humanize = false;

        /// <summary>
        /// Defines the _xmmFileName
        /// </summary>
        private string _xmmFileName = string.Format("Output_XML_{0}", DateTime.Now.ToNameFile());

        /// <summary>
        /// Defines the _skipHeader
        /// </summary>
        private bool _skipHeader = false;

        /// <summary>
        /// Defines the _endOfLine
        /// </summary>
        private string _endOfLine = "\n";

        /// <summary>
        /// Defines the _csvFileName
        /// </summary>
        private string _csvFileName = string.Format("Output_CSV_{0}", DateTime.Now.ToNameFile());

        /// <summary>
        /// Defines the _skipFirstCol
        /// </summary>
        private bool _skipFirstCol = false;

        /// <summary>
        /// Defines the _writeHeader
        /// </summary>
        private bool _writeHeader = false;

        /// <summary>
        /// Defines the _capitalizeHeader
        /// </summary>
        private bool _capitalizeHeader = false;

        /// <summary>
        /// Gets or sets the Separator
        /// Caractère de séparation
        /// </summary>
        public string Separator
        {
            get { return _separator; }
            set { _separator = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Humanize
        /// Converti les numéros de ligne en compréhension humaine
        /// </summary>
        public bool Humanize
        {
            get { return _humanize; }
            set { _humanize = value; }
        }

        /// <summary>
        /// Gets or sets the XmlFileName
        /// Nom di fichier XMl
        /// </summary>
        public string XmlFileName
        {
            get { return _xmmFileName; }
            set { _xmmFileName = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether SkipHeader
        /// Ne prend pas en compte la première ligne du fichier
        /// </summary>
        public bool SkipHeader
        {
            get { return _skipHeader; }
            set { _skipHeader = value; }
        }

        /// <summary>
        /// Gets or sets the EndOfLine
        /// Caractère de fin de ligne
        /// </summary>
        public string EndOfLine
        {
            get { return _endOfLine; }
            set { _endOfLine = value; }
        }

        /// <summary>
        /// Gets or sets the CsvFileName
        /// Nom du fichier de sortie
        /// </summary>
        public string CsvFileName
        {
            get { return _csvFileName; }
            set { _csvFileName = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether SkipFirstCol
        /// </summary>
        public bool SkipFirstCol
        {
            get { return _skipFirstCol; }
            set { _skipFirstCol = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether WriteHeader
        /// Reprend les propriétés de l'objet afin de la écrire en tant qu'header de colonne dans le fichier CSV.
        /// </summary>
        public bool WriteHeader
        {
            get { return _writeHeader; }
            set { _writeHeader = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether CapitalizeHeader
        /// Met en majuscule les headers (intitulés de colonnes)
        /// </summary>
        public bool CapitalizeHeader
        {
            get { return _capitalizeHeader; }
            set { _capitalizeHeader = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvReaderOption"/> class.
        /// </summary>
        public CsvReaderOption()
        {
        }
    }
}
