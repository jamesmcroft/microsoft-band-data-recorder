// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionExtensions.cs" company="James Croft">
//   Copyright (c) 2015 James Croft.
// </copyright>
// <summary>
//   The collection extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BandDataRecorder.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// The collection extensions.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Converts a collection of items to a CSV string.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <typeparam name="T">
        /// The type of object in the collection.
        /// </typeparam>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToCsvString<T>(this IEnumerable<T> collection, string separator)
        {
            var objType = typeof(T);
            var fields = objType.GetFields();

            var headerRow = string.Join(separator, fields.Select(field => field.Name).ToArray());

            var csvDataSet = new StringBuilder();
            csvDataSet.AppendLine(headerRow);

            foreach (var item in collection)
            {
                csvDataSet.AppendLine(GetCsvFieldsAsString(separator, fields, item));
            }

            return csvDataSet.ToString();
        }

        private static string GetCsvFieldsAsString(string separator, IEnumerable<FieldInfo> fields, object item)
        {
            var row = new StringBuilder();

            foreach (var field in fields)
            {
                if (row.Length > 0)
                {
                    row.Append(separator);
                }

                var fieldVal = field.GetValue(item);

                if (fieldVal != null)
                {
                    row.Append(fieldVal);
                }
            }

            return row.ToString();
        }
    }
}