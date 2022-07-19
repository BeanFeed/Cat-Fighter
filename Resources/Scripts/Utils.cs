using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFighter.Resources.Scripts
{
    internal class Utils
    {
        public static float Dist(Godot.Vector2 obj1, Godot.Vector2 obj2)
        {
            float xes = (float)Math.Pow(obj2.x - obj1.x, 2);
            float yes = (float)Math.Pow(obj2.y - obj1.y, 2);
            return (float)Math.Sqrt(xes + yes);
        }
        public static float Dist(float x1, float y1, float x2, float y2)
        {
            float xes = (float)Math.Pow(x2 - x1, 2);
            float yes = (float)Math.Pow(y2 - y1, 2);
            return (float)Math.Sqrt(xes + yes);
        }
        public static bool Within(float point, float testingPoint)
        {
            var lowPoint = point + 5;
            var highPoint = point - 5;
            if (lowPoint > testingPoint && testingPoint > highPoint)
            {
                return true;
            }
            return false;
        }
    }
}
