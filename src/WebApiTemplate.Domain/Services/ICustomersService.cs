using WebApiTemplate.Domain.Models;

namespace WebApiTemplate.Domain.Services
{
    public interface ICustomersService
    {
        Customer UpdateCustomer(UpdateCustomer customer);
    }
}
