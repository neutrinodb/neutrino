using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neutrino.Data;

namespace Neutrino.Api {

    [Route("api/[controller]")]
    public class OcurrencesController  {
        //private ITimeSerieService<object> _service;
        //private ITimeSerieHeaderService _timeSerieHeaderService;

        //public OcurrencesController(ITimeSerieHeaderService timeSerieHeaderService, ITimeSerieService<object> service) {
        //    _timeSerieHeaderService = timeSerieHeaderService;
        //    _service = service;
        //}

        //[HttpGet]
        //public async Task<string> Get(string id, DateTime start, DateTime end) {
        //    var header = await _timeSerieHeaderService.Get(id);

        //    return await _service.List(id, start, end);
        //}

        //[HttpPost]
        //[Route("{*id}")]
        //public async Task Post(string id, [FromBody] string ocurrences) {
        //    var header = await _timeSerieHeaderService.Get(id);
        //    switch(header.OcurrenceType) {
        //        case OcurrenceKind.Decimal:

        //    }

        //    await _service.Add(id, ocurrences);
        //}

        //private ITimeSerieService<object> GetService(OcurrenceKind ocurrenceKind) {
        //    return new TimeSerieService<decimal>(new FileFinder(""), new FileStreamOpener());
        //}
    }
}