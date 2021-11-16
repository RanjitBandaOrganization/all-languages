using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CourseLibrary.API.Services
{
    //We're looking at the GetAuthors method.
    //Theoretically, we could reuse the check we previously wrote for checking the OrderBy clause.
    //However, that wouldn't be correct.
    //It will work because we stated that we need a mapping for each property.
    //So for all our fields on the DTO, mappings will exist.
    //But our data shaping component doesn't use that mappingDictionary.
    //So if you have a resource collection that supports data shaping,
    //but not ordering, well, we're out of luck.
    //And it's also not a good separation of concerns.
    //Thus, it's better to keep these separate.
    public class PropertyCheckerService : IPropertyCheckerService
    {
        public bool TypeHasProperties<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            // the field are separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',');

            // check if the requested fields exist on source
            foreach (var field in fieldsAfterSplit)
            {
                // trim each field, as it might contain leading 
                // or trailing spaces. Can't trim the var in foreach,
                // so use another var.
                var propertyName = field.Trim();

                // use reflection to check if the property can be
                // found on T. 
                var propertyInfo = typeof(T)
                    .GetProperty(propertyName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                // it can't be found, return false
                if (propertyInfo == null)
                {
                    return false;
                }
            }

            // all checks out, return true
            return true;
        }
    }
}
