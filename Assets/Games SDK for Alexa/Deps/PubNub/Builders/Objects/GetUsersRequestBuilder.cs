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
    public class GetUsersRequestBuilder: PubNubNonSubBuilder<GetUsersRequestBuilder, PNGetUsersResult>, IPubNubNonSubscribeBuilder<GetUsersRequestBuilder, PNGetUsersResult>
    {        
        private int GetUsersLimit { get; set;}
        private string GetUsersEnd { get; set;}
        private string GetUsersStart { get; set;}
        private bool GetUsersCount { get; set;}
        private PNUserSpaceInclude[] GetUsersInclude { get; set;}
        
        public GetUsersRequestBuilder(PubNubUnity pn): base(pn, PNOperationType.PNGetUsersOperation){
        }

        #region IPubNubBuilder implementation
        public void Async(Action<PNGetUsersResult, PNStatus> callback)
        {
            this.Callback = callback;
            base.Async(this);
        }
        #endregion

        public GetUsersRequestBuilder Include(PNUserSpaceInclude[] include){
            GetUsersInclude = include;
            return this;
        }
        public GetUsersRequestBuilder Limit(int limit){
            GetUsersLimit = limit;
            return this;
        }

        public GetUsersRequestBuilder Start(string start){
            GetUsersStart = start;
            return this;
        }
        public GetUsersRequestBuilder End(string end){
            GetUsersEnd = end;
            return this;
        }
        public GetUsersRequestBuilder Count(bool count){
            GetUsersCount = count;
            return this;
        }
        protected override void RunWebRequest(QueueManager qm){
            RequestState requestState = new RequestState ();
            requestState.OperationType = OperationType;

            string[] includeString = (GetUsersInclude==null) ? new string[]{} : GetUsersInclude.Select(a=>a.GetDescription().ToString()).ToArray();      

            Uri request = BuildRequests.BuildObjectsGetUsersRequest(
                    GetUsersLimit,
                    GetUsersStart,
                    GetUsersEnd,
                    GetUsersCount,
                    string.Join(",", includeString),
                    this.PubNubInstance,
                    this.QueryParams
                );
            base.RunWebRequest(qm, request, requestState, this.PubNubInstance.PNConfig.NonSubscribeTimeout, 0, this); 
        }

        protected override void CreatePubNubResponse(object deSerializedResult, RequestState requestState){
            PNGetUsersResult pnUserResultList = new PNGetUsersResult();
            pnUserResultList.Data = new List<PNUserResult>();
            PNStatus pnStatus = new PNStatus();

            try{
                Dictionary<string, object> dictionary = deSerializedResult as Dictionary<string, object>;
                
                if(dictionary != null) {
                    object objData;
                    dictionary.TryGetValue("data", out objData);
                    if(objData!=null){
                        object[] objArr = objData as object[];
                        foreach (object data in objArr){
                            Dictionary<string, object> objDataDict = data as Dictionary<string, object>;
                            if(objDataDict!=null){
                                PNUserResult pnUserResult = ObjectsHelpers.ExtractUser(objDataDict);                                
                                pnUserResultList.Data.Add(pnUserResult);
                            }  else {
                                pnStatus = base.CreateErrorResponseFromException(new PubNubException("objDataDict null"), requestState, PNStatusCategory.PNUnknownCategory);
                            }  
                        }
                    }  else {
                        pnUserResultList = null;
                        pnStatus = base.CreateErrorResponseFromException(new PubNubException("objData null"), requestState, PNStatusCategory.PNUnknownCategory);
                    }  
                }
            } catch (Exception ex){
                pnUserResultList = null;
                pnStatus = base.CreateErrorResponseFromException(ex, requestState, PNStatusCategory.PNUnknownCategory);
            }
            Callback(pnUserResultList, pnStatus);

        }

    }
}