using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        //We know this service has to contain a list of PropertyMappings.
        //And a PropertyMapping, in turn, contains a dictionary with a key of string and a value of PropertyMappingValue.
        private Dictionary<string, PropertyMappingValue> _authorPropertyMapping =
          new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
          {
               { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
               { "MainCategory", new PropertyMappingValue(new List<string>() { "MainCategory" } )},

               //we can map Age to DateOfBirth using reverse ordering
               { "Age", new PropertyMappingValue(new List<string>() { "DateOfBirth" } , true) },

               //we are mapping from Name to FirstName and LastName.
               { "Name", new PropertyMappingValue(new List<string>() { "FirstName", "LastName" }) }
          };

        //We can now add an IList of PropertyMapping from TSource to TDestination.
        //But there seems to be a problem with this. TSource and TDestination cannot be resolved.
        //private IList<PropertyMapping<TSource, TDestination>> _propertyMappings = new List<TSource, TDestination>();

        //That can be overcome by using a marker interface.
        //That's an interface without any methods in it.
        //It's often used for cases like this.
        //If we let our PropertyMapping class implement the marker interface,
        //we can register an Ilist of that marker interface.
        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            //And in this constructor,
            //we can now add a new PropertyMapping from a source, AuthorDto, to a destination, the author entity,
            //passing through the authorPropertyMapping dictionary,
            //which contains mappings between all properties from an AuthorDto to an author entity.
            //So we now have a list of IpropertyMapping we can search.
            _propertyMappings.Add(new PropertyMapping<AuthorDto, Author>(_authorPropertyMapping));
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            // the string is separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',');

            // run through the fields clauses
            foreach (var field in fieldsAfterSplit)
            {
                // trim
                var trimmedField = field.Trim();

                // remove everything after the first " " - if the fields 
                // are coming from an orderBy string, this part must be 
                // ignored
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                // find the matching property
                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }

        //Then, let's add that method to get one property mapping.
        //Through this, we'll be able to ask for a mapping from a source type to a destination type,
        //like a mapping from an authorDto to an author.
        //But we haven't got a class to hold such a property mapping yet, so let's add that.
        //We'll name this one PropertyMapping.
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping
           <TSource, TDestination>()
        {
            //First, we find the matching mapping by searching our list of IPropertyMapping for
            //propertyMappings from the passed-in source type to the passed in destination type.
            // get matching mapping
            var matchingMapping = _propertyMappings
                .OfType<PropertyMapping<TSource, TDestination>>();

            //If we find exactly one, we return a dictionary.
            //Otherwise, we throw an exception.
            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance " +
                $"for <{typeof(TSource)},{typeof(TDestination)}");
        }
    }
}
