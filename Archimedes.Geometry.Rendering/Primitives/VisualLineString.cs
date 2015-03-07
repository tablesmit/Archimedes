﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using Archimedes.Geometry.Primitives;

namespace Archimedes.Geometry.Rendering.Primitives
{
    public class VisualLineString : Visual
    {
        private readonly LineString _lineString;

        public VisualLineString(LineString lineString)
        {
            _lineString = lineString;
        }

        public override IGeometryBase Geometry
        {
            get { return _lineString; }
        }

        public override void Draw(Graphics g)
        {
            var path = new GraphicsPath();
            var vertices = _lineString.ToVertices();
            if (vertices.Count() > 1)
            {
                path.AddLines(Vertices.ToPoints(vertices).ToArray());
            }
            if (path.PointCount > 0)
                g.DrawPath(Pen, path);
        }


    }
}
