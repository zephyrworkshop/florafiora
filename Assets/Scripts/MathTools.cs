using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MathTools{
	public static int randomSeedForGame = 82;//random
	public static int intSize=976896;//2147483647

	public static int calculateSeedFromPosition(Vector3 position){
		int result = 0;
		var prevSeed = Random.seed;
		Random.seed = Mathf.FloorToInt(position.x);
		result += Random.Range (0, intSize);
		Random.seed = Mathf.FloorToInt(position.y);
		result += Random.Range (0, intSize);
		Random.seed = Mathf.FloorToInt(position.z);
		result += Random.Range (0, intSize);
		Random.seed=randomSeedForGame;
		result += Random.Range (0, intSize);
		Random.seed = prevSeed;

		return result;
	}

	public static Vector3 rayPlaneIntersection(Ray ray, Vector3 planeNormal, Vector3 planePoint){
		Vector3 displacement = (planePoint-ray.origin);//displacement from ray origin to plane
		float dist = Vector3.Dot(planeNormal.normalized, displacement);//normalized projection of the displacement along the direction of the normal
		float proj = Vector3.Dot(planeNormal.normalized, ray.direction.normalized);//normalized projection of the ray direction along the direction of the normal
		float scaleFactor = dist / proj;//the number of directions needed to travel to the plane
		Vector3 toPlane = ray.direction.normalized*scaleFactor;//the vector from the ray origin to th plane
		return ray.origin+toPlane; //the point on the plane
	}

	public static float getMin(float x, float y){
		if(x<y){
			return x;
		}
		else{
			return y;
		}
	}

	public static float getMax(float x, float y){
		if (x > y) {
			return x;
		}
		else{
			return y;
		}
	}

	public static Vector3 bSpline(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t){
		//Vector3 pp0 = p0 + t * (p1 - p0);
		//Vector3 pp1 = p1 + t * (p2 - p1);
		//Vector3 pp2 = p2 + t * (p3 - p2);
		//Vector3 ppp0 = pp0 + t * (pp1-pp0);
		//Vector3 ppp1 = pp1 + t * (pp2-pp1);
		//return ppp0 + t * (ppp1-ppp0);
		//which simplifies to
		Vector3 val = p0+3*(p1-p0)*t+(3*p2-6*p1+3*p0)*t*t+(p3-3*p2+3*p1-p0)*t*t*t;
		if (float.IsNaN (val.x) || float.IsNaN (val.y) || float.IsNaN (val.z))
			return Vector3.zero;
		return val;
	}
	
	public static Vector3 bSplineDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t){
		return 3*(p1-p0)+(6*p2-12*p1+6*p0)*t+3*(p3-3*p2+3*p1-p0)*t*t;
	}

	public static Vector3 bSplineSecondDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t){
		return (6*p2-12*p1+6*p0)+6*(p3-3*p2+3*p1-p0)*t;
	}

	public static Vector3 linearInterpolate(Vector3 current, Vector3 target, float t){
		return current * (1 - t) + target * t;
	}

	public static float calculateAngleBetween(Vector3 start, Vector3 middle, Vector3 end){
		return calculateAngleBetween (start, middle, end, Vector3.up);
	}

	public static float calculateAngleBetween(Vector3 start, Vector3 middle, Vector3 end, Vector3 up){
		Vector3 s1 = (start - middle).normalized;
		Vector3 s2 = (end - middle).normalized;
		float s2tos1proj = Vector3.Dot (s2, s1);
		float s2toPerpProj = (s2 - FindDirectionalComponent(s1,s2)).magnitude;
		if(Vector3.Dot (Vector3.Cross (s1,s2), up)>=0){
			return Mathf.Atan (s2toPerpProj / s2tos1proj);
		}
		else{
			return Mathf.PI*2-Mathf.Atan (s2toPerpProj / s2tos1proj);
		}
	}

	public static Vector3 projectToPlane(Vector3 planarVec1, Vector3 planarVec2, Vector3 vec){
		var norm = Vector3.Cross (planarVec1, planarVec2).normalized;
		norm = norm * Vector3.Dot (vec, norm);
		vec-=norm;
		return vec; 
	}

	public static float CalculateAngleOfPlanarProjection(Vector3 planarVec1, Vector3 planarVec2, Vector3 vec1, Vector3 vec2){
		return Vector3.Angle (projectToPlane (planarVec1,planarVec2,vec1), projectToPlane (planarVec1,planarVec2,vec2));
	}

	public static Vector3 projectToXZPlane(Vector3 vec){
		vec.y = 0;
		return vec;
	}

	public static float CalculateAngleOfXZProjection(Vector3 vec1, Vector3 vec2){
		return Vector3.Angle (projectToXZPlane (vec1), projectToXZPlane (vec2));
	}

	public static float DampedHarmonic(float t){
		//returns a value that ramps from 0 to 1 then oscilates until it settles at 1
		return 1-(Mathf.Cos (t*Mathf.PI*8))*Mathf.Pow(.5f,t*8);
	}

	public static float ExponentialApproach(float t){
		return Mathf.Pow (2,8*t)/256f;
	}

	public static Vector3 FindDirectionalComponent(Vector3 direction, Vector3 vector){
		return direction.normalized * Vector3.Dot (direction, vector);
	}

	public static Vector3 RemoveDirectionalComponent(Vector3 direction, Vector3 vector){
		return vector-FindDirectionalComponent(direction,vector);
	}
	
	public static Vector3 ProjectOntoSphere(Vector3 sphereCenter, float sphereRadius, Vector3 point){
		return (point-sphereCenter).normalized*sphereRadius+sphereCenter;
	}
	
	public static Vector3 RotateAround(Vector3 center, Vector3 axis, float angle, Vector3 point){
		var rot=Quaternion.AngleAxis (angle, axis);
		var result = point - center;
		result = rot*result;
		result += center;
		return result;
	}

	public static Vector3 ScreenToWorldPosition(Vector3 screenPosition){
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
		worldPos.z = 0;
		return worldPos;
	}
}
