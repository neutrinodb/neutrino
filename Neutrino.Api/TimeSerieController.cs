using System;
using System.Collections.Generic;
using System.Web.Http;
using Neutrino.Core;

namespace Neutrino.Api {
    public class TimeSerieController : ApiController {
        public List<Occurrence> Get(string id, DateTime start, DateTime end) {
            return new List<Occurrence>();
        }
    }
}