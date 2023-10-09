using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : NetworkBehaviour
{
    [Networked] TickTimer timer { get; set; }

    public GameObject[] Circle0;
    public GameObject[] Circle1;
    public GameObject[] Circle2;
    public GameObject[] Circle3;
    public GameObject[] Circle4;
    public GameObject[] Floors;

    private int CircleCount = 0;
    private GameObject[][] Circles;

    private void OnEnable()
    {
        InitCircles();
        timer = TickTimer.CreateFromSeconds(Runner,30f);
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if (timer.Expired(Runner))
        {
            if (CircleCount < Circles.Length)
            {
                DecayArena(CircleCount);
                CircleCount++;
            }

            timer = TickTimer.None;
            timer = TickTimer.CreateFromSeconds(Runner, 30f);
        }
    }

    private void InitCircles()
    {
        Circles = new GameObject[5][];
        Circles[0] = Circle0;
        Circles[1] = Circle1;
        Circles[2] = Circle2;
        Circles[3] = Circle4;
        Circles[4] = Circle3;
    }

    private void DecayArena(int index)
    {
        foreach (GameObject obj in Circles[index])
        {
            StartCoroutine(MoveObjectDown(obj));
        }
        StartCoroutine(MoveObjectDown(Floors[index]));
    }

    private IEnumerator MoveObjectDown(GameObject obj)
    {
        var objPosition = obj.transform.position;
        var targetPosition = objPosition + new Vector3(0, -5, 0);
        float speed = UnityEngine.Random.Range(.1f, .2f);

        while (objPosition.y > targetPosition.y)
        {
            obj.transform.position = Vector3.MoveTowards(objPosition, targetPosition, speed * Runner.DeltaTime);

            objPosition = obj.transform.position;
            yield return new WaitForEndOfFrame();
        }
        
    }
}
