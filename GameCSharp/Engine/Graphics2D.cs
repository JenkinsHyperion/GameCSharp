using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.Xna.Framework;

namespace Game1.Engine
{
    public class Graphics2D
    {

        private Graphics g;

        /*public Graphics2D create()
        {
            return g.
        }*/

        public void setColor( Microsoft.Xna.Framework.Color color )
        {
            
        }

        public Microsoft.Xna.Framework.Color getColor()
        {
            return Microsoft.Xna.Framework.Color.White;
        }

        public void translate( int transX, int transY )
        {
            g.TranslateTransform( transX, transY );
        }
        
        public void rotate( float angleRadians )
        {
            g.RotateTransform(angleRadians);
        }
    }
}
