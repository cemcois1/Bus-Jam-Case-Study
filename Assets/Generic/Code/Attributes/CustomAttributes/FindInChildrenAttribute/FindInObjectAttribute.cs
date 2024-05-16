using UnityEngine;

namespace _GenericPackageStart.Core.CustomAttributes
{
    public class FindInObjectAttribute : PropertyAttribute
    {
        //değer olarak component tipini almalıyım
        public string containInName;
        public FindInObjectAttribute(string containInName = null)
        {
            this.containInName = containInName;
        }

        public bool IsContainInName(string transformName)
        {
            if (containInName == null)
            {
                return true;
            }
            return transformName.Contains(containInName);
        }
    }
}