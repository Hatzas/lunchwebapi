using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Lunch.WebApi.Helpers.CustomException
{
    public class CustomValidationException : Exception
    {
        #region Constructor
        public CustomValidationException(string message)
            : base(message)
        {
            this.HttpResponseStatus = HttpStatusCode.InternalServerError;
        }

        public CustomValidationException(string message, HttpStatusCode httpResponseStatus)
            : base(message)
        {
            this.HttpResponseStatus = httpResponseStatus;
        }
        #endregion

        public HttpStatusCode HttpResponseStatus { get; set; }
    }
}