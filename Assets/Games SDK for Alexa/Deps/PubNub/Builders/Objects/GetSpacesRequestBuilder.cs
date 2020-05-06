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
using System.Linq;

namespace PubNubAPI
{
    public class GetSpacesRequestBuilder : PubNubNonSubBuilder<GetSpacesRequestBuilder, PNGetSpacesResult>, IPubNubNonSubscribeBuilder<GetSpacesRequestBuilder, PNGetSpacesResult>
    {
        private int GetSpacesLimit { get; set; }
        private string GetSpacesEnd { get; set; }
        private string GetSpacesStart { get; set; }
        private bool GetSpacesCount { get; set; }
        private PNUserSpaceInclude[] GetSpacesInclude { get; set; }
        public GetSpacesRequestBuilder(PubNubUnity pn) : base(pn, PNOperationType.PNGetSpacesOperation)
        {
        }

        #region IPubNubBuilder implementation
        public void Async(Action<PNGetSpacesResult, PNStatus> callback)
        {
            this.Callback = callback;
            base.Async(this);
        }
        #endregion

        public GetSpacesRequestBuilder Include(PNUserSpaceInclude[] include)
        {
            GetSpacesInclude = include;
            return this;
        }
        public GetSpacesRequestBuilder Limit(int limit)
        {
            GetSpacesLimit = limit;
            return this;
        }

        public GetSpacesRequestBuilder Start(string start)
        {
            GetSpacesStart = start;
            return this;
        }
        public GetSpacesRequestBuilder End(string end)
        {
            GetSpacesEnd = end;
            return this;
        }
        public GetSpacesRequestBuilder Count(bool count)
        {
            GetSpacesCount = count;
            return this;
        }
        protected override void RunWebRequest(QueueManager qm)
        {
            RequestState requestState = new RequestState();
            requestState.OperationType = OperationType;

            string[] includeString = (GetSpacesInclude==null) ? new string[]{} : GetSpacesInclude.Select(a=>a.GetDescription().ToString()).ToArray();

            Uri request = BuildRequests.BuildObjectsGetSpacesRequest(
                    GetSpacesLimit,
                    GetSpacesStart,
                    GetSpacesEnd,
                    GetSpacesCount,
                    string.Join(",", includeString),
                    this.PubNubInstance,
                    this.QueryParams
                );
            base.RunWebRequest(qm, request, requestState, this.PubNubInstance.PNConfig.NonSubscribeTimeout, 0, this);
        }

        protected override void CreatePubNubResponse(object deSerializedResult, RequestState requestState)
        {
            PNGetSpacesResult pnSpaceResultList = new PNGetSpacesResult();
            pnSpaceResultList.Data = new List<PNSpaceResult>();
            PNStatus pnStatus = new PNStatus();

            try
            {
                Dictionary<string, object> dictionary = deSerializedResult as Dictionary<string, object>;

                if (dictionary != null)
                {
                    object objData;
                    dictionary.TryGetValue("data", out objData);
                    if (objData != null)
                    {
                        object[] objArr = objData as object[];
                        foreach (object data in objArr)
                        {
                            Dictionary<string, object> objDataDict = data as Dictionary<string, object>;
                            if (objDataDict != null)
                            {
                                PNSpaceResult pnSpaceResult = ObjectsHelpers.ExtractSpace(objDataDict);
                                pnSpaceResultList.Data.Add(pnSpaceResult);
                            }
                            else
                            {
                                pnStatus = base.CreateErrorResponseFromException(new PubNubException("objDataDict null"), requestState, PNStatusCategory.PNUnknownCategory);
                            }
                        }
                    }
                    else
                    {
                        pnSpaceResultList = null;
                        pnStatus = base.CreateErrorResponseFromException(new PubNubException("objData null"), requestState, PNStatusCategory.PNUnknownCategory);
                    }
                }
            }
            catch (Exception ex)
            {
                pnSpaceResultList = null;
                pnStatus = base.CreateErrorResponseFromException(ex, requestState, PNStatusCategory.PNUnknownCategory);
            }
            Callback(pnSpaceResultList, pnStatus);

        }

    }

}