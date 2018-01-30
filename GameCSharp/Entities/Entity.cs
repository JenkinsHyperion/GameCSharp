using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.Utilities;
using Microsoft.Xna.Framework;

namespace Game1.Entities
{
    public class Entity
    {
        //will be heavily fleshed out later when organizing large amounts of entities becomes an issue
        public static Point origin = new Point(0,0);

        public static int count;
        public String name;
        protected bool alive = true;
        protected bool collidable = true; //default to true
        protected double x;
        protected double y;
        public Point position = new Point(0, 0);
        protected int entityType;

        public Entity(int x, int y)
        {
            this.x = x;
            this.y = y;
            position.setLocation(x, y);

            name = "blank entity" + count;
            count++;
        }


        public void selfDestruct()
        {
            alive = false;
        }

        public bool isAlive()
        {
            return alive;
        }

        public int getX()
        {
            return (int)x;
        }

        public int getY()
        {
            return (int)y;
        }

        public void setX(double setx)
        {
            x = setx;
            position.setLocation(setx, position.getY());
        }

        public void setY(double sety)
        {
            y = sety;
            position.setLocation(position.getX(), sety);
        }
        /** returns copy of position */
        public Point getPosition()
        {
            return new Point((int)x, (int)y);
        }

        public void setPos(Point p)
        {
            x = (int)p.getX();
            y = (int)p.getY();
            position.setLocation(x, position.getY());
            position.setLocation(position.getX(), y);
        }
        public void setPos(int x, int y)
        {
            this.x = x;
            this.y = y;
            position.setLocation(x, position.getY());
            position.setLocation(position.getX(), y);
        }
        public void setPos(double x, double y)
        {
            this.x = x;
            this.y = y;
            position.setLocation(x, position.getY());
            position.setLocation(position.getX(), y);
        }
        public void setPos(Point2D p)
        {
            this.x = p.getX();
            this.y = p.getY();
            position.setLocation(x, position.getY());
            position.setLocation(position.getX(), y);
        }


        /**
         * Returns this entity's X position relative to the camera screen, rather than the X position in board
         * @param camera
         * @return the ordinate of this entity relative to the camera area
         */
        public int getXRelativeTo(MovingCamera camera)
        {
            return camera.getRelativeX((int)this.x);
        }

        /**
         * Returns this entity's Y position relative to the camera screen, rather than the Y position in board
         * @param camera
         * @return the ordinate of this entity relative to the camera area
         */
        public int getYRelativeTo(MovingCamera camera)
        {
            return camera.getRelativeY((int)this.y);
        }

        /**
         * Returns this entity's position relative to the camera screen, rather than the Y position in board
         * @param camera
         * @return
         */
        public Point getPositionRelativeTo(MovingCamera camera)
        {
            return new Point(
                        camera.getRelativeX((int)this.x),
                        camera.getRelativeY((int)this.y)
                    );
        }

        public Point getRelativeTranslationalPositionOf(Entity entity)
        {
            return new Point(
                        entity.getX() - this.getX(),
                        entity.getY() - this.getY()
                    );
        }

        public Point getRelativeTranslationalPositionOf(Point world_position)
        {
            return new Point(
                        world_position.x - this.getX(),
                        world_position.y - this.getY()
                    );
        }

        public Point getTranslationalAbsolutePositionOf(Point point_on_entity)
        {
            return new Point(
                        point_on_entity.x + this.getX(),
                        point_on_entity.y + this.getY()
                    );
        }

        public Point getTranslationalAbsolutePositionOf(Point2D point_on_entity)
        {
            return new Point(
                        (int)point_on_entity.getX() + this.getX(),
                        (int)point_on_entity.getY() + this.getY()
                    );
        }

        public Vector getSeparationVector(Entity partner)
        {
            return new Vector(partner.x - x, partner.y - y);
        }
        public Vector getSeparationVector(Point position)
        {
            return new Vector(position.x - x, position.y - y);
        }
        public Vector getSeparationVector(Point2D position)
        {
            return new Vector(position.getX() - x, position.getY() - y);
        }
        public Vector getSeparationUnitVector(Entity partner)
        {
            return getSeparationVector(partner).unitVector();
        }
        public Vector getSeparationUnitVector(Point position)
        {
            return getSeparationVector(position).unitVector();
        }
        public Vector getSeparationUnitVector(Point2D position)
        {
            return getSeparationVector(position).unitVector();
        }

        public Point getPositionReference()
        {
            return this.position;
        }

    }
}
