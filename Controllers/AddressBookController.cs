using Bogus;
using Bogus.DataSets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace otelSample.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class AddressBookController : ControllerBase
   {
      private static Dictionary<int, AddressBook> addressBookDb = new Dictionary<int, AddressBook>() { };
      private readonly ILogger<AddressBookController> _logger;

      public AddressBookController(ILogger<AddressBookController> logger)
      {
         _logger = logger;
      }

      [HttpGet(Name = "GetAddressBooks")]
      public IEnumerable<AddressBook> Get()
      {
         _logger.LogInformation("Address Booklog");
         return addressBookDb.Values.ToArray();
      }

      [HttpPost(Name = "CreateAddresBook")]
      public async Task<IActionResult> Post()
      {
         var faker = new Faker();
         _logger.LogInformation("Create Address book entry");
         addressBookDb.Add(faker.Random.Int(1), new AddressBook
         {
            Id = faker.Random.Int(1),
            Phone = faker.Phone.PhoneNumber(),
            Email = faker.Internet.Email(),
            ContantName = faker.Name.FullName()
         });

         return Created("GetAddressBooks", addressBookDb.Values.ToArray());
      }

      public class AddressBook
      {
         public int Id { get; set; }
         public string ContantName { get; set; }
         public string Phone { get; set; }

         public string Email { get; set; }

         static Dictionary<int, AddressBook> data;
         
      }

   }

  
}


