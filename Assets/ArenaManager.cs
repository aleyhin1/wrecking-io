using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : NetworkBehaviour
{
    [Networked] TickTimer timer { get; set; }
    public GameObject[] Circle0;

    private void OnEnable()
    {
        timer = TickTimer.CreateFromSeconds(Runner,2f);
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if (timer.Expired(Runner))
        {
            DecayArena();

            timer = TickTimer.None;
        }
    }

    private void DecayArena()
    {
        foreach (GameObject obj in Circle0)
        {
            StartCoroutine(MoveObjectDown(obj));
        }
    }

    private IEnumerator MoveObjectDown(GameObject obj)
    {
        var objPosition = obj.transform.position;
        var targetPosition = objPosition + new Vector3(0, -5, 0);
        float speed = Random.Range(.1f, .2f);

        while (objPosition.y > targetPosition.y)
        {
            Debug.Log("I'm in the While");
            obj.transform.position = Vector3.MoveTowards(objPosition, targetPosition, speed * Runner.DeltaTime);

            objPosition = obj.transform.position;
            yield return new WaitForEndOfFrame();
        }
        
    }
}
