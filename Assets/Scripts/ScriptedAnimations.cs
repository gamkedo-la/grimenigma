using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedAnimations : MonoBehaviour
{
    public void Bob(float speed, float x=0, float y=0, float z=0)
    {
        StartCoroutine(RunBob(speed, x, y, z));
    }

    public void Rotate(float x=0, float y=0, float z=0)
    {
        StartCoroutine(RunRotate(x, y ,z));
    }

    public void Twean(float speed, float x=0, float y=0, float z=0)
    {
        StartCoroutine(RunMoveTowards(speed, x, y, z));
    }

    public void Twean(float speed, Vector3 targetPosition)
    {
        StartCoroutine(RunMoveTowards(speed, targetPosition));
    }

    IEnumerator RunBob(float speed, float x=0, float y=0, float z=0)
    {
        Vector3 originPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 targetPosition = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z);

        // while true (oooh scary)
        while(true){
            if(Vector3.Distance(transform.position, targetPosition) < 0.1f){ 
                //Debug.Log("Bobbing to original location: " + originPosition + "!");
                yield return StartCoroutine(RunMoveTowards(speed, originPosition));
            }
            else if(Vector3.Distance(transform.position, originPosition) < 0.1f){
                //Debug.Log("Bobbing to target location: " + targetPosition + "!");
                yield return StartCoroutine(RunMoveTowards(speed, targetPosition));
            }
            yield return null;
        }
    }

    IEnumerator RunMoveTowards(float speed, float x=0, float y=0, float z=0)
    {
        Vector3 targetPosition = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z);
        while(Vector3.Distance(transform.position, targetPosition) > 0.1f){
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            //Debug.Log("New position: " + transform.position);
            yield return null;
        }

        yield return null;
    }

    IEnumerator RunMoveTowards(float speed, Vector3 targetPosition)
    {
        while(Vector3.Distance(transform.position, targetPosition) > 0.1f){
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            //Debug.Log("New position: " + transform.position);
            yield return null;
        }
    }

    IEnumerator RunRotate(float x=0, float y=0, float z=0)
    {
        // while true (oooh scary)
        while(true){
            transform.Rotate(x * Time.deltaTime, y * Time.deltaTime, z * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
