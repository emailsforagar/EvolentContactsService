using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Evolent.ContactsService.Models;
using System.Net;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Evolent.ContactsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        #region Member varaible
        private readonly dbsegapracdevContext _context;
        private readonly ILogger _logger;
        #endregion

        #region Constructor
        public ContactsController(dbsegapracdevContext context, ILogger<ContactsController> logger)
        {
            _context = context;
            _logger = logger; 
        }
        #endregion

        #region Operations
        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contacts>>> GetContacts()
        {
            _logger.LogTrace("Entered" + MethodBase.GetCurrentMethod());
            try
            {

                return await _context.Contacts.ToListAsync();
            }
            catch(Exception ex)
            {
               _logger.LogError(ex.ToString());
                throw;
            }
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contacts>> GetContacts(int id)
        {
            _logger.LogTrace("Entered" + MethodBase.GetCurrentMethod());
            try
            {
                var contacts = await _context.Contacts.FindAsync(id);

                if (contacts == null)
                {
                    return NotFound();
                }

                return contacts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContacts(int id, Contacts contacts)
        {
            _logger.LogTrace("Entered" + MethodBase.GetCurrentMethod());
            if (id != contacts.ContactId && !Enum.IsDefined(typeof(Status), contacts.Status))
            {
                return BadRequest();
            }

            _context.Entry(contacts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ContactsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex.ToString());
                }
            }

            return NoContent();
        }

        // POST: api/Contacts
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Contacts>> PostContacts(Contacts contacts)
        {
            _logger.LogTrace("Entered" + MethodBase.GetCurrentMethod());
            try
            {

                if (Enum.IsDefined(typeof(Status), contacts.Status))
                {
                    _context.Contacts.Add(contacts);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetContacts", new { id = contacts.ContactId }, contacts);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Contacts>> DeleteContacts(int id)
        {
            _logger.LogTrace("Entered" + MethodBase.GetCurrentMethod());
            try
            {
                var contacts = await _context.Contacts.FindAsync(id);
                if (contacts == null)
                {
                    return NotFound();
                }

                _context.Contacts.Remove(contacts);
                await _context.SaveChangesAsync();

                return contacts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
        #endregion

        private bool ContactsExists(int id)
        {
            return _context.Contacts.Any(e => e.ContactId == id);
        }
        enum Status
        {
            Inactive,
            Active            
        }
    }
}
