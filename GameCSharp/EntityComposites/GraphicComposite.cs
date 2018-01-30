using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.Rendering;
using Game1.EntityComposites;
using Game1.Engine;
using Game1.Entities;

namespace Game1.EntityComposites
{
    public abstract class GraphicComposite : EntityComposite
    {


        private static GraphicComposite.Null nullSingleton = new Null();

        public abstract Sprite getSprite();
        public abstract void setSprite(Sprite sprite);
        public abstract void setGraphicSizeFactor(double factor);
        public abstract void draw(ReferenceFrame camera);

        public abstract void setGraphicSizeFactorX(double d);
        public abstract void setGraphicSizeFactorY(double d);

        protected abstract void notifyAngleChangeFromAngularComposite(double angle);

        public abstract void setGraphicAngle(double d);

        public abstract double getGraphicSizeFactor();
        public abstract double getGraphicsSizeX();
        public abstract double getGraphicsSizeY();
        public abstract double getGraphicAngle();

        public abstract void deactivateGraphic();
        public abstract void activateGraphic();

        public abstract Shape getGraphicAbsoluteBounds(Point ownerEntityPosition);
        public abstract Shape getGraphicRelativeBounds(int areaExtender);

        public abstract bool exists();
        public abstract void disableComposite();
        public abstract EntityStatic getOwnerEntity();

        public static GraphicComposite.Null nullGraphicsComposite()
        {
            return nullSingleton;
        }


        public class Static : GraphicComposite
        {

            protected String compositeName;
            EntityStatic ownerEntity;
            protected Sprite currentSprite = Sprite.missingSprite;
            private RenderingEngine.ActiveGraphic rendererSlot;

            protected double graphicSizeFactorX = 1;
            protected double graphicSizeFactorY = 1;
            protected double graphicAngle = 0;

            protected Static(Sprite current, EntityStatic ownerEntity)
            {
                this.ownerEntity = ownerEntity;
                this.currentSprite = current;
                this.compositeName = "Graphics" + this.getClass().getSimpleName();
            }

            protected Static(EntityStatic ownerEntity)
            {
                this.ownerEntity = ownerEntity;
                this.compositeName = "Graphics" + this.getClass().getSimpleName();
            }

            public EntityStatic ownerEntity()
            {
                return getOwnerEntity();
            }
            /** created new method to remove refactoring issues with name inconsistency*/
            public override EntityStatic getOwnerEntity()
            {
                return this.ownerEntity;
            }
            
                public override Sprite getSprite()
                {
                    return currentSprite;
                }
            public override void setSprite(Sprite sprite)
            {
                this.currentSprite = sprite;
            }



                public override void setGraphicSizeFactor(double factor)
                {
                    this.graphicSizeFactorX = factor;
                    this.graphicSizeFactorY = factor;
                }

            public override void setGraphicSizeFactorX(double factorX)
                {
                    this.graphicSizeFactorX = factorX;
                }

            public override void setGraphicSizeFactorY(double factorY)
                {
                    this.graphicSizeFactorY = factorY;
                }

            public override double getGraphicSizeFactor()
                {
                    // TODO Auto-generated method stub
                    return this.graphicSizeFactorX;
                }

            public override double getGraphicsSizeX()
            {
                return this.graphicSizeFactorX;
            }

            public override double getGraphicsSizeY()
            {
                return this.graphicSizeFactorY;
            }

            public override void setGraphicAngle(double angle)
                {
                    this.graphicAngle = angle;
                }

            protected override void notifyAngleChangeFromAngularComposite(double angle)
                {
                    //DO NOTHING
                }

            public override double getGraphicAngle()
            {
                return this.graphicAngle;
            }

            public override void draw(ReferenceFrame camera)
            {
                this.currentSprite.draw(camera, this);
            }

            public void addCompositeToRenderer(RenderingEngine engine)
            {
                rendererSlot = engine.addGraphicsCompositeToRenderer(this);
            }

            public void addCompositeToRenderer(RenderingEngine engine, int layer)
            {
                rendererSlot = engine.addGraphicsCompositeToRenderer(this, layer);
            }

            public override Shape getGraphicAbsoluteBounds(Point ownerEntityPosition)
            {

                return currentSprite.getGraphicAbsoluteTranslationalBounds(graphicSizeFactorX, graphicSizeFactorY, ownerEntityPosition);
            }

            public override Shape getGraphicRelativeBounds(int areaExtender)
            {

                return currentSprite.getGraphicRelativeTranslationalBounds(graphicSizeFactorX, graphicSizeFactorY, areaExtender);
            }

            
            public override bool exists()
            {
                return true;
            }


            public override void disableComposite()
                {
                    //more disabling 
                    //System.out.println("Removing graphics from renderer");
                    rendererSlot.deactivateInRenderingEngine();
                    rendererSlot = null;
                    this.ownerEntity.nullifyGraphicsComposite();
                }

            //@Override
                public String toString()
            {
                return this.compositeName;
            }

            //@Override
            public override void deactivateGraphic()
            {
                rendererSlot.deactivateInRenderingEngine();
            }

            //@Override
            public override void activateGraphic()
            {
                rendererSlot.activateInRenderingEngine();
            }

        }

        public class Rotateable : Static
        {


                protected Rotateable(EntityStatic ownerEntity) : base(ownerEntity)
                {
                    
                }

            protected override void notifyAngleChangeFromAngularComposite(double angle)
                {
                    this.setGraphicAngle(angle);
                }

            public override Shape getGraphicAbsoluteBounds(Point ownerEntityPosition)
                {

                    return currentSprite.getGraphicAbsoluteRotationalBounds(graphicSizeFactorX, graphicSizeFactorY, graphicAngle, ownerEntityPosition);
                }

            public override Shape getGraphicRelativeBounds(int areaExtender)
                {

                    return currentSprite.getGraphicRelativeRotationalBounds(graphicSizeFactorX, graphicSizeFactorY, graphicAngle, areaExtender);
                }
		
	    }
	
	    private class Null : GraphicComposite
        {

            protected String compositeName;

            //constructor
            internal Null()
            {
                this.compositeName = "GraphicComposite" + this.getClass().getSimpleName();
                //this.currentSprite = new SpriteStillframe( new MissingIcon().paintMissingSprite() );//new MissingIcon().paintMissingSprite();
                //this.currentSprite = new SpriteStillframe("missing");
            }

            public EntityStatic ownerEntity()
            {
                return this.getOwnerEntity();
            }
            public override EntityStatic getOwnerEntity()
            {
                return null;
            }

            public override Sprite getSprite()
            {
                //System.err.println("Attempting to get sprite on null graphics composite");
                return Sprite.missingSprite;
            }
            //@Override
            public override void setSprite(Sprite sprite)
            {
                //System.err.println("Attempting to set sprite on null graphics composite");
            }
            //@Override
            public override void draw(ReferenceFrame camera)
            {
                //System.err.println("Attempting to draw null graphics composite");
            }

            //@Override
            protected override void notifyAngleChangeFromAngularComposite(double angle)
            {
                //DO NOTHING//
            }

            public void addCompositeToRenderer(RenderingEngine engine)
            {
                //System.err.println("Attempting to add null graphics composite to rendering engine");
                //This should catch null composites from getting into the renderer list 
            }

            //@Override
            public override bool exists()
            {
                return false;
            }

            //@Override
            public override void disableComposite()
            {
                //System.err.println("No graphics to disable");
            }



            //@Override
            public override void setGraphicSizeFactor(double factor)
            {
                //System.err.println("Attempting to set size factor of null graphics composite");
            }

            //@Override
            public override double getGraphicsSizeX()
            {
                // TODO Auto-generated method stub
                return 1;
            }

            //@Override
            public override double getGraphicsSizeY()
            {
                // TODO Auto-generated method stub
                return 1;
            }

            // @Override
            public override void setGraphicSizeFactorX(double d)
            {
                // TODO Auto-generated method stub

            }

            //@Override
            public override void setGraphicSizeFactorY(double d)
            {
                // TODO Auto-generated method stub

            }

            //@Override
            public override double getGraphicAngle()
            {
                //System.err.println("Warning: Attempign to set ANgle of null graphics singleton");
                return 0;
            }

            //@Override
            public override void setGraphicAngle(double d)
            {
                //System.err.println("Warning: Attempign to set ANgle of null graphics singleton");
            }
            //@Override
            public String toString()
            {
                //FIXME come back to later to decide if this should return the current string, or getClass().getSimpleName()
                return this.compositeName;
            }

            //@Override
            public override void deactivateGraphic()
            {
                //System.err.println("Warning: Attemping to deactivate null graphic composite");
            }

            //@Override
            public override void activateGraphic()
            {
                // TODO Auto-generated method stub

            }

            //@Override
            public override Shape getGraphicAbsoluteBounds(Point ownerEntityPosition)
            {
                throw new RuntimeException();
            }

            //@Override
            public override Shape getGraphicRelativeBounds(int areaExtender)
            {
                throw new RuntimeException();
            }

            //@Override
            public override double getGraphicSizeFactor()
            {
                // TODO Auto-generated method stub
                return 1;
            }
        }
	
    }
}
