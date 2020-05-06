#pragma warning disable 0219
#pragma warning disable 0168
#pragma warning disable 0618
#pragma warning disable 0649
#pragma warning disable 0067
#pragma warning disable 0414

#pragma warning disable 0067
#pragma warning disable 0414

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace PubNubAPI
{
    public class WhereNowBuilder
    {     
        private readonly WhereNowRequestBuilder pubBuilder;
        
        public WhereNowBuilder(PubNubUnity pn){
            pubBuilder = new WhereNowRequestBuilder(pn);
        }
        public WhereNowBuilder Uuid(string uuidForWhereNow){
            pubBuilder.Uuid(uuidForWhereNow);
            return this;
        }

        public WhereNowBuilder QueryParam(Dictionary<string, string> queryParam){
            pubBuilder.QueryParam(queryParam);
            return this;
        }

        public void Async(Action<PNWhereNowResult, PNStatus> callback)
        {
            pubBuilder.Async(callback);
        }
    }
}