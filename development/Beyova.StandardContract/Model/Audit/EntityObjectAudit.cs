using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityObjectAudit : BaseAuditObject<Guid?, JToken>, IIdentifier
    {
    }
}