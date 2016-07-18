using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Neutrino {
    public interface ITimeSerieService {
        Task<string> Create(TimeSerieHeader timeSerieHeader);
        Task<TimeSerieHeader> Load(string id);
        Task<TimeSerie> List(string id, DateTime start, DateTime end);
        Task Save(string id, Occurrence occurrence);
        Task Save(string id, List<Occurrence> occurrences);
    }
}