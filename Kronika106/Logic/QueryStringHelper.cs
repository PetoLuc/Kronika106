using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kronika106.Logic
{
    public static class QueryStringHelper
    {

        public const string KeyID = "ID";
        public static string GetIdFromRequest(HttpRequest request)
        {
            return request.QueryString[KeyID];
        }

        public const string KeyCommentId = "CommentId";
        public static string GetCommentIdFromRequest(HttpRequest request)
        {
            return HttpUtility.UrlDecode(request.QueryString[KeyCommentId]);
        }
    }
}