using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VineDragComponent : MonoBehaviour {

	static VineComponent vine;

	public GameObject linePrefab;

	static LineRenderer line1;

	static VineDragComponent instance;

	private static bool dragging = false;

	private static List<Vector3> startDragPositions = new List<Vector3> ();
	private static List<float> vinePercentagePositions = new List<float> ();
	private static List<Vector3> releasePositions = new List<Vector3>();
	private static Vector3 clickStartPosition;
	private static Vector3 clickStopPosition;
	private static float clickStartPercentage;

    private static bool vineCut = false;

	//private static GameObject debugSphere1;
	//private static GameObject debugSphere2;

	// Use this for initialization
	void Start () {
		instance = this;
		//debugSphere1 = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		//debugSphere2 = GameObject.CreatePrimitive (PrimitiveType.Sphere);
	}
	
	// Update is called once per frame
	void Update () {
        if (vineCut == false)
        {
            if (vine != null)
            {
                line1.transform.position = gameObject.transform.position;
                line1.SetPosition(0, vine.ends[0].gameObject.transform.position);
                line1.SetPosition(2, vine.ends[1].gameObject.transform.position);

                var temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                temp.z = 1f;
                line1.SetPosition(1, temp);
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (vine != null)
                {

                    CameraPanningScript.Enable();

                    //show the vine
                    vine.gameObject.GetComponent<SpriteRenderer>().enabled = true;

                    //turn collisions on again
                    vine.colliderOnAgainTime = Time.time + .5f;

                    //destroy the lines
                    GameObject.Destroy(line1);
                    line1 = null;

                    Vector3 dir = vine.gameObject.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    OnDragRelease(dir);

                    //Debug.Log ("Releasing vine: " + vine.gameObject.name + " at dir: " + dir);

                    vine = null;
                }
            }


            if (dragging && vine != null)
            {
                var mP = ClickToDrag.GetCursorWorldLocation();

                var pp1 = vine.ends[0].gameObject.transform.position;

                var pp2 = vine.ends[1].gameObject.transform.position;

                for (int i = 0; i < vine.seedizens.Count; ++i)
                {
                    var seedizen = vine.seedizens[i];
                    if (seedizen == null || seedizen.gameObject == null)
                    {
                        continue;
                    }
                    if (i >= vinePercentagePositions.Count)
                    {
                        Debug.Log("The system of coordinating positions on lists stored on multiple classes has failed!!! Consider storing this data on the seedizens");
                        continue;
                    }
                    var perc = vinePercentagePositions[i];
                    if (perc < clickStartPercentage)
                    {
                        var halfPer = perc / clickStartPercentage;
                        //Debug.Log ("proportional percentage: "+halfPer);
                        seedizen.transform.position = halfPer * (mP - pp1) + pp1;

                    }
                    else {
                        var halfPer = (perc - clickStartPercentage) / (1 - clickStartPercentage);
                        //Debug.Log ("proportional percentage past cursor: "+halfPer);
                        seedizen.transform.position = halfPer * (pp2 - mP) + mP;

                    }
                }

                UpdateFlingArrow();
            }
        }
	}

    public static void StartVineDrag(VineComponent vc)
    {
        if (vineCut == false)
        {
            CameraPanningScript.Disable();
            vine = vc;

            dragging = true;
            clickStartPosition = ClickToDrag.GetCursorWorldLocation();
            //var sph = GameObject.CreatePrimitive (PrimitiveType.Sphere);
            //sph.transform.position = clickStartPosition;


            startDragPositions.Clear();
            vinePercentagePositions.Clear();
            releasePositions.Clear();
            var vineVector = vine.ends[1].gameObject.transform.position - vine.ends[0].gameObject.transform.position;
            clickStartPercentage = (clickStartPosition - vine.ends[0].gameObject.transform.position).magnitude / vineVector.magnitude;
            //Debug.Log ("click percentage: " + clickStartPercentage);
            foreach (var seedizen in vine.seedizens)
            {
                if (seedizen == null || seedizen.gameObject == null)
                {
                    Debug.Log("null seedizen");
                    continue;
                }
                seedizen.inTransit = false;
                startDragPositions.Add(seedizen.transform.position);
                vinePercentagePositions.Add((seedizen.transform.position - vine.ends[0].gameObject.transform.position).magnitude / vineVector.magnitude);
            }

            //create some lines
            var l1 = GameObject.Instantiate(instance.linePrefab) as GameObject;
            line1 = l1.GetComponent<LineRenderer>();

            //hide the vine
            vc.gameObject.GetComponent<SpriteRenderer>().enabled = false;

            //turn off collisions
            vc.gameObject.GetComponent<Collider2D>().enabled = false;

            MakeFlingArrow();
        }
    }

    void OnDragRelease(Vector3 dir)
    {
        if (vineCut == false)
        {
            releasePositions.Clear();
            clickStopPosition = ClickToDrag.GetCursorWorldLocation();
            List<SeedizenComponent> seedizensCopy = new List<SeedizenComponent>(vine.seedizens);
            for (int i = 0; i < seedizensCopy.Count; i++)
            {
                var seedizen = seedizensCopy[i];
                if (seedizen == null || seedizen.gameObject == null)
                {
                    Debug.Log("Null seedizen");
                    continue;
                }
                releasePositions.Add(seedizen.transform.position);
                //debugSphere1.transform.position=releasePositions[i];
                //debugSphere2.transform.position=startDragPositions[i];
                seedizen.temporarilyDisableCollider(.5f);
                if (i >= startDragPositions.Count || i >= releasePositions.Count)
                {
                    Debug.Log("The system of coordinating positions on lists stored on multiple classes has failed!!! Consider storing this data on the seedizens");
                    continue;
                }
                seedizen.StartFlight((startDragPositions[i] - releasePositions[i]));//still need to make the falloff for this greater than linear

            }
            vine.seedizens.Clear();
            dragging = false;

            DestroyFlingArrow();
        }
    }

	static GameObject flingArrow;

    static void MakeFlingArrow()
    {
        if (vineCut == false)
        {
            flingArrow = GameObject.Instantiate(Resources.Load<GameObject>("FlingArrow")) as GameObject;
            flingArrow.transform.position = clickStartPosition;
            UpdateFlingArrow();
        }
    }

    static void UpdateFlingArrow()
    {
        if (vineCut == false)
        {
            if (flingArrow == null)
            {
                Debug.Log("Fling arrow is null");
                return;
            }

            //also clickStartPosition
            Vector3 pos = ClickToDrag.GetCursorWorldLocation();
            Vector3 differenceVector = clickStartPosition - pos;

            float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg - 90f;
            flingArrow.transform.rotation = Quaternion.Euler(0, 0, angle);

            flingArrow.transform.localScale = Vector3.one * (differenceVector.magnitude / 4f);
        }
    }

    static void DestroyFlingArrow()
    {
        if (vineCut == false)
        {
            GameObject.Destroy(flingArrow);
        }
    }

    public void VineCut()
    {
        if (vineCut == true)
        {
            vineCut = false;
        }
        else
        {
            vineCut = true;
        }
    }
}
