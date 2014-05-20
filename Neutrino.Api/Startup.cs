using System;
using System.Collections.Generic;
using System.Web.Http;
using Neutrino.Core;
using Owin;

namespace Neutrino.Api {
    public class Startup {
        public void Configuration(IAppBuilder app) {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("default", "api/{controller}");
            app.UseWebApi(config);
        }

        
    }
}