using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisExampleCore.Data;
using RedisExampleCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExampleCore.Controllers
{
    public class PersonController : Controller
    {

        private readonly Context _context;
        private readonly IDistributedCache distributedCache;

        public PersonController(Context context,IDistributedCache distributedCache) 
        {
            _context = context;
            this.distributedCache = distributedCache;
        }
        public IActionResult Index()
        {
            //var persons = _context.Persons.ToList();
            var persons = new List<Person>();
            if (string.IsNullOrEmpty(distributedCache.GetString("persons")))
            {
          
                persons = _context.Persons.ToList();

                persons = persons.Select(x =>
                {
                    x.data = DateTime.Now;
                    return x;
                }).ToList();
                var personsInString = JsonConvert.SerializeObject(persons);
                distributedCache.SetString("persons", personsInString);
            }
            else
            {
                var personsFromCache = distributedCache.GetString("persons");
                persons = JsonConvert.DeserializeObject<List<Person>>(personsFromCache);
            }
            return View(persons);
        }
        public IActionResult ClearCache()
        {
            return Ok();
        }
        public IActionResult New()
        {
            return View(new Person());
        }
        [HttpPost]
        public async Task<IActionResult> New(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var person = _context.Persons.FirstOrDefault(x => x.Id == id);
            return View(person);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Person person)
        {
            _context.Persons.Update(person);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var person = _context.Persons.FirstOrDefault(x => x.Id ==id);
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

    }
}
