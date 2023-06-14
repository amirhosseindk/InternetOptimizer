namespace Application.IServices
{
    public interface IIpAddressService
    {
        string GetLocalIpAddress();
        string GetPublicIpAddress();
        string GetDefaultGatewayIpAddress();
    }
}
