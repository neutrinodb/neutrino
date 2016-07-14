using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neutrino;

namespace Neutrino.Api.Controllers {
    [Route("api/[controller]")]
    public class ValuesController : Controller {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public List<object> Get(string id) {

            var list = new List<object>();
            list.Add(new Occurrence(DateTime.Today, 10.1m));
            list.Add(new Occurrence(DateTime.Today, 11.0m));
            list.Add(new Occurrence(DateTime.Today, 12.0m));
            return list;
        }

        //[HttpGet("{id}")]
        //public string Get(double id) {
        //    return "double " + id;
        //}

        //[HttpGet("{id}")]
        //public string Get(float id) {
        //    return "float " + id;
        //}

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value) {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}
