using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

using Vector3 = UnityEngine.Vector3;

public class Shuffling : MonoBehaviour
{
    public SplineContainer[] splinesLeft;
    public SplineContainer[] splinesMid;
    public SplineContainer[] splinesRight;

    public GameObject LeftCup;
    public GameObject RightCup;
    public GameObject MidCup;
    public GameObject ball;


    private AudioSource sfx;

    public GameObject RealCup;

    public float moveSpeed = 5f;
    public float moveDistance = 5f;
    public Button gate;
    public GameObject popPU;

    public UILogin iLogin;

    bool flag = true;
    int randomIndex = 0;

    private Dictionary<SplineContainer, SplineContainer> splinePair = new Dictionary<SplineContainer, SplineContainer>();
    List<KeyValuePair<SplineContainer, SplineContainer>> keyValueList = new List<KeyValuePair<SplineContainer, SplineContainer>>();

    // Start is called before the first frame update
    void Start()
    {
        inistializeSplinePair();
        sfx = GetComponent<AudioSource>();
    }

    public void showThree()
    {
        StartCoroutine(showThreeCups());
        flag = false;
    }

    public IEnumerator showLoseCups()
    {
        StartCoroutine(ShowBall(LeftCup));
        flag = false;
        StartCoroutine(ShowBall(MidCup));
        StartCoroutine(ShowBall(RightCup));
        yield return new WaitForSeconds(3f);
        ResetCups();
    }
    public IEnumerator showThreeCups()
    {
        StartCoroutine(ShowBall(LeftCup));
        StartCoroutine(ShowBall(MidCup));
        StartCoroutine(ShowBall(RightCup));
        yield return new WaitForSeconds(3f);
        StartCoroutine(ShuffleCups());
    }

    void inistializeSplinePair()
    {
        splinePair.Clear();
        splinePair.Add(splinesLeft[0], splinesMid[0]);
        splinePair.Add(splinesLeft[1], splinesMid[1]);
        splinePair.Add(splinesLeft[2], splinesRight[0]);
        splinePair.Add(splinesLeft[3], splinesRight[1]);
        splinePair.Add(splinesMid[2], splinesRight[2]);
        splinePair.Add(splinesMid[3], splinesRight[3]);

        keyValueList = new List<KeyValuePair<SplineContainer, SplineContainer>>(splinePair);

    }

    // Update is called once per frame
    async void Update()
    {

        ball.transform.position = new Vector3(RealCup.transform.position.x, -2.262f, RealCup.transform.position.z);
        if (RealCup.transform.position.y > -1.326)
        {
            ball.gameObject.SetActive(true);
        }
        else
        {
            ball.gameObject.SetActive(false);
        }


        if (!gate.IsActive() && !popPU.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Raycast to detect the clicked GameObject
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Check if the ray hits a collider
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(flag);
                    if (flag)
                    {
                        if (hit.collider.CompareTag("cup"))
                        {
                            flag = false;

                            StartCoroutine(showAndShuffleAsync(hit));

                        }
                    }

                }
            }
        }
    }


    public IEnumerator showAndShuffleAsync(RaycastHit hit)
    {
        StartCoroutine(ShowBall(hit.collider.gameObject));
        yield return new WaitForSeconds(2);
        if (hit.collider.gameObject.Equals(RealCup))
        {
            Task task = iLogin.VictoryAsync();
            yield return new WaitUntil(() => task.IsCompleted);

        }
        else
        {
            iLogin.Defeted();
        }

    }


    IEnumerator ShuffleCups()
    {
        for (int i = 0; i < 20; i++)
        {
            sfx.Play();
            KeyValuePair<SplineContainer, SplineContainer> randomPair = GetRandomKeyValuePair();
            flag = false;

            StartCoroutine(MoveCupPair(randomPair));
            yield return new WaitForSeconds(0.2f);
            sfx.Stop();
        }
        flag = true;
    }


    KeyValuePair<SplineContainer, SplineContainer> GetRandomKeyValuePair()
    {
        // Convert the dictionary's key-value pairs into a list
        randomIndex = Random.Range(0, keyValueList.Count);
        // Return the random key-value pair
        return keyValueList[randomIndex];
    }

    GameObject getCup(char s)
    {
        switch (s)
        {
            case 'M':
                return MidCup;
            case 'L':
                return LeftCup;
            case 'R':
                return RightCup;
        }
        return null;
    }





    IEnumerator MoveCupPair(KeyValuePair<SplineContainer, SplineContainer> cup)
    {
        GameObject CupA = getCup(cup.Key.name[0]);
        GameObject CupB = getCup(cup.Value.name[0]);
        SplineAnimate animateA = AddSplineComponent(ref CupA, cup.Key);
        SplineAnimate animateB = AddSplineComponent(ref CupB, cup.Value);
        if ((cup.Key.name[0] == 'L' && cup.Value.name[0] == 'R') || (cup.Key.name[0] == 'R' && cup.Value.name[0] == 'L'))
        {
            GameObject temp;
            temp = LeftCup;
            LeftCup = RightCup;
            RightCup = temp;
        }
        else if ((cup.Key.name[0] == 'R' && cup.Value.name[0] == 'M') || (cup.Key.name[0] == 'M' && cup.Value.name[0] == 'R'))
        {
            GameObject temp;
            temp = RightCup;
            RightCup = MidCup;
            MidCup = temp;

        }
        else if ((cup.Key.name[0] == 'L' && cup.Value.name[0] == 'M') || (cup.Key.name[0] == 'M' && cup.Value.name[0] == 'L'))
        {
            GameObject temp;
            temp = LeftCup;
            LeftCup = MidCup;
            MidCup = temp;

        }
        Debug.Log(CupA.gameObject.name + "    " + CupB.gameObject.name);
        animateA.Play();
        animateB.Play();

        // Wait for the animation to finish
        yield return new WaitForSeconds(animateA.Duration);





    }

    SplineAnimate AddSplineComponent(ref GameObject cup, SplineContainer spline)
    {
        SplineAnimate splineAnimator = cup.GetComponent<SplineAnimate>();
        if (splineAnimator == null)
        {
            splineAnimator = cup.AddComponent<SplineAnimate>();
        }
        else
        {
            Destroy(cup.GetComponent<SplineAnimate>());
            splineAnimator = cup.AddComponent<SplineAnimate>();
        }

        splineAnimator.Loop = 0;
        splineAnimator.Alignment = SplineAnimate.AlignmentMode.None;

        splineAnimator.Container = spline;
        splineAnimator.Duration = 0.2f;
        return splineAnimator;
    }

    public void ResetCups()
    {
        if (RealCup.transform.position.x == -5.26f && RealCup.transform.position.x!=0)
        {
            Vector3 tempPos;
            tempPos = MidCup.transform.position;
            MidCup.transform.position = RealCup.transform.position;
            RealCup.transform.position = tempPos;
            GameObject temp;
            temp = LeftCup;
            LeftCup = MidCup;
            MidCup = temp;
        }
        else if (RealCup.transform.position.x == 5.26f)
        {
            Vector3 tempPos;
            tempPos = MidCup.transform.position;
            MidCup.transform.position = RealCup.transform.position;
            RealCup.transform.position = tempPos;
            GameObject temp;
            temp = RightCup;
            RightCup = MidCup;
            MidCup = temp;
        }


    }

    IEnumerator ShowBall(GameObject cup)
    {

        // Calculate the initial Y position
        float startY = cup.transform.position.y;

        // Calculate the elapsed time
        float elapsedTime = 0f;

        // Move the cup towards the target position
        while (elapsedTime < 1f)
        {
            // Increment the elapsed time
            elapsedTime += Time.deltaTime * moveSpeed;

            // Calculate the new Y position using Lerp
            float newY = Mathf.Lerp(startY, 0.6f, elapsedTime);

            // Set the new position of the cup
            cup.transform.position = new Vector3(cup.transform.position.x, newY, cup.transform.position.z);

            yield return null;
        }

        // Ensure the cup reaches exactly the target position
        cup.transform.position = new Vector3(cup.transform.position.x, 0.6f, cup.transform.position.z);
        yield return new WaitForSeconds(1);
        StartCoroutine(HideBall(cup));
    }

    IEnumerator HideBall(GameObject cup)
    {
        // Calculate the initial Y position
        float startY = cup.transform.position.y;

        // Calculate the elapsed time
        float elapsedTime = 0f;

        // Move the cup towards the target position
        while (elapsedTime < 1f)
        {
            // Increment the elapsed time
            elapsedTime += Time.deltaTime * moveSpeed;

            // Calculate the new Y position using Lerp
            float newY = Mathf.Lerp(startY, -1.327f, elapsedTime);

            // Set the new position of the cup
            cup.transform.position = new Vector3(cup.transform.position.x, newY, cup.transform.position.z);

            yield return null;
        }

        // Ensure the cup reaches exactly the target position
        cup.transform.position = new Vector3(cup.transform.position.x, -1.327f, cup.transform.position.z);
        yield return new WaitForSeconds(2f);

    }


}
