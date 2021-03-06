﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MJpegCameraProxy.Pages.Admin
{
	class Main : AdminBase
	{
		protected override string GetPageHtml(SimpleHttp.HttpProcessor p, Session s)
		{
			return @"
	<script type=""text/javascript"">
	var camDefs = " + MJpegServer.cm.GenerateAllCameraIdNameList(s == null ? 0 : s.permission) + @";
	var remoteIp = '" + p.RemoteIPAddress + @"';
	var refreshDisabled = false;
	var refreshTime = 2500;
	$(function()
	{
		setTimeout(""disableRefresh()"", 300000);
		var isLocal = remoteIp.indexOf(""192.168.0."") > -1;
		for(var i = 0; i < camDefs.length; i++)
		{
			var myid = camDefs[i][0];
			$(""#maindiv"").append('<div class=""outer""><a href=""../image/' + myid + '.cam""><table><tbody><tr><td class=""imgbox""><img id=""' + myid + '"" alt=""' + camDefs[i][1] + '""></td></tr></tbody></table><div class=""caption"">' + camDefs[i][1] + '</div></a></div>');
			if(isLocal)
				refreshTime = 250;
			$(""#"" + myid).load(function ()
			{
				if(!refreshDisabled)
				{
					lastUpdate = new Date().getTime();
					setTimeout(""GetNewImage('"" + this.id + ""');"", refreshTime);
				}
			});
			$(""#"" + myid).error(function ()
			{
				setTimeout(""GetNewImage('"" + this.id + ""');"", refreshTime);
			});
			GetNewImage(myid);
		}
	});
	function GetNewImage(id)
	{
		$(""#"" + id).attr('src', '../image/' + id + '.jpg?maxwidth=320&maxheight=240&nocache=' + new Date().getTime());
	}
	function disableRefresh()
	{
		$(""#maindiv"").prepend('<div style=""color: Red;"">Camera Updating Stopped to conserve resources</div>');
		refreshDisabled = true;
	}
	function PopupMessage(msg)
	{
		var pm = $(""#popupMessage"");
		if(pm.length < 1)
			$(""#maindiv"").after('<div id=""popupFrame""><div id=""popupMessage"">' + msg + '</div><center><input type=""button"" value=""Close Message"" onclick=""CloseMessage()""/></center></div>');
		else
			pm.append(msg);
	}
	function CloseMessage()
	{
		$(""#popupFrame"").remove();
	}
	</script>
	<style type=""text/css"">
	img
	{
		max-width: 320px;
		max-height: 240px;
		vertical-align: middle;
	}
	.caption
	{
		text-align: center;
	}
	a
	{
		text-decoration: none;
		color: Black;
	}
	.imgbox
	{
		width: 320px;
		height: 240px;
	}
	.outer
	{
		display: inline-block;
		padding-right: 20px;
		padding-bottom: 20px;
	}
	</style>
	<div id=""maindiv""></div>";
		}
	}
}
