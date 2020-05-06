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
    public class GetMembershipsBuilder
    {     
        private readonly GetMembershipsRequestBuilder getMembershipsBuilder;
        
        public GetMembershipsBuilder(PubNubUnity pn){
            getMembershipsBuilder = new GetMembershipsRequestBuilder(pn);
        }
        public GetMembershipsBuilder Include(PNMembershipsInclude[] include){
            getMembershipsBuilder.Include(include);
            return this;
        }

        public GetMembershipsBuilder UserID(string id){
            getMembershipsBuilder.UserID(id);
            return this;
        }

        public GetMembershipsBuilder Limit(int limit){
            getMembershipsBuilder.Limit(limit);
            return this;
        }

        public GetMembershipsBuilder Start(string start){
            getMembershipsBuilder.Start(start);
            return this;
        }
        public GetMembershipsBuilder End(string end){
            getMembershipsBuilder.End(end);
            return this;
        }
        public GetMembershipsBuilder Count(bool count){
            getMembershipsBuilder.Count(count);
            return this;
        }
        public GetMembershipsBuilder QueryParam(Dictionary<string, string> queryParam){
            getMembershipsBuilder.QueryParam(queryParam);
            return this;
        }

        public void Async(Action<PNMembershipsResult, PNStatus> callback)
        {
            getMembershipsBuilder.Async(callback);
        }
    }
}