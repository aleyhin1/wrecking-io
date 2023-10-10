using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : NetworkBehaviour
{
    public GameObject[] Circle0;
    public GameObject[] Circle1;
    public GameObject[] Circle2;
    public GameObject[] Circle3;
    public GameObject[] Circle4;
    public GameObject[] Floors;

    [Networked] private TickTimer timer { get; set; }

    private GameObject[][] _circles;
    private int _circleCount = 0;
    private float _decayDelayInSeconds = 30f;


    private void OnEnable()
    {
        InitCircles();

        timer = TickTimer.CreateFromSeconds(Runner,_decayDelayInSeconds);
    }

    private void InitCircles()
    {
        _circles = new GameObject[5][];
        _circles[0] = Circle0;
        _circles[1] = Circle1;
        _circles[2] = Circle2;
        _circles[3] = Circle4;
        _circles[4] = Circle3;
    }
    

    // Decays one circle of the arena for every _decayDelayInSeconds seconds
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (timer.Expired(Runner))
        {
            if (_circleCount < _circles.Length)
            {
                DecayArena(_circleCount);

                _circleCount++;
            }

            timer = TickTimer.None;
            timer = TickTimer.CreateFromSeconds(Runner, _decayDelayInSeconds);
        }
    }

    // Moves the arena circle and corresponding floor collider downwards
    private void DecayArena(int index)
    {
        foreach (GameObject obj in _circles[index])
        {
            StartCoroutine(MoveObjectDown(obj));
        }

        StartCoroutine(MoveObjectDown(Floors[index]));
    }

    private IEnumerator MoveObjectDown(GameObject obj)
    {
        var objPosition = obj.transform.position;
        var targetPosition = objPosition + new Vector3(0, -5, 0);
        float speed = Random.Range(.1f, .2f);

        while (objPosition.y > targetPosition.y)
        {
            obj.transform.position = Vector3.MoveTowards(objPosition, targetPosition, speed * Runner.DeltaTime);
            objPosition = obj.transform.position;

            yield return new WaitForEndOfFrame();
        }
        
    }
}
