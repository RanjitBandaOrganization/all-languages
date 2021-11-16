using System.Collections.Generic;

namespace CourseLibrary.API.Services
{
    //Now let's create an interface from this
    //so we can register it as an interface- implementing class on the container. 
    //Registering this on the container is done in the ConfigureServices method in the startup class.
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool ValidMappingExistsFor<TSource, TDestination>(string fields);
    }
}