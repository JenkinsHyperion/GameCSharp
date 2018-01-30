using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.Utilities;
using Game1.Rendering;

namespace Game1.Engine
{
    public class RenderingEngine
    {

        private MovingCamera camera;
        private BoardAbstract ownerBoard;

        private Graphics2D graphics;

        //private LinkedHead layer1Head;

        private DoubleLinkedList<ActiveGraphic> activeSpriteCompositeList = new DoubleLinkedList<ActiveGraphic>();

        public RenderingLayer[] layersList;

        private BufferedImage image = new BufferedImage(ownerBoard.B_WIDTH, ownerBoard.B_HEIGHT, BufferedImage.TYPE_INT_ARGB);

        private List<OverlayComposite> visibleOverlayList = new List<OverlayComposite>();

        public RenderingEngine(BoardAbstract board)
        {
            this.camera = new MovingCamera(board, this.graphics, board);
            this.ownerBoard = board;
            board.addEntityToUpdater(camera);
            init();
        }

        private void init()
        {

            layersList = new RenderingLayer[]{
            new RenderingLayer(1,1,1,camera),
            new RenderingLayer(1.1,1.1 ,0.98 ,camera), //nearest
			new RenderingLayer(1.6,1.4 , 0.9 ,camera),
            new RenderingLayer(2.0, 1.6, 0.4 ,camera),
            new RenderingLayer(3.0, 3.0, 0.05 ,camera ),
            new RenderingLayer(5, 5 ,0.1,camera),
            new RenderingLayer(10, 10 ,0.01,camera),
            new RenderingLayer(100, 100 ,0,camera) //farthest
		};

        }

        public Graphics2D debugGetOverlayGraphics()
        {
            image = new BufferedImage(ownerBoard.B_WIDTH, ownerBoard.B_HEIGHT, BufferedImage.TYPE_INT_ARGB);
            //image.
            return (Graphics2D)image.getGraphics();
        }

        public ReferenceFrame debugGetOverlayCamera()
        {
            return camera;
        }

        public void render(Graphics2D g2)
        { //ENTRY POINT OF RENDERING ENGINE FROM BOARD PAINTCOMPONENT(g)

            //Repainting
            this.graphics = g2;
            camera.repaint(g2);

            for (int i = layersList.length - 1; i > -1; i--)
            {
                layersList[i].renderLayer(camera);
            }

            //int spriteNumber = 0;

            while (activeSpriteCompositeList.hasNext())
            {

                ActiveGraphic graphic = activeSpriteCompositeList.get();

                graphic.composite.draw(camera);
                //spriteNumber++;

            }
            //		g2.setColor(Color.CYAN);
            //		g2.drawString( "Rendering Engine: Sprites: "+ spriteNumber , 20, 300);

            //Overlays

            for (OverlayComposite overlay : visibleOverlayList)
            {
                overlay.paintOverlay(camera.getOverlayGraphics(), camera);
            }

        }


        public ActiveGraphic addGraphicsCompositeToRenderer(GraphicComposite graphic, int layer)
        {

            try
            {
                ActiveGraphic newActiveGraphic = new ActiveGraphic(graphic);

                layersList[layer].addGraphicToLayer(graphic);

                return newActiveGraphic;
            }

            catch (ClassCastException exc)
            {
                System.err.println("Rendering Engine attempted to add spriteless entity.");
                return null;
            }

        }

        public ActiveGraphic addGraphicsCompositeToRenderer(GraphicComposite graphic)
        {

            try
            {
                ActiveGraphic newActiveGraphic = new ActiveGraphic(graphic);
                newActiveGraphic.listPosition = activeSpriteCompositeList.add(newActiveGraphic);
                return newActiveGraphic;
            }

            catch (ClassCastException exc)
            {
                System.err.println("Rendering Engine attempted to add spriteless entity.");
                return null;
            }

        }

        public MovingCamera getCamera()
        {
            return this.camera;
        }

        public void debugClearRenderer()
        {

            activeSpriteCompositeList.clear();

        }



        public OverlayComposite addOverlay(Overlay overlay)
        {
            OverlayComposite newOverlayComp = new OverlayComposite(overlay);
            newOverlayComp.setHashID(this.visibleOverlayList.size(), this);
            this.visibleOverlayList.add(newOverlayComp);
            return newOverlayComp;

        }

        public OverlayComposite quickAddOverlay(Overlay overlay)
        {

            OverlayComposite newOverlayComp = new OverlayComposite(overlay);
            newOverlayComp.setHashID(0, this);
            newOverlayComp.toggle();
            return newOverlayComp;

        }

        public OverlayComposite showOverlay(OverlayComposite overlay)
        {

            overlay.setHashID(visibleOverlayList.size(), this);

            visibleOverlayList.add(overlay);

            return overlay;

        }

        public void removeOverlay(int hashID)
        {

            for (int i = hashID + 1; i < visibleOverlayList.size(); i++)
            {
                visibleOverlayList.get(i).setHashID(i - 1, this);
            }

            visibleOverlayList.remove(hashID);

        }

        public Graphics2D getGraphics()
        {
            return graphics;
        }

        public class ActiveGraphic
        {

            protected GraphicComposite composite;
            protected ListNodeTicket listPosition;

            protected ActiveGraphic(GraphicComposite composite)
            {
                this.composite = composite;
            }

            public void activateInRenderingEngine()
            {
                if (listPosition == null)
                {
                    listPosition = activeSpriteCompositeList.add(this);
                }
                else
                {
                    System.err.println("Graphic is already active");
                }
            }

            public void deactivateInRenderingEngine()
            {
                if (listPosition != null)
                {
                    listPosition.removeSelfFromList();
                    listPosition = null;
                }
                else
                {
                    System.err.println("Graphic is already inactive");
                }
            }

            public void notifyRenderingEngineOfDisabledGraphic()
            {
                composite = null;
                listPosition.removeSelfFromList();
                listPosition = null;
            }
        }



        public class SpriteOverlay implements Overlay
        {


        protected Font defaultFont = new Font(Font.SANS_SERIF, Font.PLAIN, 12);
        protected Font contextFont = new Font(Font.SANS_SERIF, Font.PLAIN, 16);

        @Override
        public void paintOverlay(Graphics2D g2, MovingCamera cam)
        {

            cam.setColor(Color.GREEN);
            g2.setColor(Color.GREEN);
            g2.setFont(contextFont);
            g2.drawString("SPRITE OVERLAY", 500, 30);
            g2.setFont(defaultFont);
            g2.drawString("Active Sprites: " + activeSpriteCompositeList.size(), 500, 45);

            while (activeSpriteCompositeList.hasNext())
            {

                ActiveGraphic graphic = activeSpriteCompositeList.get();

                Shape relativeShape = graphic.composite.getGraphicRelativeBounds(0);

                camera.drawShapeInWorld(relativeShape, graphic.composite.getOwnerEntity().getPosition());

            }

        }
    }

}
