using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace ZSCY_Win10.Util
{
    public static class GetCredential
    {
        public static PasswordCredential getCredential(string resource)
        {
            PasswordCredential credential = null;
            var vault = new PasswordVault();
            var credentialList = vault.FindAllByResource(resource);
            if (credentialList.Count == 1)
            {
                credentialList[0].RetrievePassword();
                credential = credentialList[0];
            }
            return credential;
        }
    }
}
