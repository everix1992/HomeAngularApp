using HomeAngularApp.DataAccess.Contacts.Intf;
using HomeAngularApp.DataAccess.Contacts.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HomeAngularApp.DataAccess.Contacts.Impl
{
    public class ContactRepository_FileBacked : IContactRepository
    {
        // Needs to be instantiated as a singleton

        // TODO: There are all kinds of concurrency problems here. Replace with a real DB
        private readonly string BackingFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ContactBook.json");
        private ConcurrentDictionary<int, ContactDto> ContactsDatabase = null;
        private static readonly SemaphoreSlim AddOrUpdateRecordLock = new SemaphoreSlim(1, 1);
        private static readonly SemaphoreSlim ReadLock = new SemaphoreSlim(1, 1);

        public async Task<ContactDto> AddContactAsync(string firstName, string lastName, string emailAddress, string phoneNumber)
        {
            await EnsureDatabaseInMemory();
            var receivedLock = await AddOrUpdateRecordLock.WaitAsync(TimeSpan.FromSeconds(10));

            if (!receivedLock)
            {
                // TODO: Better exception
                throw new TimeoutException();
            }

            try
            {
                var newRecord = new ContactDto
                {
                    Id = ContactsDatabase.Count == 0 ? 0 : ContactsDatabase.Keys.Max() + 1,
                    FirstName = firstName,
                    LastName = lastName,
                    EmailAddress = emailAddress,
                    PhoneNumber = phoneNumber
                };

                ContactsDatabase[newRecord.Id] = newRecord;
                await File.WriteAllTextAsync(BackingFilePath, JsonSerializer.Serialize(ContactsDatabase));

                return newRecord;
            }
            finally
            {
                AddOrUpdateRecordLock.Release();
            }
        }

        public async Task DeleteContactAsync(int id)
        {
            await EnsureDatabaseInMemory();
            var receivedLock = await AddOrUpdateRecordLock.WaitAsync(TimeSpan.FromSeconds(10));

            if (!receivedLock)
            {
                // TODO: Better exception
                throw new TimeoutException();
            }

            try
            {
                ContactsDatabase.Remove(id, out var _);
                await File.WriteAllTextAsync(BackingFilePath, JsonSerializer.Serialize(ContactsDatabase));
            }
            finally
            {
                AddOrUpdateRecordLock.Release();
            }
        }

        public async Task DeleteContactAsync(ContactDto contact)
        {
            await EnsureDatabaseInMemory();
            await DeleteContactAsync(contact.Id);
        }

        public async Task<IEnumerable<ContactDto>> GetAllContactsAsync()
        {
            await EnsureDatabaseInMemory();
            return ContactsDatabase.Values.OrderBy(c => c.Id);
        }

        public async Task UpdateContactAsync(ContactDto contact)
        {
            await EnsureDatabaseInMemory();
            var receivedLock = await AddOrUpdateRecordLock.WaitAsync(TimeSpan.FromSeconds(10));

            if (!receivedLock)
            {
                // TODO: Better exception
                throw new TimeoutException();
            }

            try
            {

                ContactsDatabase[contact.Id] = contact;
                await File.WriteAllTextAsync(BackingFilePath, JsonSerializer.Serialize(ContactsDatabase));
            }
            finally
            {
                AddOrUpdateRecordLock.Release();
            }
        }

        private async Task EnsureDatabaseInMemory()
        {
            if (ContactsDatabase == null)
            {
                var receivedLock = await ReadLock.WaitAsync(TimeSpan.FromSeconds(10));

                if (!receivedLock)
                {
                    // TODO: Better exception
                    throw new TimeoutException();
                }

                try
                {
                    if (File.Exists(BackingFilePath))
                    {
                        ContactsDatabase =
                            JsonSerializer.Deserialize<ConcurrentDictionary<int, ContactDto>>(
                                await File.ReadAllTextAsync(BackingFilePath));
                    }
                    else
                    {
                        ContactsDatabase = new ConcurrentDictionary<int, ContactDto>();
                        Directory.CreateDirectory(Path.GetDirectoryName(BackingFilePath));
                        await File.WriteAllTextAsync(BackingFilePath, JsonSerializer.Serialize(ContactsDatabase));
                    }
                }
                finally
                {
                    ReadLock.Release();
                }
            }
        }
    }
}