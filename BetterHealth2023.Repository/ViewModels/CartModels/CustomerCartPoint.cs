using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.CartModels
{
    [FirestoreData]
    public class CustomerCartPoint
    {
        public string CartId { get; set; }

        [FirestoreProperty("point")]
        public int UsingPoint { get; set; }
    }
}
