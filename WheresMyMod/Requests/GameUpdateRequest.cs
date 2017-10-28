using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WheresMyMod.Requests
{
    public class GameUpdateRequest : GameAddRequest
    {
        public int Id { get; set; }
    }
}