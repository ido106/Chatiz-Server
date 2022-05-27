﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly WebAppContext _context;

        public UserService(WebAppContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<User> Get(string username)
        {
            return  await _context.User.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async void Add(User user)
        {
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exist(string username)
        {
           var q = await _context.User.FirstOrDefaultAsync(x => x.Username.Equals(username));
           return q != null;
        }

        public async Task<List<Contact>> GetContacts(string username)
        {
            User user = await Get(username);
            if (user == null) return null;
            return user.Contacts;
        }

        public async Task<Contact> GetContact(string username, string contact_name)
        {
            List<Contact> all_contacts = await GetContacts(username);
            if (all_contacts == null) return null;
            Contact contact = all_contacts.FirstOrDefault(x => x.ContactUsername == contact_name);
            return contact;
        }

        public async Task<bool> AddContact(string username, string contact_name)
        {
            User user = await Get(username);
            if(user == null) return false;

            if(await Get(contact_name) == null) return false;

            Contact contact = new (contact_name);

            user.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteContact(string username, string contact_name)
        {
            User user = await Get(username);
            if(user == null || username.Equals(contact_name)) return false;
            Contact contact = await GetContact(username, contact_name);
            if(contact == null) return false;
            user.Contacts.Remove(contact);

            await _context.SaveChangesAsync();
            return true;
        }

    
    }
}
