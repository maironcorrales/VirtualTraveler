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
    public class RemoveAllPushChannelsForDeviceBuilder
    {     
        private readonly RemoveAllPushChannelsForDeviceRequestBuilder pubBuilder;
        
        public RemoveAllPushChannelsForDeviceBuilder DeviceID (string deviceIdForPush){ 
            pubBuilder.DeviceId(deviceIdForPush);
            return this;
        }

        public RemoveAllPushChannelsForDeviceBuilder PushType(PNPushType pnPushType) {
            pubBuilder.PushType = pnPushType;
            return this;
        }
        public RemoveAllPushChannelsForDeviceBuilder QueryParam(Dictionary<string, string> queryParam){
            pubBuilder.QueryParam(queryParam);
            return this;
        }

        public RemoveAllPushChannelsForDeviceBuilder(PubNubUnity pn){
            pubBuilder = new RemoveAllPushChannelsForDeviceRequestBuilder(pn);
        }
        public void Async(Action<PNPushRemoveAllChannelsResult, PNStatus> callback)
        {
            pubBuilder.Async(callback);
        }
    }
}