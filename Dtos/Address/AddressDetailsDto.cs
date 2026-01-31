namespace Loja.Dtos.Address
{
    public record AddressDetailsDto
        (
            int Id,
            string Street,
            string City,
            string ZipCode,
            string Country
        );
}
