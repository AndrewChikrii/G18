using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreepController : MonoBehaviour
{
    [SerializeField] GameObject[] inputDestList;
    [SerializeField] List<Vector3> destList;
    [SerializeField] GameObject watchPoint;
    [SerializeField] bool useRandomDest; 
    [SerializeField] bool forgetExtraDests; 
    [SerializeField] float roamWaitTime; 
    [SerializeField] string currState;
    [SerializeField] int currDestIndex;
    [SerializeField] Vector3 dest;
    string[] states = {"idle", "roaming", "sus", "aggro"};
    GameObject player;
    NavMeshAgent agent;
    RaycastHit targetHit;
    bool rayTargetShot;
    Color stateColor = Color.white;
    [SerializeField] float aggroSpoolUp = 0f;
    [SerializeField] float aggroSpoolMax; //100
    [SerializeField] float aggroSpoolUpModifier; //1
    [SerializeField] float aggroSpoolDownModifier; //.25

    void Start() {
        currState = states[1]; //0
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("PlayerCamera");
        foreach (GameObject g in inputDestList) {
            destList.Add(g.transform.position);
        }
    }

    void FixedUpdate() {
        Debug.DrawRay(watchPoint.transform.position, watchPoint.transform.TransformDirection(Vector3.forward) * 10f, Color.grey);

        HandleStates();
    
        switch(currState) {
            case "idle": //0
                stateColor = Color.blue;
                Idle();
                break;
            case "roaming": //1
                stateColor = Color.white;
                Roam();
                break;
            case "sus": //2
                stateColor = Color.yellow;
                break;
            case "aggro": //3
                stateColor = Color.red;
                Aggro();
                break;
        }
    }

    void HandleStates() {
        Vector3 cpDir = (player.transform.position - watchPoint.transform.position).normalized;
        float cpDist = Vector3.Distance(player.transform.position, watchPoint.transform.position);
        float cpAngle = Vector3.Angle(cpDir, watchPoint.transform.TransformDirection(Vector3.forward));

        Debug.DrawRay(watchPoint.transform.position, cpDir * cpDist, stateColor);
        rayTargetShot = Physics.Raycast(watchPoint.transform.position, cpDir, out targetHit, cpDist);

        Component playerCheckComp = null;
        try {
            playerCheckComp = targetHit.collider.gameObject.GetComponent<SC_FPSController>();
        } catch {}

        if(playerCheckComp) { //check if player is visible for creep 
            if ((cpAngle < 105f)) { //check angle player <=> creep
                aggroSpoolUp += Time.deltaTime * 100f * aggroSpoolUpModifier;
                if(aggroSpoolUp >= aggroSpoolMax) {
                    aggroSpoolUp = aggroSpoolMax;
                    currState = states[3]; //WHEN TO AGGRO (1)
                    dest = player.transform.position;
                }
            }  
        } else if (currState == states[3] && aggroSpoolUp <= 0f) { // WHEN TO DEAGGRO
            destList.Add(transform.position); //add last pos where seen player as dest for future
            currState = states[1];
            if (Vector3.Distance(watchPoint.transform.position, dest) < 1.5f) { 
                // ...
            }
        } else if (aggroSpoolUp > 0) { // keep following until aggro spool drop to 0
            dest = player.transform.position;
            aggroSpoolUp -= Time.deltaTime * 100f * aggroSpoolDownModifier;
        }
        if (cpDist < 7.5f) { //check dist player <=> creep 
            aggroSpoolUp = aggroSpoolMax;
            dest = player.transform.position;
            currState = states[3]; //WHEN TO AGGRO (2)
        }
    }
    // TODO dropAggroPoint add to dest list and clear after visit. add initdestlength, in Start() init. check if dest is out of it when pop

    void Idle() {
        
    }

    void Roam() {
        dest = destList[currDestIndex];
        agent.SetDestination(dest);
        if(Vector3.Distance(watchPoint.transform.position, dest) < 1.5f) {
            StartCoroutine(IdleWhileRoaming());
            if(!useRandomDest) {
                if(currDestIndex < destList.Count - 1) {
                    currDestIndex++;
                } else {
                    currDestIndex = 0;
                }
            } else {
                currDestIndex = Random.Range(0, destList.Count);
            }
        }
    }

    IEnumerator IdleWhileRoaming() {
        if(forgetExtraDests && currDestIndex > inputDestList.Length - 1) {
            destList.RemoveAt(currDestIndex); // clear player generated dest
        }
        currState = states[0];
        yield return new WaitForSeconds(roamWaitTime);
        if(currState == states[0] || aggroSpoolUp <= 0f) {
            aggroSpoolUp = 0f;
            currState = states[1];
        }
        yield return null;
    }

    void Aggro() {
        StopCoroutine(IdleWhileRoaming());
        agent.SetDestination(dest);
    }

}
