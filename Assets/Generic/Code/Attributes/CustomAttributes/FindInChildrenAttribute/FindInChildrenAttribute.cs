using UnityEngine;

namespace _GenericPackageStart.Core.CustomAttributes
{
    public class FindInChildrenAttribute : PropertyAttribute
    {
        public string containInName;
        //değer olarak component tipini almalıyım
        public FindInChildrenAttribute(string containInName = null)
        {
            this.containInName = containInName;
        }
        public bool IsContainInName(string name)
        {
            if (containInName == null)
            {
                return true;
            }
            return name.Contains(containInName);
        }
    }
}
