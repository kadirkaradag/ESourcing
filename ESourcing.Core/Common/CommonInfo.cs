using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESourcing.Core.Common
{
    public static class CommonInfo
    {
        public static string BaseAddress = "http://localhost:5000"; // api gateway üzerinden istek atıcaz. dockerda api gateway 5000 portu oldugu icin 5000 üzrinden baglanıyoruz.
        //public static string LocalAuctionBaseAddress = "http://localhost:17962";
        //public static string LocalProductBaseAddress = "http://localhost:44739";
        public static string LocalAuctionBaseAddress = "http://localhost:8001";
        public static string LocalProductBaseAddress = "http://localhost:8000";
        public static string LocalOrderBaseAddress = "http://localhost:60834";



    }
}
