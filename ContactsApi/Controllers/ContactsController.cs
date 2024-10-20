using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

[Route("api/[controller]")]
[ApiController]
public class ContactsController : ControllerBase
{
    private const string FilePath = "contacts.json";

    // GET: api/contacts
    [HttpGet]
    public ActionResult<List<Contact>> GetContacts()
    {
        return Ok(ReadContactsFromFile());
    }

    // POST: api/contacts
    [HttpPost]
    public ActionResult<Contact> AddContact(Contact contact)
    {
        var contacts = ReadContactsFromFile();
        contact.Id = contacts.Any() ? contacts.Max(c => c.Id) + 1 : 1;
        contacts.Add(contact);
        WriteContactsToFile(contacts);
        return CreatedAtAction(nameof(GetContacts), new { id = contact.Id }, contact);
    }

    // PUT: api/contacts/{id}
    [HttpPut("{id}")]
    public IActionResult EditContact(int id, Contact updatedContact)
    {
        var contacts = ReadContactsFromFile();
        var existingContact = contacts.FirstOrDefault(c => c.Id == id);
        if (existingContact == null)
        {
            return NotFound();
        }

        // Update contact properties
        existingContact.FirstName = updatedContact.FirstName;
        existingContact.LastName = updatedContact.LastName;
        existingContact.Email = updatedContact.Email;

        WriteContactsToFile(contacts);
        return NoContent(); // HTTP 204: Successfully updated with no content to return
    }

    // DELETE: api/contacts/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteContact(int id)
    {
        var contacts = ReadContactsFromFile();
        var contact = contacts.FirstOrDefault(c => c.Id == id);
        if (contact == null)
        {
            return NotFound();
        }
        contacts.Remove(contact);
        WriteContactsToFile(contacts);
        return NoContent(); // HTTP 204: No Content
    }

    // Helper method to read contacts from JSON file
    private List<Contact> ReadContactsFromFile()
    {
        if (!System.IO.File.Exists(FilePath))
        {
            return new List<Contact>();
        }
        var json = System.IO.File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
    }

    // Helper method to write contacts to JSON file
    private void WriteContactsToFile(List<Contact> contacts)
    {
        var json = JsonSerializer.Serialize(contacts);
        System.IO.File.WriteAllText(FilePath, json);
    }
}

