#pragma warning disable 0219
#pragma warning disable 0168
#pragma warning disable 0618
#pragma warning disable 0649
#pragma warning disable 0067
#pragma warning disable 0414

#pragma warning disable 0067
#pragma warning disable 0414


using System;
using System.Collections.Generic;

namespace PubNubAPI
{
    public class PNPushListProvisionsResult: PNResult
    {
        public List<string> Channels {get; set;}
    }

}