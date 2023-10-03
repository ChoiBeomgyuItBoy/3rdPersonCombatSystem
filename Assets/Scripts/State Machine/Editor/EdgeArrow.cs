using UnityEngine;
using UnityEngine.UIElements;

namespace CombatSystem.StateMachine.Editor
{
    public class EdgeArrow : VisualElement
    {   
        StateEdge edge;

        public EdgeArrow(StateEdge edge)
        {
            this.edge = edge;
            generateVisualContent += OnGenerateVisualContent;
        }

        private void OnGenerateVisualContent(MeshGenerationContext ctx)
        {
            if (edge.GetPoints().Length < 2)
            {
                return;
            }

            Color color = GetEdgeColor();

            Vector2 start = edge.GetPoints()[edge.GetPoints().Length / 2 - 1];
            Vector2 end = edge.GetPoints()[edge.GetPoints().Length / 2];
            Vector2 lineDirection = end - start;
            Vector2 midPoint = (start + end) / 2;

            float arrowEdgeLength = 12f;
            float distanceFromMiddle = arrowEdgeLength * Mathf.Sqrt(3) / 4;
            float angle = Vector2.SignedAngle(Vector2.right, lineDirection);
            float perpendicularLength = arrowEdgeLength / (Mathf.Sin(Mathf.Deg2Rad * (angle - 60)) * 2);

            if (angle < 60 && angle > 0)
            {
                perpendicularLength = arrowEdgeLength / (Mathf.Sin(Mathf.Deg2Rad * (angle + 120)) * 2);
            }
            else if (angle > -120 && angle < -60)
            {
                perpendicularLength = arrowEdgeLength / (Mathf.Sin(Mathf.Deg2Rad * (angle - 120)) * 2);
            }
            else if (angle > -60 && angle < 0)
            {
                perpendicularLength = arrowEdgeLength / (Mathf.Sin(Mathf.Deg2Rad * (angle + 60)) * 2);
            }

            Vector2 perpendicular = new Vector2(-lineDirection.y, lineDirection.x).normalized * perpendicularLength;

            MeshWriteData mesh = ctx.Allocate(3, 3);
            Vertex[] vertices = new Vertex[3];
            vertices[0].position = midPoint + (lineDirection.normalized * distanceFromMiddle);
            vertices[1].position = midPoint + (-lineDirection.normalized * distanceFromMiddle) + (perpendicular.normalized * arrowEdgeLength / 2);
            vertices[2].position = midPoint + (-lineDirection.normalized * distanceFromMiddle) + (-perpendicular.normalized * arrowEdgeLength / 2);

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].position += Vector3.forward * Vertex.nearZ;
                vertices[i].tint = color;
            }

            mesh.SetAllVertices(vertices);
            mesh.SetAllIndices(new ushort[] { 0, 1, 2 });
        }

        private Color GetEdgeColor()
        {
            if (edge.isGhostEdge) 
            {
                return edge.ghostColor;
            }
            else if (edge.selected) 
            {
                return edge.selectedColor;
            }
            else 
            {
                return edge.defaultColor;
            }
        }
    }
}
