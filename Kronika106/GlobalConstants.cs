using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Kronika106
{
    public static class GlobalConstants
    {
        public const int MaxYearMenuDescriptionLenght = 200;
        public const Char Space = ' ';
        public const string EmptyPopis = "...";
        public const string PthFileSystemRoot = "AllPhotos";
        public const string PthVideoRoot = "Video";
        public const string fnRokPopis = "RokPopis.txt";
        public const string fnRokPopisShort = "RokPopisKratky.txt";
        public const string fnRokFotka = "RokFotka.jpg";
        public const string fnAkciaPopis = "AkciaPopis.txt";
        public const string fnAkciaPopisShort = "AkciaPopisKratky.txt";
        public const string fnAkciaFotka = "AkciaFotka.jpg";
        public const string fnVideoDir = "Video";
        public const string fnVideoPopis = "VideoPopis.txt";
        public const string fnVideoPopisShort = "VideoPopisKratky.txt";
        public const string fnVideoMP4 = "video.mp4";
        public const string fnVideoOGV = "video.ogv";
        public const string fnVideoWEBM = "video.webm";
        public const string fnVideoPoster = "poster.jpg";
        public const string PthDefaultRokFotka = "~/AllPhotos/DefaultRokFotka.jpg";
        public const string fnMapa = "mapa.txt";
        public const string PthThumb = "Thumbs";
        public const string ContentTypeJpeg = "image/JPEG";
        public const string RedirectURLKey = "LastOKurl";
        public const string UserNick = "LoginUserNick";                        

        public const string EmailRegistrationSendOK = "Potvrdenie registrácie bolo odoslané na email: {0}. Registrácia bude dokončená kliknutím na linku v správe.";

        public const string LoadedCommentsCount = "loadedCommentsCount";
        public const string MaxId = "maxId";

        public const string urlDefault = "~/Default.aspx";
        public const string urlForbidden = "~/Account/Login.aspx?EventForbidden=1";        


        public const string RoleAdmin = "admin";

        public static readonly char[] EventIdSeparator = {/* '/' */ Path.AltDirectorySeparatorChar};
	}
}