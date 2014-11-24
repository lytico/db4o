using System;
using System.Collections;
using System.Text;

namespace Sharpen.Util
{
    public interface ISet: ICollection
    {
    	bool Add(object element);
    	bool Remove(object element);
    	bool Contains(object element);
    }
}
