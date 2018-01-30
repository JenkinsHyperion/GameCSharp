using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.Entities;

namespace Game1.EntityComposites
{
    public interface EntityComposite
    {

        bool exists();

        void disableComposite();
        EntityStatic getOwnerEntity();

    }
}
