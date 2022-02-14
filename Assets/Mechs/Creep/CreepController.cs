using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CreepController : MonoBehaviour
{
    [SerializeField] GameObject[] destList;
    [SerializeField] GameObject watchPoint;
    [SerializeField] bool useRandomDest; //f
    [SerializeField] float roamWaitTime; //5
    [SerializeField] string currState;
    [SerializeField] int currDest;
    [SerializeField] Vector3 dest;
    string[] states = {"idle", "roaming", "sus", "aggro"};
    GameObject player;
    NavMeshAgent agent;
    RaycastHit targetHit;
    bool rayTargetShot;
    Color stateColor;
    [SerializeField] float aggroSpoolUp = 0f;
    [SerializeField] float aggroSpoolMax; //200

    void Start() {
        currState = states[1]; //0
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("PlayerCamera");
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

        if(targetHit.collider.gameObject.GetComponent<SC_FPSController>()) { //check if player is visible for creep WHEN TO AGGRO
            if ((cpAngle < 105f)) { //check angle player <=> creep
                aggroSpoolUp += Time.deltaTime * 100f;
                if(aggroSpoolUp >= aggroSpoolMax) {
                    aggroSpoolUp = aggroSpoolMax;
                    currState = states[3];
                    dest = player.transform.position;
                }
            } 
        } else if (cpDist < 7.5f) { //check dist player <=> creep
            aggroSpoolUp = aggroSpoolMax;
            dest = player.transform.position;
            currState = states[3]; 
        } else if (currState == states[3] && aggroSpoolUp <= 0f) { // WHEN TO DEAGGRO
            dest = player.transform.position;
            currState = states[1];
            if (Vector3.Distance(watchPoint.transform.position, dest) < 0.5f) { 
                // ...
            }
        } else if (aggroSpoolUp > 0) {
            dest = player.transform.position;
            aggroSpoolUp -= Time.deltaTime * 10f;
        }

    }
    // TODO dropAggroPoint add to dest list and clear after visit. add serialized initdestlength, in Start() init. check if dest is out of it when pop

    void Idle() {
        
    }

    void Roam() {
        dest = destList[currDest].transform.position;
        agent.SetDestination(dest);
        if(Vector3.Distance(watchPoint.transform.position, dest) < 1.5f) {
            StartCoroutine(IdleWhileRoaming());
            if(!useRandomDest) {
                if(currDest < destList.Length - 1) {
                    currDest++;
                } else {
                    currDest = 0;
                }
            } else {
                currDest = Random.Range(0, destList.Length);
            }
        }
    }

    IEnumerator IdleWhileRoaming() {
        Debug.Log(1);
        currState = states[0];
        yield return new WaitForSeconds(roamWaitTime);
        Debug.Log(2);
        if(aggroSpoolUp <= 0f) {
            currState = states[1];
        }
        yield return null;
    }

    void Aggro() {
        StopCoroutine(IdleWhileRoaming());
        agent.SetDestination(dest);
    }

}
