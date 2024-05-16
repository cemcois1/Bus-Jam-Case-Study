using UnityEngine;

namespace _GenericPackageStart.Code._Mechanic.CustomAttributes.FinInParentAttribute
{
    public class FindInParentAttribute : PropertyAttribute
    {
        public string containInName;
        //değer olarak component tipini almalıyım
        public bool IsContainInName(string name)
        {
            if (containInName == null)
            {
                return true;
            }
            return name.Contains(containInName);
        }
        //değer olarak component tipini almalıyım
        public FindInParentAttribute(string name="")
        {
            this.containInName = name;
            
        }
    }
    public class CreateInChildAttribute : PropertyAttribute
    {
        
        public CreateInChildAttribute()
        {
            
        }
    }
}