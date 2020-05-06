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
    public class GetMembersBuilder
    {     
        private readonly GetMembersRequestBuilder getMembersBuilder;
        
        public GetMembersBuilder(PubNubUnity pn){
            getMembersBuilder = new GetMembersRequestBuilder(pn);
        }
        public GetMembersBuilder Include(PNMembersInclude[] include){
            getMembersBuilder.Include(include);
            return this;
        }

        public GetMembersBuilder SpaceID(string id){
            getMembersBuilder.SpaceID(id);
            return this;
        }
        public GetMembersBuilder Limit(int limit){
            getMembersBuilder.Limit(limit);
            return this;
        }

        public GetMembersBuilder Start(string start){
            getMembersBuilder.Start(start);
            return this;
        }
        public GetMembersBuilder End(string end){
            getMembersBuilder.End(end);
            return this;
        }
        public GetMembersBuilder Count(bool count){
            getMembersBuilder.Count(count);
            return this;
        }
        public GetMembersBuilder QueryParam(Dictionary<string, string> queryParam){
            getMembersBuilder.QueryParam(queryParam);
            return this;
        }

        public void Async(Action<PNMembersResult, PNStatus> callback)
        {
            getMembersBuilder.Async(callback);
        }
    }
}