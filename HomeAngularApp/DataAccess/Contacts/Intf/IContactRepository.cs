using HomeAngularApp.DataAccess.Contacts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeAngularApp.DataAccess.Contacts.Intf
{
    public interface IContactRepository
    {
        Task<IEnumerable<ContactDto>> GetAllContactsAsync();
        Task<ContactDto> AddContactAsync(string firstName, string lastName, string emailAddress, string phoneNumber);
        Task DeleteContactAsync(int id);
        Task DeleteContactAsync(ContactDto contact);
        Task UpdateContactAsync(ContactDto contact);
    }
}
