using UnityEngine;

namespace Utils
{
    public static class ScreenSpaceConverter
    {
        private static Camera mainCam;

        public static  Vector3 ViewportToWorldPoint(float vx, float fixedSpawnY, float offScreenOffset)
        {
            if (mainCam == null) mainCam = Camera.main;

            Ray ray = mainCam.ViewportPointToRay(new Vector3(vx, offScreenOffset, 0));
            Plane plane = new Plane(Vector3.up, new Vector3(0, fixedSpawnY, 0));

            if (plane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }

            return new Vector3(0, fixedSpawnY, 0);
        }
    }
}


