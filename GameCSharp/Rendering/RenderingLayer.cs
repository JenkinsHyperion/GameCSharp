using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.Engine;

namespace Game1.Rendering
{
    public class RenderingLayer
    {

        private double PARALLAX_X; // 1 = background moves as fast as entities, 100 = background nearly static relative to camera 
                                         // negative = parallax moves in opposite direction, 0 = divide by zero error so can you just not
        private double PARALLAX_Y;

        private double ZOOM_SCALE;

        private MovingCamera camera1;
        private ParallaxFrame parallaxFrame = new ParallaxFrame();

        protected ArrayList<GraphicComposite.Static> entitiesList = new ArrayList<>();

        public RenderingLayer(double parallax_x, double parallax_y, double zoomScale, MovingCamera camera)
        {
            PARALLAX_X = parallax_x;
            PARALLAX_Y = parallax_y;
            ZOOM_SCALE = zoomScale;
            this.camera1 = camera;
        }

        public void addGraphicToLayer(EntityStatic entity)
        {
            this.entitiesList.add((Static)entity.getGraphicComposite()); //TODO REMOVAL INDEXING
        }

        public void addGraphicToLayer(GraphicComposite graphic)
        {
            this.entitiesList.add((Static)graphic); //TODO REMOVAL INDEXING
        }

        public void renderLayer(MovingCamera camera)
        {

            parallaxFrame.setPosition((int)(camera1.getX() / PARALLAX_X),
                    (int)(camera1.getY() / PARALLAX_Y),
                    camera1.getGraphics(),
                    camera1.getObserver()
                    );

            for (GraphicComposite.Static comp : entitiesList)
            { //FIXME Redo this entire class

                //comp.draw(cam);		
                AffineTransform frameTransform = new AffineTransform();

                //frameTransform.scale(camera.getZoom(), camera.getZoom() );

                final double zoom = (ZOOM_SCALE / camera.getDeltaZoom()) + 1;

                frameTransform.translate(
                        ((-(camera.getX())) * (camera.getZoom()) * Math.pow(ZOOM_SCALE, 1)),
                        ((-(camera.getY())) * (camera.getZoom()) * Math.pow(ZOOM_SCALE, 1))
                        );

                frameTransform.translate(BoardAbstract.B_WIDTH / 2, BoardAbstract.B_HEIGHT / 2);    //centering translation

                frameTransform.scale(1 / zoom, 1 / zoom);


                frameTransform.scale(comp.getGraphicsSizeX(), comp.getGraphicsSizeY());

                frameTransform.translate(
                        ((comp.getOwnerEntity().getX())),
                        ((comp.getOwnerEntity().getY()))
                        );

                frameTransform.translate((comp.getSprite().spriteOffsetX), (comp.getSprite().spriteOffsetY));

                parallaxFrame.drawOnCamera(comp, frameTransform);

            }

        }



    }
}
