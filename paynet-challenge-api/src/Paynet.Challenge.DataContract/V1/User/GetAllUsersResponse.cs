namespace Paynet.Challenge.DataContract.V1.User
{
    public class GetAllUsersResponse : OperationResponse
    {
        public List<UserDto> Users { get; set; }
    }
}