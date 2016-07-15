using System;
using System.Threading.Tasks;

namespace Neutrino.Core.Util {
    public static class Try {
        public static async Task It(Action action, int times, int delayInMillis) {
            try {
                action.Invoke();
            }
            catch (Exception ex) when (times > 0) {
                await Task.Delay(delayInMillis);
                await It(action, --times, delayInMillis);
            }
        }
    }
}