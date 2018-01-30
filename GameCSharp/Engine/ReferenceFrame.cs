using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace Game1.Engine
{
    public interface ReferenceFrame
    {

        void drawOnCamera(GraphicComposite.Static sprite, AffineTransform entityTransform);
        void drawString(String string , int world_x, int world_y);
        void debugDrawPolygon(Shape polygon, Color color, Point point, AffineTransform entityTransform, float alpha);

        void draw(Image image, Point world_position, AffineTransform entityTransform, float alpha);

        int getOriginX();
        int getOriginY();

        ImageObserver getObserver();
        Graphics2D getGraphics();
        void drawLine(Point p1, Point p2);
        void drawLine(Point2D p1, Point2D p2);
    }
}
