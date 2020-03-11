using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Services
{
    public class PropertyMappingValue
    {
        /// <summary>
        /// FirstName+LastName
        /// </summary>
        public IEnumerable<string> DestinationProperties { get; set; }
        /// <summary>
        /// 反转
        /// </summary>
        public bool Revert { get; set; }
        public PropertyMappingValue(IEnumerable<string> destinationProperties, bool revert = false)
        {
            DestinationProperties = destinationProperties ?? throw new ArgumentNullException(nameof(destinationProperties));
            Revert = revert;
        }
    }
}
