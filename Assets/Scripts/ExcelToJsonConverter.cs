using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Excel;
using UnityEngine;

public class ExcelToJsonConverter
{
	////[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ExcelToJsonConverter.ConversionToJsonSuccessfullHandler ConversionToJsonSuccessfull;



	////[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ExcelToJsonConverter.ConversionToJsonFailedHandler ConversionToJsonFailed;



	public void ConvertExcelFilesToJson(string inputPath, string outputPath, bool recentlyModifiedOnly = false)
	{
		List<string> list = this.GetExcelFileNamesInDirectory(inputPath);
		UnityEngine.Debug.Log("Excel To Json Converter: " + list.Count.ToString() + " excel files found.");
		if (recentlyModifiedOnly)
		{
			list = this.RemoveUnmodifiedFilesFromProcessList(list, outputPath);
			if (list.Count == 0)
			{
				UnityEngine.Debug.Log("Excel To Json Converter: No updates to excel files since last conversion.");
			}
			else
			{
				UnityEngine.Debug.Log("Excel To Json Converter: " + list.Count.ToString() + " excel files updated/added since last conversion.");
			}
		}
		bool flag = true;
		for (int i = 0; i < list.Count; i++)
		{
			if (!this.ConvertExcelFileToJson(list[i], outputPath))
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			this.ConversionToJsonSuccessfull();
		}
		else
		{
			this.ConversionToJsonFailed();
		}
	}

	private List<string> GetExcelFileNamesInDirectory(string directory)
	{
		string[] files = Directory.GetFiles(directory);
		List<string> list = new List<string>();
		Regex regex = new Regex("^((?!(~\\$)).*\\.(xlsx|xls$))$");
		for (int i = 0; i < files.Length; i++)
		{
			string input = files[i].Substring(files[i].LastIndexOf('/') + 1);
			if (regex.IsMatch(input))
			{
				list.Add(files[i]);
			}
		}
		return list;
	}

	public bool ConvertExcelFileToJson(string filePath, string outputPath)
	{
		UnityEngine.Debug.Log("Excel To Json Converter: Processing: " + filePath);
		DataSet excelDataSet = this.GetExcelDataSet(filePath);
		if (excelDataSet == null)
		{
			UnityEngine.Debug.LogError("Excel To Json Converter: Failed to process file: " + filePath);
			return false;
		}
		string text = string.Empty;
		for (int i = 0; i < excelDataSet.Tables.Count; i++)
		{
			text = this.GetSpreadSheetJson(excelDataSet, excelDataSet.Tables[i].TableName);
			if (string.IsNullOrEmpty(text))
			{
				UnityEngine.Debug.LogError("Excel To Json Converter: Failed to covert Spreadsheet '" + excelDataSet.Tables[i].TableName + "' to json.");
				return false;
			}
			string str = excelDataSet.Tables[i].TableName.Replace(" ", string.Empty);
			UnityEngine.Debug.LogWarning("????" + text);
			this.WriteTextToFile(text, outputPath + "/" + str + ".json");
			UnityEngine.Debug.Log("Excel To Json Converter: " + excelDataSet.Tables[i].TableName + " successfully written to file.");
		}
		return true;
	}

	private IExcelDataReader GetExcelDataReaderForFile(string filePath)
	{
		FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
		Regex regex = new Regex("^(.*\\.(xls$))");
		Regex regex2 = new Regex("^(.*\\.(xlsx$))");
		IExcelDataReader excelDataReader;
		if (regex.IsMatch(filePath))
		{
			excelDataReader = ExcelReaderFactory.CreateBinaryReader(fileStream);
		}
		else
		{
			if (!regex2.IsMatch(filePath))
			{
				UnityEngine.Debug.LogError("Excel To Json Converter: Unexpected files type: " + filePath);
				fileStream.Close();
				return null;
			}
			excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
		}
		fileStream.Close();
		excelDataReader.IsFirstRowAsColumnNames = true;
		return excelDataReader;
	}

	private DataSet GetExcelDataSet(string filePath)
	{
		IExcelDataReader excelDataReaderForFile = this.GetExcelDataReaderForFile(filePath);
		if (excelDataReaderForFile == null)
		{
			return null;
		}
		DataSet dataSet = new DataSet();
		do
		{
			DataTable excelSheetData = this.GetExcelSheetData(excelDataReaderForFile);
			if (excelSheetData != null)
			{
				dataSet.Tables.Add(excelSheetData);
			}
		}
		while (excelDataReaderForFile.NextResult());
		return dataSet;
	}

	private DataTable GetExcelSheetData(IExcelDataReader excelReader)
	{
		if (excelReader == null)
		{
			UnityEngine.Debug.LogError("Excel To Json Converter: Excel Reader is null. Cannot read data");
			return null;
		}
		Regex regex = new Regex("^~.*$");
		UnityEngine.Debug.Log(excelReader.Name);
		if (regex.IsMatch(excelReader.Name))
		{
			return null;
		}
		DataTable dataTable = new DataTable(excelReader.Name);
		dataTable.Clear();
		string text = string.Empty;
		while (excelReader.Read())
		{
			DataRow dataRow = dataTable.NewRow();
			bool flag = true;
			for (int i = 0; i < excelReader.FieldCount; i++)
			{
				if (!excelReader.IsDBNull(i) || (excelReader.Depth != 1 && i <= dataTable.Columns.Count - 1))
				{
					text = ((!excelReader.IsDBNull(i)) ? excelReader.GetString(i) : string.Empty);
					if (excelReader.Depth == 1)
					{
						dataTable.Columns.Add(text);
					}
					else
					{
						dataRow[dataTable.Columns[i]] = text;
					}
					if (!string.IsNullOrEmpty(text))
					{
						flag = false;
					}
				}
			}
			if (excelReader.Depth != 1 && !flag)
			{
				dataTable.Rows.Add(dataRow);
			}
		}
		return dataTable;
	}

	private string GetSpreadSheetJson(DataSet excelDataSet, string sheetName)
	{
		DataTable dataTable = excelDataSet.Tables[sheetName];
		for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
		{
			bool flag = true;
			IEnumerator enumerator = dataTable.Rows.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					DataRow dataRow = (DataRow)obj;
					if (!dataRow.IsNull(i))
					{
						flag = false;
						break;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			if (flag)
			{
				dataTable.Columns.RemoveAt(i);
			}
		}
		Regex regex = new Regex("^~.*$");
		for (int j = dataTable.Columns.Count - 1; j >= 0; j--)
		{
			if (regex.IsMatch(dataTable.Columns[j].ColumnName))
			{
				dataTable.Columns.RemoveAt(j);
			}
		}
		return this.DataTableToJSONWithStringBuilder2(dataTable);
	}

	public string DataTableToJSONWithStringBuilder(DataTable table)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (table.Rows.Count > 0)
		{
			stringBuilder.Append("[");
			for (int i = 0; i < table.Rows.Count; i++)
			{
				stringBuilder.Append("{");
				for (int j = 0; j < table.Columns.Count; j++)
				{
					if (j < table.Columns.Count - 1)
					{
						stringBuilder.Append(string.Concat(new string[]
						{
							"\"",
							table.Columns[j].ColumnName.ToString(),
							"\":\"",
							table.Rows[i][j].ToString(),
							"\","
						}));
					}
					else if (j == table.Columns.Count - 1)
					{
						stringBuilder.Append(string.Concat(new string[]
						{
							"\"",
							table.Columns[j].ColumnName.ToString(),
							"\":\"",
							table.Rows[i][j].ToString(),
							"\""
						}));
					}
				}
				if (i == table.Rows.Count - 1)
				{
					stringBuilder.Append("}");
				}
				else
				{
					stringBuilder.Append("},");
				}
			}
			stringBuilder.Append("]");
		}
		return stringBuilder.ToString();
	}

	public string DataTableToJSONWithStringBuilder2(DataTable table)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (table.Rows.Count > 0)
		{
			stringBuilder.Append("{");
			stringBuilder.AppendLine();
			for (int i = 0; i < table.Rows.Count; i++)
			{
				stringBuilder.Append("    \"" + table.Rows[i][0].ToString() + "\":");
				stringBuilder.AppendLine();
				stringBuilder.Append("        [");
				for (int j = 1; j < table.Columns.Count; j++)
				{
					if (j < table.Columns.Count - 1)
					{
						stringBuilder.Append("\"" + table.Rows[i][j].ToString() + "\",");
					}
					else if (j == table.Columns.Count - 1)
					{
						stringBuilder.Append("\"" + table.Rows[i][j].ToString() + "\"]");
					}
				}
				if (i == table.Rows.Count - 1)
				{
					stringBuilder.AppendLine();
				}
				else
				{
					stringBuilder.Append(",");
					stringBuilder.AppendLine();
				}
			}
			stringBuilder.Append("}");
		}
		return stringBuilder.ToString();
	}

	private void WriteTextToFile(string text, string filePath)
	{
		File.WriteAllText(filePath, text);
	}

	private List<string> RemoveUnmodifiedFilesFromProcessList(List<string> excelFiles, string outputDirectory)
	{
		Regex regex = new Regex("^~.*$");
		for (int i = excelFiles.Count - 1; i >= 0; i--)
		{
			List<string> sheetNamesInFile = this.GetSheetNamesInFile(excelFiles[i]);
			bool flag = true;
			for (int j = 0; j < sheetNamesInFile.Count; j++)
			{
				if (!regex.IsMatch(sheetNamesInFile[j]))
				{
					string path = outputDirectory + "/" + sheetNamesInFile[j] + ".json";
					if (!File.Exists(path) || File.GetLastWriteTimeUtc(excelFiles[i]) > File.GetLastWriteTimeUtc(path))
					{
						flag = false;
					}
				}
			}
			if (flag)
			{
				excelFiles.RemoveAt(i);
			}
		}
		return excelFiles;
	}

	private List<string> GetSheetNamesInFile(string filePath)
	{
		List<string> list = new List<string>();
		IExcelDataReader excelDataReaderForFile = this.GetExcelDataReaderForFile(filePath);
		if (excelDataReaderForFile == null)
		{
			return list;
		}
		do
		{
			list.Add(excelDataReaderForFile.Name);
		}
		while (excelDataReaderForFile.NextResult());
		return list;
	}

	public delegate void ConversionToJsonSuccessfullHandler();

	public delegate void ConversionToJsonFailedHandler();
}
