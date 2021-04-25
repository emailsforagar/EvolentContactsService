using System;
using System.Collections.Generic;

namespace Evolent.ContactsService.Models
{
    public partial class Contacts
    {
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        public string Status { get; set; }
    }
}
