using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYSD.DDL.K3.BOS.Models
{
    public class ResponseMsg
    {
        public Result Result;
    }
    public class Result
    {
        public ResponseStatus ResponseStatus;
        public string Id;
        public string Number;
    }
    public class ResponseStatus
    {
        public int ErrorCode;
        public bool IsSuccess;
        public Errors[] Errors;
        public SuccessEntitys[] SuccessEntitys;
    }

    public class Errors
    {
        public string FieldName;
        public string Message;
        public int DIndex;
    }
    public class SuccessEntitys
    {
        public long Id;
        public string Number;
        public int DIndex;
    }
}
