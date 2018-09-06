

#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System.Data;
#endregion

namespace SmartEP.Utilities.FileFormats.ExtensionMethods
{
    /// <summary>
    /// Extension methods pertaining to file formats
    /// </summary>
    public static class ExtensionMethods
    {
        #region Functions

        #region ToCSV
        
        /// <summary>
        /// Converts an IEnumerable to a CSV file
        /// </summary>
        /// <typeparam name="T">Type of the items within the list</typeparam>
        /// <param name="List">The list to convert</param>
        /// <returns>The CSV file containing the list</returns>
        public static CSV.CSV ToCSV<T>(this IEnumerable<T> List)
        {
            return List.ToDataTable().ToCSV();
        }

        /// <summary>
        /// Converts an IEnumerable to a CSV file
        /// </summary>
        /// <param name="Data">The DataTable to convert</param>
        /// <returns>The CSV file containing the list</returns>
        public static CSV.CSV ToCSV(this DataTable Data)
        {
            CSV.CSV ReturnValue = new CSV.CSV();
            if (Data.IsNull())
                return ReturnValue;
            Delimited.Row TempRow = new Delimited.Row(",");
            foreach (DataColumn Column in Data.Columns)
            {
                TempRow.Cells.Add(new Delimited.Cell(Column.ColumnName));
            }
            ReturnValue.Rows.Add(TempRow);
            foreach (DataRow Row in Data.Rows)
            {
                TempRow = new Delimited.Row(",");
                for (int x = 0; x < Data.Columns.Count; ++x)
                {
                    TempRow.Cells.Add(new Delimited.Cell(Row.ItemArray[x].ToString()));
                }
                ReturnValue.Rows.Add(TempRow);
            }
            return ReturnValue;
        }

        #endregion

        #region ToDelimitedFile

        /// <summary>
        /// Converts an IEnumerable to a delimited file
        /// </summary>
        /// <typeparam name="T">Type of the items within the list</typeparam>
        /// <param name="List">The list to convert</param>
        /// <returns>The delimited file containing the list</returns>
        public static Delimited.Delimited ToDelimitedFile<T>(this IEnumerable<T> List, string Delimiter = "\t")
        {
            return List.ToDataTable().ToDelimitedFile(Delimiter);
        }

        /// <summary>
        /// Converts an IEnumerable to a delimited file
        /// </summary>
        /// <param name="Data">The DataTable to convert</param>
        /// <returns>The delimited file containing the list</returns>
        public static Delimited.Delimited ToDelimitedFile(this DataTable Data, string Delimiter = "\t")
        {
            GenericDelimited.GenericDelimited ReturnValue = new GenericDelimited.GenericDelimited(Delimiter);
            if (Data.IsNull())
                return ReturnValue;
            Delimited.Row TempRow = new Delimited.Row(Delimiter);
            foreach (DataColumn Column in Data.Columns)
            {
                TempRow.Cells.Add(new Delimited.Cell(Column.ColumnName));
            }
            ReturnValue.Rows.Add(TempRow);
            foreach (DataRow Row in Data.Rows)
            {
                TempRow = new Delimited.Row(Delimiter);
                for (int x = 0; x < Data.Columns.Count; ++x)
                {
                    TempRow.Cells.Add(new Delimited.Cell(Row.ItemArray[x].ToString()));
                }
                ReturnValue.Rows.Add(TempRow);
            }
            return ReturnValue;
        }

        #endregion

        #endregion
    }
}
