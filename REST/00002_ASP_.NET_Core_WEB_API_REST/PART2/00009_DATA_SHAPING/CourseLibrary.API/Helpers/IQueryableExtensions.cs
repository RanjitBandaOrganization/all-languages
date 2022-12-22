using CourseLibrary.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace CourseLibrary.API.Helpers
{
    public static class IQueryableExtensions
    {
        //As the original demo interacts with DB so they used IQUERYABLE, for us its in memory so IEnumerable is fine
        //public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy,
        public static IEnumerable<T> ApplySort<T>(this IEnumerable<T> source, string orderBy,

       Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (mappingDictionary == null)
            {
                throw new ArgumentNullException(nameof(mappingDictionary));
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderByString = string.Empty;

            // the orderBy string is separated by ",", so we split it.
            var orderByAfterSplit = orderBy.Split(',');

            // apply each orderby clause  
            foreach (var orderByClause in orderByAfterSplit)
            {
                // trim the orderBy clause, as it might contain leading
                // or trailing spaces. Can't trim the var in foreach,
                // so use another var.
                var trimmedOrderByClause = orderByClause.Trim();

                // if the sort option ends with with " desc", we order
                // descending, ortherwise ascending
                var orderDescending = trimmedOrderByClause.EndsWith(" desc");

                // remove " asc" or " desc" from the orderBy clause, so we 
                // get the property name to look for in the mapping dictionary
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

                // find the matching property
                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                // get the PropertyMappingValue
                var propertyMappingValue = mappingDictionary[propertyName];

                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }

                // revert sort order if necessary
                if (propertyMappingValue.Revert)
                {
                    orderDescending = !orderDescending;
                }

                // Run through the property names 
                // so the orderby clauses are applied in the correct order
                foreach (var destinationProperty in 
                    propertyMappingValue.DestinationProperties)
                {
                    orderByString = orderByString + 
                        (string.IsNullOrWhiteSpace(orderByString) ? string.Empty : ", ") 
                        + destinationProperty 
                        + (orderDescending ? " descending" : " ascending");                     
                }
            }

            //As Dynamic LINQ OrderBy works for IQUERYABLE only,
            //so we are converting the IENUMERABLE to ASQUERYABLE to make this work
            return source.AsQueryable().OrderBy(orderByString);
        }
    }
}
