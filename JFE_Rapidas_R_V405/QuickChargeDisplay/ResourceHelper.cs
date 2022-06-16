using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Globalization;
using System.Collections;

namespace QuickChargeDisplay
{
    public static class ResourceHelper
    {
        private static UriBuilder uriBuilder = new UriBuilder();

        public static UnmanagedMemoryStream GetResourceStream(Type type, string name)
        {
            Assembly assembly = Assembly.GetAssembly(type);

            string resourceName = assembly.GetName().Name + ".g";

            ResourceManager rm = new ResourceManager(resourceName, assembly);

            UnmanagedMemoryStream stream;

            using (ResourceSet set = rm.GetResourceSet(CultureInfo.CurrentCulture, true, true))
            {
                // 리소스 경로 생성
                uriBuilder.Path = name;

                stream = (UnmanagedMemoryStream)set.GetObject(uriBuilder.Path, true);
            }

            return stream;
        }
    }
}
