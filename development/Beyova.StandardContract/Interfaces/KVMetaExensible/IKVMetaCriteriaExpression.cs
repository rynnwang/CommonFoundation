using Beyova.BooleanSearch;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public interface IKVMetaCriteriaExpression : IExpression<string, JValue, string>, IBooleanComputable
    {
    }
}