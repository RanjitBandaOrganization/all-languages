using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CourseLibrary.API.Helpers
{
    //There's one big difference between the ShapeData method on object(ObjectExtensions) versus the one on an IEnumerable of object(IEnumerableExtensions).
    //The reason is performance. As explained when we implemented the ShapeData method on IEnumerable of TSource,
    //reflection is expensive.
    //In our ShapeData method on IEnumerable TSource,
    //we get around that by saving a list of PropertyInfo objects,
    //and we reuse those for each object in the list.
    //That's what you see on the bottom part of the screen.
    //On the top part of the screen, you'll see that we don't save such a list.
    //We simply get the propertyInfo we need for this object as we're only working on one object.
    //But were we to use the ShapeData extension method on object, the one on top of the screen,
    //for each object in the list of authors, we'd have this additional piece of reflection for each object.
    //By creating two extensions methods, we avoid this performance impact.
    //The REST of the ShapeData methods on object follows the exact same logic as the other one.
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(
            this IEnumerable<TSource> source,
            string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            // create a list to hold our ExpandoObjects
            var expandoObjectList = new List<ExpandoObject>();

            // create a list with PropertyInfo objects on TSource.  Reflection is
            // expensive, so rather than doing it for each object in the list, we do 
            // it once and reuse the results.  After all, part of the reflection is on the 
            // type of the object (TSource), not on the instance
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                // all public properties should be in the ExpandoObject
                var propertyInfos = typeof(TSource)
                        .GetProperties(BindingFlags.IgnoreCase 
                        | BindingFlags.Public | BindingFlags.Instance);

                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                // the field are separated by ",", so we split it.
                var fieldsAfterSplit = fields.Split(',');

                foreach (var field in fieldsAfterSplit)
                {
                    // trim each field, as it might contain leading 
                    // or trailing spaces. Can't trim the var in foreach,
                    // so use another var.
                    var propertyName = field.Trim();

                    // use reflection to get the property on the source object
                    // we need to include public and instance, b/c specifying a binding 
                    // flag overwrites the already-existing binding flags.
                    var propertyInfo = typeof(TSource)
                        .GetProperty(propertyName, BindingFlags.IgnoreCase |
                        BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new Exception($"Property {propertyName} wasn't found on" +
                            $" {typeof(TSource)}");
                    }

                    // add propertyInfo to list 
                    propertyInfoList.Add(propertyInfo);
                }
            }

            // run through the source objects
            foreach (TSource sourceObject in source)
            {
                // create an ExpandoObject that will hold the 
                // selected properties & values
                var dataShapedObject = new ExpandoObject();

                // Get the value of each property we have to return.  For that,
                // we run through the list
                foreach (var propertyInfo in propertyInfoList)
                {  
                    // GetValue returns the value of the property on the source object
                    var propertyValue = propertyInfo.GetValue(sourceObject);

                    // add the field to the ExpandoObject
                    ((IDictionary<string, object>)dataShapedObject)
                        .Add(propertyInfo.Name, propertyValue);
                }

                // add the ExpandoObject to the list
                expandoObjectList.Add(dataShapedObject);
            }

            // return the list
            return expandoObjectList;
        }
    }
}
