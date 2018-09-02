using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository.Interface
{
    public interface ICryptography
    {
        string Encrypt(string clearText);
        string Decrypt(string cipherText);
    }
}
