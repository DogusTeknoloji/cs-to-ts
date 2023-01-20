using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace CsToTs.Tests.Fixture
{
    public class EntityWithMoreTypes
    {
        public DateTimeOffset DateTimeOffset { get; set; }
        public JObject JObject { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public Guid Guid { get; set; }
    }
}
