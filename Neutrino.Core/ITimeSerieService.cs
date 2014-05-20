using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neutrino.Core {
    public interface ITimeSerieService {
        Task<string> Create(TimeSerie timeSerie);
        Task<List<Occurrence>> List(string id, DateTime start, DateTime end);
    }
}