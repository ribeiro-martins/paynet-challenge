namespace Paynet.Challenge.DataContract.V1
{
    public class OperationResponse
    {
        public bool Success => this.Errors?.Any() == false;

        public List<OperationInfo> Errors { get; set; }

        public string TraceKey { get; } = Guid.NewGuid().ToString();

        public OperationResponse()
        {
            this.Errors = new List<OperationInfo>();
        }

        public void AddError(string errorCode, string message, string field)
        {
            this.Errors.Add(new OperationInfo(errorCode, message, field));
        }

        public void AddError(string errorCode, string message)
        {
            this.Errors.Add(new OperationInfo(errorCode, message));
        }
    }
}