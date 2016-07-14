using System;

namespace Neutrino.Tests {
    public static class TestUtil {
        public static int OneHour = 1000*60*60;
        public static DateTime Yesterday = DateTime.Today.AddDays(-1);
        public static DateTime Today = DateTime.Today;
    }
}