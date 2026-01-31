using System.ComponentModel.DataAnnotations;

namespace Loja.Dtos.Address
{
    public record CreateAddressDto
        (
            [Required][StringLength(100, MinimumLength = 3)] string Street,
            [Required][StringLength(50, MinimumLength = 3)] string City,
            [Required][StringLength(20, MinimumLength = 3)] string ZipCode,
            [Required][StringLength(50, MinimumLength = 2)] string Country,
            bool IsDefault
        );
}
