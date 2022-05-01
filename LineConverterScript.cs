using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LineConverterScript : MonoBehaviour
{
    [SerializeField] GameObject linePrefab;
    [SerializeField] GameObject dangerLinePrefab;
    [SerializeField] GameObject lineCornerPrefab;

    [Header("Options")]
    [SerializeField] bool isDangerLine;
    [SerializeField] Color initialColor;
    [SerializeField] Color triggeredColor;

    private SpriteShapeRenderer spriteShapeRenderer;
    private SpriteShapeController spriteShapeController;

    private int lineCornerCount;
    private Vector2[] lineCornerPositions;

    private void Awake()
    {
        //Getting Components
        spriteShapeRenderer = GetComponent<SpriteShapeRenderer>();
        spriteShapeController = GetComponent<SpriteShapeController>();

        var lineGroup = new GameObject();
        lineGroup.transform.parent = transform.parent;
        if (!isDangerLine){
            lineGroup.name = "LineGroup";
        }
        else{
            lineGroup.name = "DangerLineGroup";
        }

        lineCornerCount = spriteShapeController.spline.GetPointCount();
        lineCornerPositions = new Vector2[lineCornerCount];
        for (int i = 0; i < lineCornerCount; i++)
        {
            //Getting corner position
            lineCornerPositions[i] = new Vector2(spriteShapeController.spline.GetPosition(i).x / 10, spriteShapeController.spline.GetPosition(i).y / 10);

            //Creating line object
            Vector2 position;
            float scale;
            float rotation;
            if (i>0){
                //Calculating position of line
                position = new Vector2((lineCornerPositions[i - 1].x + lineCornerPositions[i].x) / 2, (lineCornerPositions[i - 1].y + lineCornerPositions[i].y) / 2);

                //Calculating scale of line
                scale = Vector2.Distance(lineCornerPositions[i - 1], lineCornerPositions[i]);

                //Calculating rotation of line
                rotation = Mathf.Rad2Deg * Mathf.Atan2(lineCornerPositions[i].y - lineCornerPositions[i-1].y, lineCornerPositions[i].x - lineCornerPositions[i - 1].x);

                
                if (!isDangerLine)//is not danger line
                {
                    var line = GameObject.Instantiate(linePrefab, position, Quaternion.Euler(0, 0, rotation), lineGroup.transform);
                    line.name = "Line:(" + line.transform.position.x + ", " + line.transform.position.y + ")";
                    line.transform.localScale = new Vector3(scale, line.transform.localScale.y, line.transform.localScale.z);
                    LineScript lineScriptComponent = line.GetComponent<LineScript>();
                    lineScriptComponent.initialColor = initialColor;
                    lineScriptComponent.triggeredColor = triggeredColor;
                }
                else//is danger line
                {
                    var line = GameObject.Instantiate(dangerLinePrefab, position, Quaternion.Euler(0, 0, rotation), lineGroup.transform);
                    line.name = "DangerLine:(" + line.transform.position.x + ", " + line.transform.position.y + ")";
                    line.transform.localScale = new Vector3(scale, line.transform.localScale.y, line.transform.localScale.z);
                }
            }

            //Creating line corner object
            var lineCorner = GameObject.Instantiate(lineCornerPrefab, lineCornerPositions[i], Quaternion.identity, lineGroup.transform);
            lineCorner.name = "LineCorner:(" + lineCornerPositions[i].x + ", " + lineCornerPositions[i].y + ")";
        }

        //Destroying this Mock Line
        Destroy(gameObject);
    }
}
