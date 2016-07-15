using System;
using System.Text;

namespace Neutrino.Client {
    public class UrlBuilderEx : UrlBuilderEx.IUrlBuilderExAddPath, UrlBuilderEx.IUrlBuilderExAddParameter {
        private int _step;
        private StringBuilder _url = new StringBuilder();

        private UrlBuilderEx() {
            
        }

        public static IUrlBuilderExAddPath StartWithBaseUri(Uri baseurl) {
            var b = new UrlBuilderEx();
            var url = baseurl.ToString();
            b._url.Append(url.Substring(0, url.Length - 1));
            return b;
        }

        public IUrlBuilderExAddPath AddPath(string path) {
            if (!path.StartsWith("/")) {
                _url.Append("/");
            }
            if (path.EndsWith("/")) {
                path = path.Substring(0, path.Length - 1);
            }
            _url.Append(path);
            return this;
        }

        public IUrlBuilderExAddParameter AddQueryParameter(string key, string value) {
            _step++;
            if (_step == 1) {
                _url.Append("?");
            }
            else if (_step > 1) {
                _url.Append("&");
            }
            _url.Append(key).Append("=").Append(value);
            return this;
        }

        public string Build() {
            return _url.ToString();
        }

        public interface IUrlBuilderExAddPath {
            IUrlBuilderExAddPath AddPath(string path);
            IUrlBuilderExAddParameter AddQueryParameter(string key, string value);
            string Build();
        }

        public interface IUrlBuilderExAddParameter {
            IUrlBuilderExAddParameter AddQueryParameter(string key, string value);
            string Build();
        }
    }
}