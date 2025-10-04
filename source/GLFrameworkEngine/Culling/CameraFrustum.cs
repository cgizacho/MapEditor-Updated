﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GLFrameworkEngine
{
    // https://www.flipcode.com/archives/Frustum_Culling.shtml

    /// <summary>
    /// Detects if objects are within the given camera's Frustum
    /// </summary>
    public class CameraFrustum
    {
        Vector4[] Planes;
        BoundingBox AABB = new BoundingBox();

        /// <summary>
        /// Updates the Frustum planes using the given control's camera and projection matricies.
        /// This must be called each time the camera is updated.
        /// </summary>
        /// <param name="camera"></param>
        public void UpdateCamera(Camera camera) {
            Planes = CreateCameraFrustum(camera.ViewMatrix * camera.ProjectionMatrix);
        }

        public Vector4[] CreateCameraFrustum(Matrix4 m, bool normalize = true) {
            Vector4[] planes = FrustumHelper.ExtractFrustum(m, normalize);
            AABB.Set(planes);
            return planes;
        }

        /// <summary>
        /// Determines if the given bounding node is within the current camera Frustum.
        /// </summary>
        public bool CheckIntersection(Camera camera, BoundingNode bounding)
        {
            if (Planes == null) UpdateCamera(camera);

            //Check sphere detection
            var sphereFrustum = ContainsSphere(Planes,
                bounding.GetCenter(),
                bounding.GetRadius());

            switch (sphereFrustum)
            {
                case Frustum.FULL:
                    return true;
                case Frustum.NONE: //Check the box anyways atm to be sure
                case Frustum.PARTIAL: //Do bounding box detection
                    var boxFrustum = ContainsBox(Planes, bounding.Box);
                    if (boxFrustum != Frustum.NONE)
                        return true;
                    else
                        break;
            }

            foreach (var child in bounding.Children) {
                bool hasIntersection = CheckIntersection(camera, child);
                if (hasIntersection)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the given sphere is contained within the plane Frustum.
        /// </summary>
        static Frustum ContainsSphere(Vector4[] planes, Vector3 center, float radius)
        {
            for (int i = 0; i < 6; i++)
            {
                float dist = Vector3.Dot(center, planes[i].Xyz) + planes[i].W;
                if (dist < -radius)
                    return Frustum.NONE;

                if (MathF.Abs(dist) < radius)
                    return Frustum.PARTIAL;
            }
            return Frustum.FULL;
        }

        /// <summary>
        /// Checks if the given bounding box is contained within the plane Frustum.
        /// </summary>
        static Frustum ContainsBox(Vector4[] planes, BoundingBox box)
        {
            Frustum finalResult = Frustum.FULL;

            for (int p = 0; p < 6; p++)
            {
                var intersect = TestIntersct(planes[p], box.GetCenter(), box.GetExtent());
                if (intersect == Frustum.NONE)
                    return Frustum.NONE;

                finalResult = intersect;
            }
            return finalResult;
        }

        static Frustum TestIntersct(Vector4 plane, Vector3 center, Vector3 extent)
        {
            float d = Vector3.Dot(center, plane.Xyz) + plane.W;
            float n = extent.X * MathF.Abs(plane.X) +
                      extent.Y * MathF.Abs(plane.Y) +
                      extent.Z * MathF.Abs(plane.Z);

            if (d - n >= 0)
                return Frustum.FULL;
            if (d + n > 0)
                return Frustum.PARTIAL;
            return Frustum.NONE;
        }

        public enum Frustum
        {
            FULL,
            NONE,
            PARTIAL,
        }
    }
}
