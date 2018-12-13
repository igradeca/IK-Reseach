using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmLogicScript : MonoBehaviour {

    public GameObject prefabArmSegment;
    public List<GameObject> armSegments;

    public Vector3 startPosition;
    public Vector3 startRotation;

    public float armLength;
    public int numberOfSegments;

    Ray ray;

    // Use this for initialization
    void Start () {

        armSegments = new List<GameObject>();
        startPosition = new Vector3(-4, -2);
        startRotation = new Vector3(0.0f, 0.0f, 0.0f);

        SetInitialSegment();
    }

    public void SetInitialSegment() {

        GameObject tempSegment = Instantiate(prefabArmSegment, startPosition, Quaternion.Euler(startRotation));
        tempSegment.GetComponent<SegmentScript>().startPoint = tempSegment.transform.position;
        tempSegment.name = "Segment " + 0;

        armSegments.Add(tempSegment);
    }

    void Update() {
        //Input.GetButtonDown("Fire1")
        if (Input.GetButton("Fire1") && !PointerOverUI()) {

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            MoveSegmentsToTarget();
            //Debug.Log(ray.origin);
        }

        if (Input.GetButtonDown("Fire2")) {

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            GameObject.Find("Dot").transform.position = ray.origin;
            //Debug.Log("Dot loc: " + ray.origin);
        }
    }

    public bool PointerOverUI() {

        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    private void MoveSegmentsToTarget() {

        Vector3 target = new Vector3(ray.origin.x, ray.origin.y, 0.0f);
        armSegments[armSegments.Count - 1].GetComponent<SegmentScript>().MoveSegment(target);
        armSegments[armSegments.Count - 1].GetComponent<SegmentScript>().SetSegmentEndPoint();
        
        for (int i = armSegments.Count - 2; i >= 0; i--) {
            armSegments[i].GetComponent<SegmentScript>().MoveSegment(armSegments[i + 1].GetComponent<SegmentScript>().startPoint);
            armSegments[i].GetComponent<SegmentScript>().SetSegmentEndPoint();
        }

        armSegments[0].GetComponent<SegmentScript>().startPoint = startPosition;

        for (int i = 1; i < armSegments.Count; i++) {
            armSegments[i - 1].GetComponent<SegmentScript>().SetSegmentEndPoint();
            armSegments[i].GetComponent<SegmentScript>().startPoint = armSegments[i - 1].GetComponent<SegmentScript>().endPoint;
        }
        
        // drawing
        for (int i = 0; i < armSegments.Count; i++) {
            armSegments[i].transform.position = armSegments[i].GetComponent<SegmentScript>().startPoint;
            armSegments[i].GetComponent<SegmentScript>().UpdateSegmentAngle();
        }        
    }

    private float CalculateError() {

        return Vector3.Distance(armSegments[(armSegments.Count - 1)].GetComponent<SegmentScript>().endPoint, ray.origin);
    }

    public void SetNewArmLenghts() {

        for (int i = 0; i < armSegments.Count; i++) {
            if (i > 0) {
                armSegments[i].transform.position = armSegments[i - 1].GetComponent<SegmentScript>().endPoint;
            }
            armSegments[i].GetComponent<SegmentScript>().SetArmLength();
            armSegments[i].GetComponent<SegmentScript>().SetSegmentEndPoint();
        }
    }

    public void SetSegments() {

        if (armSegments.Count > numberOfSegments) {
            DestroySegments();
        } else if (armSegments.Count < numberOfSegments) {
            AddSegments();
        }
    }

    private void DestroySegments() {

        for (int i = numberOfSegments; i < armSegments.Count; i++) {
            Destroy(armSegments[i]);            
        }
        for (int i = numberOfSegments; i < armSegments.Count; i++) {
            armSegments.RemoveAt(i);
        }
    }

    private void AddSegments() {

        for (int i = (armSegments.Count); i < numberOfSegments; i++) {
            AddNewSegment(i);
        }
    }

    private void AddNewSegment(int segmentNumber) {

        GameObject tempSegment = Instantiate(prefabArmSegment);

        tempSegment.transform.position = armSegments[armSegments.Count - 1].GetComponent<SegmentScript>().endPoint;
        tempSegment.name = "Segment " + segmentNumber;

        armSegments.Add(tempSegment);
    }


}
