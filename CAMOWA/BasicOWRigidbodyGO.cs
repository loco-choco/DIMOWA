using UnityEngine;

namespace CAMOWA
{
    //public class BasicOWRigidbodyGO
    //{

    //    //The Simplest Game Object that you can make from a Cube Primitive
    //    public static GameObject SimplestBoxOWObject(Vector3 cubeSize, float colliderSize = 0.5f)
    //    {
    //        GameObject simplestGO = GameObject.CreatePrimitive(PrimitiveType.Cube);

    //        simplestGO.transform.localScale = cubeSize;
    //        Object.Destroy(simplestGO.GetComponent<BoxCollider>());
    //        simplestGO.AddComponent<Rigidbody>().mass = 1f;
    //        simplestGO.AddComponent<OWRigidbody>();

    //        GameObject sphereMesh = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //        GameObject simplestGODetector = new GameObject();
    //        simplestGODetector.transform.localScale = cubeSize;

    //        simplestGODetector.layer = LayerMask.NameToLayer("BasicEffectVolume");
    //        simplestGODetector.GetComponent<Transform>().parent = simplestGO.transform;
    //        simplestGODetector.GetComponent<Transform>().name = "Detector";
    //        simplestGODetector.AddComponent<SphereCollider>().radius = colliderSize;
    //        simplestGODetector.GetComponent<SphereCollider>().isTrigger = true;
    //        simplestGODetector.AddComponent<MultiFieldDetector>();

    //        GameObject simplestGOCollider = new GameObject();

    //        simplestGOCollider.transform.localScale = cubeSize;

    //        simplestGOCollider.layer = LayerMask.NameToLayer("Primitive");
    //        simplestGOCollider.GetComponent<Transform>().parent = simplestGO.transform;
    //        simplestGOCollider.GetComponent<Transform>().name = "Collider";
    //        simplestGOCollider.AddComponent<BoxCollider>().size = cubeSize;
    //        simplestGOCollider.AddComponent<OWCollider>();
    //        //Destruir a esfera, pois apenas queremos seu mesh filter
    //        Object.Destroy(sphereMesh);
            
    //        return simplestGO;
    //    }
    //}
}
