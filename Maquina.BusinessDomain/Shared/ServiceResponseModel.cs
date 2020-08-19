using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;

namespace Maquina.BusinessDomain.Shared
{
    /// <summary>
    /// Wrapper around the response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceResponseModel<T>
    {
        public ServiceResponseModel()
        {

        }

        public ServiceResponseModel(string errorCode)
        {
            this.ErrorCode = errorCode;
        }

        public ServiceResponseModel(string errorCode, string errorDescription)
        {
            this.ErrorCode = errorCode;
            this.ErrorDescription = errorDescription;
        }

        public ServiceResponseModel(T data)
        {
            this.Data = data;
        }

        public string ErrorCode { get; set; }

        public string ErrorDescription { get; set; }

        public bool IsError
        {
            get
            {
                return !string.IsNullOrEmpty(ErrorCode) || !string.IsNullOrEmpty(ErrorDescription);
            }
        }

        public T Data { get; set; }
    }

    public class ServiceResponseModel
    {
        public ServiceResponseModel()
        {

        }

        public ServiceResponseModel(string errorCode)
        {
            this.ErrorCode = errorCode;
        }

        public ServiceResponseModel(string errorMessage, string errorDescription)
        {
            this.ErrorCode = errorMessage;
            this.ErrorDescription = errorDescription;
        }

        public string ErrorCode { get; set; }

        public string ErrorDescription { get; set; }

        public bool IsError
        {
            get
            {
                return !string.IsNullOrEmpty(ErrorCode) || !string.IsNullOrEmpty(ErrorDescription);
            }
        }
    }
}
