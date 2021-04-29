using HomeAngularApp.DataAccess.Contacts.Intf;
using HomeAngularApp.DataAccess.Contacts.Models;
using HomeAngularApp.Services.ContactBook.Intf;
using HomeAngularApp.Services.ContactBook.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAngularApp.Services.ContactBook
{
    public class ContactBookService : IContactBookService
    {
        private readonly IContactRepository _contactRepo;

        public ContactBookService(IContactRepository contactRepo)
        {
            _contactRepo = contactRepo;
        }

        public async Task<ContactViewModel> AddContactAsync(string firstName, string lastName, string emailAddress,
            string phoneNumber)
        {
            var dto = await _contactRepo.AddContactAsync(firstName, lastName, emailAddress, phoneNumber);
            return MapDtoToViewModel(dto);
        }

        public async Task DeleteContactAsync(int id)
        {
            await _contactRepo.DeleteContactAsync(id);
        }

        public async Task DeleteContactAsync(ContactViewModel contact)
        {
            await DeleteContactAsync(contact.Id);
        }

        public async Task<IEnumerable<ContactViewModel>> GetAllContactsAsync()
        {
            return (await _contactRepo.GetAllContactsAsync()).Select(MapDtoToViewModel);
        }

        public async Task UpdateContactAsync(ContactViewModel contact)
        {
            await _contactRepo.UpdateContactAsync(MapViewModelToDto(contact));
        }

        private static ContactViewModel MapDtoToViewModel(ContactDto contact)
        {
            return new ContactViewModel
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                EmailAddress = contact.EmailAddress,
                PhoneNumber = contact.PhoneNumber
            };
        }

        private static ContactDto MapViewModelToDto(ContactViewModel contact)
        {
            return new ContactDto
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                EmailAddress = contact.EmailAddress,
                PhoneNumber = contact.PhoneNumber
            };
        }
    }
}
