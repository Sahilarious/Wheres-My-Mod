using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WheresMyMod.Requests
{
	public class ModAddRequest
	{
	    public string Name { get; set; }
	    public string PageUrl { get; set; }
	    public string PicUrl { get; set; }
	    public int GameId { get; set; }
	}
}