# MyCsvReader

## A small library that allows you to read and write CSV files.

- Get line number.
- Get the column number.
- Trim the empty end-of-line columns.
- Get values from a line in text or table format.
- Get line blocks.
- Get only certain lines
- Get a value at certain position
- And more ...

<u>More advanced functions:</u>

- Map a line in an object.
- Map certain columns in certain properties of an object ...
- Export to Json *(All lines , just one or a selection)*
- Exportt to Xml *(All lines  just one or a selection)*

#### Install

You can specify some option

```C#
   CsvReaderOption readerOption = new CsvReaderOption();
            readerOption.Humanize = true;
            readerOption.SkipHeader = true;
```

And then call the reader :

```C#
var reader = MyCsvReader.GetCsvReader(Path.Combine(_baseFile, @"Myfile.csv"), readerOption);
```

For example call `GetValue` function

```c#
  var result = reader.GetValue(4, 2);
```

Export to Json example

```c#
           CsvReaderOption readerOption = new CsvReaderOption();
            readerOption.Humanize = true;
            readerOption.SkipHeader = false;
            var reader = MyCsvReader.GetCsvReader(Path.Combine(_baseFile, @"MyFile.csv"), readerOption);
           

```

Specify some region to map

```c#
 var dicRegion1 = new Dictionary<int, int>();
            dicRegion1.Add(5, 10);
            var dicRegion2 = new Dictionary<int, int>();
            dicRegion2.Add(12, 12);
            var Listregion = new List<IDictionary<int, int>>();
            Listregion.Add(dicRegion1);
            Listregion.Add(dicRegion2);
```

And export (cols specify : 2,3,16)

```c#
            var res = reader.ToJson<MyDTO>(colsToMap: new List<int> { 2, 3, 16 }, regions: Listregion);
```

