using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Services
{
    public class PropertyMappingValue
    {
        //This one contains an IEnumerable of string.
        //Those are the destination properties 
        public IEnumerable<string> DestinationProperties { get; private set; }

        //And it also contains a Revert boolean, allowing us to revert the sort order if needed.
        public bool Revert { get; private set; }

        public PropertyMappingValue(IEnumerable<string> destinationProperties,
            bool revert = false)
        {
            DestinationProperties = destinationProperties
                ?? throw new ArgumentNullException(nameof(destinationProperties));
            Revert = revert;
        }
    }
}
