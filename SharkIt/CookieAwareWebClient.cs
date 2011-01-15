using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace SharkIt
{
    public class CookieAwareWebClient : WebClient
    {
        private CookieContainer m_cc;
        private string m_lastPage;

        public CookieAwareWebClient(CookieContainer cc)
        {
            m_cc = cc;
        }

        public CookieAwareWebClient(CookieContainer cc, string referer)
        {
            m_cc = cc;
            m_lastPage = referer;
        }

        protected override WebRequest GetWebRequest(System.Uri address)
        {
            WebRequest R = base.GetWebRequest(address);
            if (R is HttpWebRequest)
            {
                HttpWebRequest WR = (HttpWebRequest)R;
                WR.CookieContainer = m_cc;
                if (m_lastPage != null)
                {
                    WR.Referer = m_lastPage;
                }
            }
            m_lastPage = address.ToString();
            return R;
        }
    }
}
