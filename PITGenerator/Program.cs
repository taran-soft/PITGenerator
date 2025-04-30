// See https://aka.ms/new-console-template for more information

using TaranSoft.PITConverter.Writers;
using TaranSoft.PITGenerator;

Console.WriteLine("PIT Converter started...");

var convertedRows = await new PITFileGenerator().Generate("p2p.report.csv");

CsvWriterHelper.WriteCsvFile("PITResult.csv", convertedRows.ToList());

Console.WriteLine("PIT Converter finished...");

Console.ReadKey();