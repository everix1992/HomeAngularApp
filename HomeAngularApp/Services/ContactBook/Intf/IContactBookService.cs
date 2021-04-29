using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAngularApp.Services.ContactBook.Models;

namespace HomeAngularApp.Services.ContactBook.Intf
{
    public interface IContactBookService
    {
        Task<ContactViewModel> AddContactAsync(string firstName, string lastName, string emailAddress,
            string phoneNumber);

        Task DeleteContactAsync(int id);
        Task DeleteContactAsync(ContactViewModel contact);
        Task<IEnumerable<ContactViewModel>> GetAllContactsAsync();
        Task UpdateContactAsync(ContactViewModel contact);
    }
}