using System;
using HomeAngularApp.Services.ContactBook.Intf;
using HomeAngularApp.Services.ContactBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeAngularApp.Controllers
{
    [ApiController]
    [Route("api/v1/contacts")]
    public class ContactsController
    {
        private readonly IContactBookService _contactBookService;

        public ContactsController(IContactBookService contactBookService)
        {
            _contactBookService = contactBookService;
        }

        [HttpGet("")]
        public async Task<IEnumerable<ContactViewModel>> GetAllContacts()
        {
            var contacts = await _contactBookService.GetAllContactsAsync();
            return contacts;
        }

        [HttpGet("{id:int}")]
        public async Task<ContactViewModel> GetContact(int id)
        {
            // TODO
            throw new NotImplementedException();
        }

        [HttpPost("")]
        public async Task<ContactViewModel> AddContact(ContactViewModel contact)
        {
            // TODO: Probably shouldn't be using ContactViewModel above
            return await _contactBookService.AddContactAsync(contact.FirstName, contact.LastName, contact.EmailAddress,
                contact.PhoneNumber);
        }

        [HttpPut("{id:int}")]
        public async Task UpdateContact(int id, ContactViewModel contact)
        {
            // TODO: Probably shouldn't be using ContactViewModel above
            contact.Id = id;
            await _contactBookService.UpdateContactAsync(contact);
        }

        [HttpDelete("{id:int}")]
        public async Task DeleteContact(int id)
        {
            await _contactBookService.DeleteContactAsync(id);
        }
    }
}
