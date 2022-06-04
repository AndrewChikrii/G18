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
    [SerializeField] float roamWaitTime = 3f;
    [SerializeField] float aggroDist = 7.5f;
    [SerializeField] float aggroAngle = 105f;
    public string currState;
    [SerializeField] int currDestIndex;
    [SerializeField] Vector3 dest;
    string[] states = { "idle", "roaming", "sus", "aggro" };
    GameObject player;
    NavMeshAgent agent;
    [SerializeField] float agentSpeedNormal = 2;
    [SerializeField] float agentSpeedTurbo = 3;
    RaycastHit targetHit;
    bool rayTargetShot;
    Color stateColor = Color.white;
    public float aggroSpoolUp = 0f;
    [SerializeField] float aggroSpoolMax = 100f;
    [SerializeField] float aggroSpoolUpModifier = 0.5f;
    [SerializeField] float aggroSpoolDownModifier = 0.25f;

    Animator anim;

    public RenderTexture lightCheckTexture;
    public float lightLevel;

    void Start()
    {
        anim = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
        currState = states[1];
        agent = GetComponent<NavMeshAgent>();
        agent.speed = agentSpeedNormal;
        player = GameObject.Find("PlayerCamera");
        foreach (GameObject g in inputDestList)
        {
            destList.Add(g.transform.position);
        }
    }

    void FixedUpdate()
    {
        Debug.DrawRay(watchPoint.transform.position, watchPoint.transform.TransformDirection(Vector3.forward) * 10f, Color.grey);

        HandleStates();

        switch (currState)
        {
            case "idle":
                stateColor = Color.blue;
                break;
            case "roaming":
                stateColor = Color.white;
                Roam();
                break;
            case "sus":
                stateColor = Color.yellow;
                break;
            case "aggro":
                stateColor = Color.red;
                Aggro();
                break;
        }
    }

    void HandleStates()
    {
        // LIGHT LEVEL CHECKS
        RenderTexture tmpTexture = RenderTexture.GetTemporary(lightCheckTexture.width, lightCheckTexture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        Graphics.Blit(lightCheckTexture, tmpTexture);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = tmpTexture;
        Texture2D temp2DTexture = new Texture2D(lightCheckTexture.width, lightCheckTexture.height);
        temp2DTexture.ReadPixels(new Rect(0, 0, tmpTexture.width, tmpTexture.height), 0, 0);
        temp2DTexture.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(tmpTexture);
        Color32[] colors = temp2DTexture.GetPixels32();
        Destroy(temp2DTexture);
        lightLevel = 0;
        for (int i = 0; i < colors.Length; i++)
        {
            lightLevel += ((0.2126f * colors[i].r) + (0.7152f * colors[i].g) + (0.0722f * colors[i].b)) / 10000;
        }
        if (lightLevel > 1)
        {
            aggroSpoolUpModifier = 1f;
        }
        else
        {
            aggroSpoolUpModifier = 0.5f;
        }

        Vector3 cpDir = (player.transform.position - watchPoint.transform.position).normalized;
        float cpDist = Vector3.Distance(player.transform.position, watchPoint.transform.position);
        float cpAngle = Vector3.Angle(cpDir, watchPoint.transform.TransformDirection(Vector3.forward));

        Debug.DrawRay(watchPoint.transform.position, cpDir * cpDist, stateColor);
        rayTargetShot = Physics.Raycast(watchPoint.transform.position, cpDir, out targetHit, 100f);

        Component playerCheckComp = null;
        try
        {
            playerCheckComp = targetHit.collider.gameObject.GetComponent<SC_FPSController>();
        }
        catch { }

        if (targetHit.collider.gameObject.GetComponent<SC_FPSController>())
        {
            if ((cpAngle < aggroAngle))
            {
                aggroSpoolUp += Time.deltaTime * 100f * aggroSpoolUpModifier;
                if (aggroSpoolUp >= aggroSpoolMax)
                {
                    aggroSpoolUp = aggroSpoolMax;
                    currState = states[3];
                    dest = player.transform.position;
                }
            }
        }
        else if (currState == states[3] && aggroSpoolUp <= 0f)
        {
            destList.Add(transform.position);
            currState = states[1];
            if (anim.GetBool("run") == true)
            {
                anim.SetBool("run", false);
                anim.SetTrigger("walk");
            }
        }
        else if (aggroSpoolUp > 0)
        {
            dest = player.transform.position;
            aggroSpoolUp -= Time.deltaTime * 100f * aggroSpoolDownModifier;
        }
        if (cpDist < aggroDist)
        {
            aggroSpoolUp = aggroSpoolMax;
            dest = player.transform.position;
            currState = states[3];
        }
    }

    void Roam()
    {
        dest = destList[currDestIndex];
        agent.speed = agentSpeedNormal;
        agent.SetDestination(dest);
        if (Vector3.Distance(watchPoint.transform.position, dest) < 1.5f)
        {
            StartCoroutine(IdleWhileRoaming());
            if (!useRandomDest)
            {
                if (currDestIndex < destList.Count - 1)
                {
                    currDestIndex++;
                }
                else
                {
                    currDestIndex = 0;
                }
            }
            else
            {
                currDestIndex = Random.Range(0, destList.Count);
            }
        }
    }

    IEnumerator IdleWhileRoaming()
    {
        anim.ResetTrigger("walk");
        anim.SetTrigger("idle");
        if (forgetExtraDests && currDestIndex > inputDestList.Length - 1)
        {
            destList.RemoveAt(currDestIndex);
        }
        currState = states[0];
        yield return new WaitForSeconds(roamWaitTime);
        if (currState == states[0] || aggroSpoolUp <= 0f)
        {
            aggroSpoolUp = 0f;
            currState = states[1];
        }
        anim.ResetTrigger("idle");
        anim.SetTrigger("walk");
        yield return null;
    }

    void Aggro()
    {
        StopCoroutine(IdleWhileRoaming());
        anim.ResetTrigger("idle");
        anim.ResetTrigger("walk");

        agent.speed = agentSpeedTurbo;
        agent.SetDestination(dest);

        if (anim.GetBool("run") == false)
        {
            anim.SetBool("run", true);
        }
    }
}