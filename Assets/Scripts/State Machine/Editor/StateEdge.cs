using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace CombatSystem.StateMachine.Editor
{
    public class StateEdge : Edge
    {
        EdgeArrow arrow;

        public StateEdge()
        {
            arrow = new(this);
            Add(arrow);
            edgeControl.RegisterCallback<GeometryChangedEvent>(OnEdgeControlGeometryChanged);
        }

        private void OnEdgeControlGeometryChanged(GeometryChangedEvent evt)
        {
            PointsAndTangents[1] = PointsAndTangents[0];
            PointsAndTangents[2] = PointsAndTangents[3];

            if(input != null && output != null)
            {
                if (input.node.GetPosition().y > output.node.GetPosition().y)
                {
                    PointsAndTangents[1].x += 8;
                    PointsAndTangents[2].x += 8;
                }
                else if (input.node.GetPosition().y < output.node.GetPosition().y)
                {
                    PointsAndTangents[1].x -= 8;
                    PointsAndTangents[2].x -= 8;
                }
                else if (input.node.GetPosition().y == output.node.GetPosition().y)
                {
                    PointsAndTangents[1].x -= 1;
                    PointsAndTangents[1].y -= 1;
                }

                if (input.node.GetPosition().x > output.node.GetPosition().x)
                {
                    PointsAndTangents[1].y -= 8;
                    PointsAndTangents[2].y -= 8;
                }
                else if(input.node.GetPosition().x < output.node.GetPosition().x)
                {
                    PointsAndTangents[1].y += 8;
                    PointsAndTangents[2].y += 8;
                }
            }

            arrow.MarkDirtyRepaint();
        }

        public override void OnSelected()
        {
            base.OnSelected();
            arrow.MarkDirtyRepaint();
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            arrow.MarkDirtyRepaint();
        }

        public Vector2[] GetPoints()
        {
            return PointsAndTangents;
        }
    }
}
