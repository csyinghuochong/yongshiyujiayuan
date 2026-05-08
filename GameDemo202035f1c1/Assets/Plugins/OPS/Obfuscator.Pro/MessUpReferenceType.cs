using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace OPS.Obfuscator
{
    internal class MessUpReferenceType
    {
        public void MessTypeUpReferenceMethod()
        {
            int var_Code = UnityEngine.Random.Range(0,1);

            try
            {
                if(var_Code != 0)
                {
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(msEncrypt))
                        {
                            swEncrypt.Write(var_Code.ToString());
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
