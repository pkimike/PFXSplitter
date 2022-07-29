using System;
using System.Runtime.InteropServices;
using System.Security;

namespace PFXSplitter {
    static class Extensions {
        public static Boolean Matches(this SecureString ss1, SecureString ss2) {
            if (ss1 == null && ss2 == null) {
                return true;
            }

            if (ss1 == null || ss2 == null) {
                return false;
            }

            if (ss1.Length != ss2.Length) {
                return false;
            }

            IntPtr ss_bstr1_ptr = IntPtr.Zero;
            IntPtr ss_bstr2_ptr = IntPtr.Zero;

            try {
                ss_bstr1_ptr = Marshal.SecureStringToBSTR(ss1);
                ss_bstr2_ptr = Marshal.SecureStringToBSTR(ss2);

                String str1 = Marshal.PtrToStringBSTR(ss_bstr1_ptr);
                String str2 = Marshal.PtrToStringBSTR(ss_bstr2_ptr);

                return str1.Equals(str2, StringComparison.InvariantCulture);
            } finally {
                if (ss_bstr1_ptr != IntPtr.Zero) {
                    Marshal.ZeroFreeBSTR(ss_bstr1_ptr);
                }

                if (ss_bstr2_ptr != IntPtr.Zero) {
                    Marshal.ZeroFreeBSTR(ss_bstr2_ptr);
                }
            }
        }

        public static Boolean IsNullOrEmpty(this SecureString secureString) {
            if (secureString == null) {
                return true;
            }

            return secureString.Length == 0;
        }
    }
}
