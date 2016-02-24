using UnityEngine;
using System.Collections;

public class OwlScript : MonoBehaviour {
    public Transform owlTarget;
    public float owlSpeed;

    public GameObject[] planetTargets;

    private float stuckTimer;
    private Vector3 previousPos;

    void Start () {
        planetTargets = GameObject.FindGameObjectsWithTag("planet");
        FindNewTarget();
    }

	void Update () {
        Vector3 direction = owlTarget.position - gameObject.transform.position;
        direction.Normalize();
        GetComponent<Rigidbody2D>().velocity = direction * owlSpeed;
        
        //Debug.Log(GetComponent<Rigidbody2D>().velocity.magnitude);
    }

    void FixedUpdate()
    {
        previousPos = gameObject.transform.position;

    }

    public void FindNewTarget()
    {
        owlTarget = planetTargets[Random.Range(0, planetTargets.Length)].transform;
    }

    void Unsticker()
    {
        //really bad way to detect if stuck
        if (this.transform.position == previousPos)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer > 1.5f)
            {
                FindNewTarget();
                stuckTimer = 0;
            }
        }
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "planet")
        {
            FindNewTarget();
        }
    }
}
