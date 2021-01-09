using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bom.Web.Lib.Infrastructure
{

    public static class Answer
    {
        public static ListAnswer<T> CreateList<T>(IEnumerable<T>? val)
        {
            if(val == null)
            {
                return new ListAnswer<T>();
            }
            var a = new ListAnswer<T>(val);
            return a;
        }

        public static Answer<T> Create<T>(T val)
        {
            var a = new Answer<T>(val);
            return a;
        }
    }

    /// <summary>
    /// Api-Answer class
    /// </summary>
    /// <typeparam name="T">Result type</typeparam>
    public class Answer<T>
    {

        // used for 2 reasons: 
        // - avoid problems unpredictable return types (which cause problems when generating NSWAG clients, see https://weblog.west-wind.com/posts/2020/Feb/24/Null-API-Responses-and-HTTP-204-Results-in-ASPNET-Core)
        // - allow extending answers with generic info

        public Answer(T? val)
        {
            this.Value = val;
        }
        public Answer()
        {
            this.Value = default(T);
        }

        public T? Value { get; }
    }

    /// <summary>
    /// Api-Answer class for collections
    /// </summary>
    /// <typeparam name="T">Result type in list</typeparam>
    public class ListAnswer<T>
    {

        // used for 2 reasons: 
        // - avoid problems unpredictable return types (which cause problems when generating NSWAG clients, see https://weblog.west-wind.com/posts/2020/Feb/24/Null-API-Responses-and-HTTP-204-Results-in-ASPNET-Core)
        // - allow extending answers with generic info

        public ListAnswer(IEnumerable<T> val)
        {
            this.Value = val;
        }
        public ListAnswer()
        {
            this.Value = new List<T>();
        }

        public IEnumerable<T> Value { get; }
    }

}