using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Neutrino.Api.Controllers {
    [Route("api/[controller]")]
    public class ValuesController : Controller {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(string id) {
            return "int " + id;
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
