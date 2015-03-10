﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Geometry.Primitives;
using Archimedes.Geometry.Units;
using NUnit.Framework;

namespace Archimedes.Geometry.Tests
{
    public class Rectangle2Tests
    {
        [TestCase("100, 100", "500, 300", "0°")]
        [TestCase("0, 0", "50, 50", "0°")]
        public void Constructor(string locationStr, string sizeStr, string rotationStr)
        {
            var location = Vector2.Parse(locationStr);
            var size = SizeD.Parse(sizeStr);
            var rotation = Angle.Parse(rotationStr);

            var rect = new Rectangle2(location, size, rotation);

            Assert.AreEqual(location, rect.Location);
            Assert.AreEqual(size.Width, rect.Width);
            Assert.AreEqual(size.Height, rect.Height);
            Assert.AreEqual(rotation, rect.Rotation);
        }

        [TestCase("100, 100", "500, 300", "0°")]
        [TestCase("100, 100", "500, 300", "90°")]
        public void LocationRotation(string locationStr, string sizeStr, string rotationStr)
        {
            var location = Vector2.Parse(locationStr);
            var size = SizeD.Parse(sizeStr);
            var rotation = Angle.Parse(rotationStr);

            var rect = new Rectangle2(location, size, rotation);

            Assert.AreEqual(location, rect.Location);
            Assert.AreEqual(size.Width, rect.Width, GeometrySettings.DEFAULT_TOLERANCE);
            Assert.AreEqual(size.Height, rect.Height, GeometrySettings.DEFAULT_TOLERANCE);
            Assert.AreEqual(rotation, rect.Rotation);
        }

        [TestCase("(0, 0), (100, 0), (100, 200), (0, 200)", "50, 100", "0°")]
        //[TestCase("(0, 0), (100, 0), (100, 200), (0, 200)", "-100, 50", "90°")]
        // TODO add more tests with rotation
        public void FromVertices(string verticesStr, string expectedMiddleStr, string expectedRotationStr)
        {
            var vertices = Vector2.ParseAll(verticesStr);
            var expectedMiddle = Vector2.Parse(expectedMiddleStr);


            var rect = Rectangle2.FromVertices(vertices);
            var expectedRotation = Angle.Parse(expectedRotationStr);

            Assert.AreEqual(vertices[0], rect.Location);
            Assert.AreEqual(expectedMiddle, rect.MiddlePoint);
            Assert.AreEqual(expectedRotation, rect.Rotation);
        }

        /**/
        [TestCase(200,100)]
        public void Constructor2(double witdh, double height)
        {
            var rect = new Rectangle2(0, 0, 0, 0);

            rect.Width = witdh;
            rect.Height = height;

            var location = rect.Location;

            Assert.AreEqual(new Vector2(0, 0), location);
            Assert.AreEqual(witdh, rect.Width);
            Assert.AreEqual(height, rect.Height);
            Assert.AreEqual(Angle.Zero, rect.Rotation);
        }


    }
}
