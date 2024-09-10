namespace Paynet.Challenge.DataContract.V1
{
    public class OperationInfo
    {
        public string ErrorCode { get; set; }

        public string Message { get; set; }
    
        public string Field { get; set; }

        public OperationInfo() { }

        public OperationInfo(string errorCode, string message)
        {
            this.ErrorCode = errorCode;
            this.Message = message;
        }
        
        public OperationInfo(string errorCode, string message, string field)
        {
            this.ErrorCode = errorCode;
            this.Message = message;
            this.Field = field;
        }
    }
}