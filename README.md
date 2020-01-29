# MyCsvReader

## A small library that allows you to read and write CSV files.

- Get line number.

- Get the column number.
- Trim the empty end-of-line columns.
- Get values from a line in text or table format.
- Get line blocks.
- Get only certain lines
- Get a value at certain position

<u>More advanced functions:</u>

- Map a line in an object.
- Map certain columns in certain properties of an object ...

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

