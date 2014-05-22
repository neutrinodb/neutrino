using System;
using System.Threading.Tasks;

namespace Neutrino.Core {
    public interface ITimeSerieService {
        Task<string> Create(TimeSerieInfo timeSerieInfo);
        Task<TimeSerie> List(string id, DateTime start, DateTime end);
        Task Add(string id, Occurrence occurrence);
    }
}