using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueueSample
{
    [Serializable]
    public class Person
    {
        public string FirstName
        {
            get;
            set;
        }
        public string LastName
        {
            get; set;
        }
    }
}
