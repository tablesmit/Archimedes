﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Archimedes.Geometry.Primitives
{
    public partial class Line2 : IGeometryBase
    {


        /// <summary>
        /// Calculates the interception point of two Lines.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="tolerance"></param>
        /// <returns>Returns a Point if func succeeds. If there is no interception, empty point is returned.</returns>
        private Vector2? InterceptLine(Line2 other, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {

            double interPntX = 0;
            double interPntY = 0;

            if (other == null || this.EqualSlope(other, tolerance))
            {
                return null;            // Lines are parralell
            }

            //intercept of two endless lines
            if (!this.IsVertical && !other.IsVertical) {    // both NOT vertical
                interPntX = ((-1 * (this.YMovement - other.YMovement)) / (this.Slope - other.Slope));
                interPntY = (this.Slope * interPntX + this.YMovement);
            } else if (this.IsVertical) {                  // this vertical (so it must lie on this.X)
                interPntX = this.Start.X;
                interPntY = (other.Slope * interPntX + other.YMovement);
            } else if (other.IsVertical) {                // Line2 vertical (so it must lie on Line2.X)
                interPntX = other.Start.X;
                interPntY = (this.Slope * interPntX + this.YMovement);
            }

            var interPnt = new Vector2(interPntX, interPntY);

            //check if computed intercept lies on our line.
            if (this.Contains(interPnt, tolerance) && other.Contains(interPnt, tolerance))
            {
                return interPnt;
            }else
                return null;
        }

        /// <summary>
        /// Is there a interception?
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Returns true/false.</returns>
        public bool InterceptLineWith(Line2 other, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            double intersectionX = 0;
            double intersectionY = 0;

            if (other == null || this.EqualSlope(other, tolerance))
                return false;

            //intercept of two endless lines
            if ((!this.IsVertical) && (!other.IsVertical)) {    // both NOT vertical
                intersectionX = ((-1 * (this.YMovement - other.YMovement)) / (this.Slope - other.Slope));
                intersectionY = (this.Slope * intersectionX + this.YMovement);
            } else if (this.IsVertical) {                  // this vertical (so it must lie on this.X)
                intersectionX = this.Start.X;
                intersectionY = (other.Slope * intersectionX + other.YMovement);
            } else if (other.IsVertical) {                // Line2 vertical (so it must lie on Line2.X)
                intersectionX = other.Start.X;
                intersectionY = (this.Slope * intersectionX + this.YMovement);
            }

            var intersection = new Vector2(intersectionX, intersectionY);

            // check if computed intercept lies on our line.
            return (this.Contains(intersection, tolerance) && other.Contains(intersection, tolerance));
        }

        /// <summary>
        /// Returns true if there is at least one Interception Point.
        /// </summary>
        /// <param name="rect1"></param>
        /// <returns></returns>
        public bool InterceptRectWith(RectangleF rect1, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            
            // is Start/EndPoint in the Rectangle?
            if (rect1.Contains(this.Start) || rect1.Contains(this.End)) // TODO handle collision
                return true; 
            // crosses the Line a Rectangle Border?
            var borderLines = Line2.RectExplode(rect1); //get 4 borderlines from rect

            // check if any of the borderlines intercept with this line
            return borderLines.Any(border => this.InterceptLineWith(border, tolerance));
        }


        /// <summary>Returns every intersection Point of the Rect::Line if any. Max returned Points are 2.
        /// This Method actaully can't handle full contained Lines in the Rect - use the InterceptRectWith to determite if there is collision.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="tolerance"></param>
        /// <returns>Returns count of Interception Points</returns>
        public List<Vector2> InterceptRect(RectangleF rect, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            var intercepts = new List<Vector2>(2);
            short i = 0;
            var borderLines = Line2.RectExplode(rect); //get 4 borderlines from rect

            foreach (var border in borderLines) {
                if (this.InterceptLineWith(border, tolerance))
                {
                    // found interception
                    var pnt = this.InterceptLine(border, tolerance);
                    if(pnt.HasValue)
                        intercepts.Add(pnt.Value);
                    i++;
                    if (i == 2) break;  
                }
            }
            return intercepts;
        }



        /// <summary>
        /// Checks if a Point is on the 2dLine (or in its range)
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="tolerance"></param>
        /// <returns>true/false</returns>
        public bool Contains(Vector2 pos, double tolerance = GeometrySettings.DEFAULT_TOLERANCE) {

            // We can't check directly with math functions,
            // as it is possible that the slope is undefinied. (on vertical lines)

            if (this.IsVertical) {
                // Vertical means that the slope is undefinied
                if (Math.Abs(this.Start.X - pos.X) <= tolerance)
                {
                    return (((pos.Y >= this.Start.Y) && (pos.Y <= this.End.Y)) || ((pos.Y <= this.Start.Y) && (pos.Y >= this.End.Y)));
                } else 
                    return false;
            } else {
                if (Math.Abs(pos.Y - (pos.X * (float)this.Slope + (float)this.YMovement)) <= tolerance)
                {
                    return (((pos.X >= this.Start.X) && (pos.X <= this.End.X)) || ((pos.X <= this.Start.X) && (pos.X >= this.End.X)));
                } else
                    return false;
            }
        }
    }
}
