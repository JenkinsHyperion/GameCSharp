using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.Engine;
using Game1.EntityComposites;

namespace Game1.Rendering
{
    public interface Graphic
    {

        void draw(ReferenceFrame camera, GraphicComposite.Static composite);
    }
}
