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
    public class UnsubscribeBuilder
    {     
        private readonly LeaveRequestBuilder pubBuilder;

        public UnsubscribeBuilder Channels(List<string> channelNames){
            pubBuilder.Channels(channelNames);
            return this;
        }

        public UnsubscribeBuilder ChannelGroups(List<string> channelGroupNames){
            pubBuilder.ChannelGroups(channelGroupNames);
            return this;
        }
        public UnsubscribeBuilder QueryParam(Dictionary<string, string> queryParam){
            pubBuilder.QueryParam(queryParam);
            return this;
        }
        
        public UnsubscribeBuilder(PubNubUnity pn){
            pubBuilder = new LeaveRequestBuilder(pn);
        }
        public void Async(Action<PNLeaveRequestResult, PNStatus> callback)
        {
            pubBuilder.Async(callback);
        }
    }
}