using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neutrino.Client.Sample {
    public class Program {
        public static void Main(string[] args) {
            Task.Run(async () => await Start()).Wait();
            Console.WriteLine("End");
        }

        public static async Task Start() {
            var list = new List<OccurrenceDecimal>();
            for (int i = 0; i < 10; i++) {
                list.Add(new OccurrenceDecimal(DateTime.Today.AddMinutes(5 * i), 10));
            }
            try {
                var nc = new NeutrinoClient(new Uri("http://localhost:34177/"));
                await nc.SaveAsync($"pontos/{100}/ene", list);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
