using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjConcept.Models
{
    public class ErrorDetails
    {
        public Exception Exception;
        public bool ExceptionLoaded;

        public ErrorDetails()
        {
            this.ExceptionLoaded = false;
            this.Exception = new Exception();
        }
    }
}