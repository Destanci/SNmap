using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSpheres : MonoBehaviour
{
    public GameObject ColliderEdgePrefab;
    public List<GameObject> BottomSpheres = new List<GameObject>();

    private void Awake()
    {
        SetColliderSpheres();
    }
    private void SetColliderSpheres()
    {
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();

        float bottom = capsule.bounds.center.y - capsule.bounds.extents.y;
        //float top = capsule.bounds.center.y + capsule.bounds.extents.y;
        float front = capsule.bounds.center.z + capsule.bounds.extents.z;
        float back = capsule.bounds.center.z - capsule.bounds.extents.z;

        GameObject bottomFront = CreateEdgeSphere(new Vector3(0f, bottom, front));
        GameObject bottomBack = CreateEdgeSphere(new Vector3(0f, bottom, back));

        bottomFront.transform.parent = this.transform;
        bottomBack.transform.parent = this.transform;

        BottomSpheres.Add(bottomFront);
        BottomSpheres.Add(bottomBack);

        float horSec = (bottomFront.transform.position - bottomBack.transform.position).magnitude / 5f;
        CreateMiddleSphers(bottomFront, -this.transform.forward, horSec, 4, BottomSpheres);
    }

    public void CreateMiddleSphers(GameObject start, Vector3 dir, float sec, int interations, List<GameObject> spheresList)
    {
        for (int i = 0; i < interations; i++)
        {
            Vector3 pos = start.transform.position + (dir * sec * (i + 1));

            GameObject newObj = CreateEdgeSphere(pos);
            newObj.transform.parent = this.transform;
            spheresList.Add(newObj);
        }
    }

    GameObject CreateEdgeSphere(Vector3 pos)
    {
        GameObject obj = Instantiate(ColliderEdgePrefab, pos, Quaternion.identity);
        return obj;
    }
}
