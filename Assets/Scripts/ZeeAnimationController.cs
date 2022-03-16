using System.Collections;

using UnityEngine;


public class ZeeAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool isZeeSpeaking = false;
    Animator anim;
    int listningHash = Animator.StringToHash("Listning");
    int right45Hash = Animator.StringToHash("Right_45");
    int left45Hash = Animator.StringToHash("Left_45");
    int yesHash = Animator.StringToHash("Yes");
    int noHash = Animator.StringToHash("No");
    int punchHash = Animator.StringToHash("Punch");
    int fiestBumpHash = Animator.StringToHash("FiestBump");
    int byeHash = Animator.StringToHash("Bye");
    int boredHash = Animator.StringToHash("Boored");
    int expressing1Hash = Animator.StringToHash("Expressing1");
    int expression2Hash = Animator.StringToHash("Expression2");
    int expression3Hash = Animator.StringToHash("Expression3");
    int expression4Hash = Animator.StringToHash("Expression4");
    int agreeingHash = Animator.StringToHash("Agreeing");
    int angryHash = Animator.StringToHash("Angry");
    int angry2Hash = Animator.StringToHash("Angry2");
    int arlaunghHash = Animator.StringToHash("Arlaungh");
    int boored0Hash = Animator.StringToHash("Boored0");
    int fistbumpHash = Animator.StringToHash("Fistbump");
    int mixamoGreetingHash = Animator.StringToHash("MixamoGreeting");
    int mixamoRunningTiredHash = Animator.StringToHash("MixamoRunningTired");
    int mixamoRunningJumpHash = Animator.StringToHash("MixamoRunningJump");
    int mixamoRunningFlipHash = Animator.StringToHash("MixamoRunningFlip");
    int mixamoRunningBackwardHash = Animator.StringToHash("MixamoRunningBackward");
    int mixamoJumpingUpHash = Animator.StringToHash("MixamoJumpingUp");
    int mixamoJumpingJacksHash = Animator.StringToHash("MixamoJumpingJacks");

    int runStateHash = Animator.StringToHash("Base Layer.Run");
    int animeState = 0;
    float waitTime = 12;
    bool speaking = false;
    bool forgroundAnimartionActive = false;
    IEnumerator enumerator;
    Coroutine m_MyCoroutineReference;
    WaitForSeconds waitFor, waitForLess ; //= new WaitForSeconds(waitTime);
    bool autoPlayAnimation = false;
    void Awake()
    {
        waitFor = new WaitForSeconds(waitTime);
        waitForLess = new WaitForSeconds(9);
        anim = GetComponent<Animator>();
        
       // ////Debug.Log("Start: transform " + transform.eulerAngles);
    }

    private void OnEnable()
    {
        //StartCoroutine(WaitAndPlay());
        CallRestHandler.OnAudioLoaded += PlayBefore;
    }

    private void OnDisable()
    {
        CallRestHandler.OnAudioLoaded -= PlayBefore;
    }

    private void Start()
    {
        arlaunghPose();
        forgroundAnimartionActive = false;
    }

    //public void isSpeeking()
    //{
    //    speaking = true;
    //}

    public void PlayBefore()
    {
        // play anime before anime
        isZeeSpeaking = true;
        ////Debug.Log("animeBefore: " + CallRestHandler.animeBefore);
        if (!string.Equals(CallRestHandler.animeBefore, "") && !string.Equals(CallRestHandler.animeAfter, "SMILE"))
        {
            PlayAnime(CallRestHandler.animeBefore);
            ////Debug.Log("playing animeBefore ");
            forgroundAnimartionActive = true;
            //StopCoroutine(m_MyCoroutineReference);
            //m_MyCoroutineReference = null;
        }
        //if (!string.Equals(CallRestHandler.animeAfter, ""))
        Invoke("PlayAfter", CallRestHandler.audioLength);
        Invoke("OnCallEnd", CallRestHandler.audioLength);
    }

    public void OnCallEnd()
    {
        isZeeSpeaking = false;
        if (Utils.checkafterjoke)
        {
            Invoke("checkForNeutralImotion", 2);
        }

    }

    void checkForNeutralImotion()
    {
        Utils.nwutralafterjoke = true;
    }


    public void ListingAnimation(bool nooding)
    {
        if (nooding)
        {
            PlayAnime("YES");
        }
        else
        {
            PlayAnime("LISTEN");
        }
        forgroundAnimartionActive = true;
        //StopCoroutine(m_MyCoroutineReference);
        //m_MyCoroutineReference = null;
        Invoke("RestartAnimation", 5);

    }

    public void PlayAfter()
    {
        //Debug.Log("animeAfter: " + string.Equals(CallRestHandler.animeAfter, ""));
        //Debug.Log("animeAfter: " + CallRestHandler.animeAfter);
        // play anime before anime
        if (!string.Equals(CallRestHandler.animeAfter, "") && !string.Equals(CallRestHandler.animeAfter, "SMILE"))
        {
            //Debug.Log("Playing anime after");
            PlayAnime(CallRestHandler.animeAfter);
            forgroundAnimartionActive = true;
            //if (m_MyCoroutineReference != null)
            {
                //StopCoroutine(m_MyCoroutineReference);
                //m_MyCoroutineReference = null;
                Invoke("RestartAnimation", 5);
            }
            
        }
        else
        {
            forgroundAnimartionActive = false;
            ////Debug.Log("animeAfter: m_MyCoroutineReference " + (m_MyCoroutineReference == null));
            //if (m_MyCoroutineReference == null)
            {
                //enumerator = WaitAndPlay();
                //m_MyCoroutineReference = StartCoroutine(enumerator);
            }
                //m_MyCoroutineReference = StartCoroutine(enumerator);
        }
            
    }

    public void RestartAnimation()
    {
        forgroundAnimartionActive = false;
        //if(m_MyCoroutineReference == null)
        {
            //enumerator = WaitAndPlay();
            //m_MyCoroutineReference = StartCoroutine(enumerator);
        }
                //m_MyCoroutineReference = StartCoroutine(enumerator);
    }


    public void PlayAnime(string animeName)
    {
        int serveranimeState = 1;
        switch (animeName)
        {
            case "AGREE":
                serveranimeState = 15;
                break;
            case "ANGRY":
                serveranimeState = 13;
                break;
            case "LAUNCH":
                serveranimeState = 0;
                arlaunghPose();
                break;
            case "BORED":
                serveranimeState = 16;
                break;
            case "FIST_BUMP":
                serveranimeState = 17;
                break;
            case "LISTEN":
                serveranimeState = 1;
                break;
            case "LOOK_LEFT":
                serveranimeState = 3;
                break;
            case "LOOK_RIGHT":
                serveranimeState = 2;
                break;
            case "NO":
                serveranimeState = 5;
                break;
            case "WAVE":
                serveranimeState = 6;
                break;
            case "YES":
                serveranimeState = 4;
                break;
            case "WHISPER":
                serveranimeState = 9;
                break;
            case "LEAN_LOOK":
                serveranimeState = 10;
                break;
            case "EXPRESSIVE_TALK":
                serveranimeState = 11;
                break;
        }
        randomAnime(serveranimeState);
        //resetAnime();
        servverAnimeState = serveranimeState;
        Invoke("resetServerAnime", 0.2f);
    }

    public void notSpeaking()
    {
        speaking = false;
    }

    private IEnumerator WaitAndPlay()
    {
        while (true)
        {
            if(animeState == 16 || animeState == 8 || animeState == 9)
                yield return waitFor;
            else
                yield return waitForLess;

            if (!forgroundAnimartionActive)
            {
                /*
                randomAnime(Random.Range(1, 9));
                Invoke("resetAnime", 2f);
                */
                //Debug.LogError("playNextAnime WaitAndPlay + speaking" + speaking);
                playNextAnime();
            }
            //playNextAnime();
        }
    }
    // Update is called once per frame

    public void playNextAnime()
    {
        animeState++;
        if (animeState > 17)//24
            animeState = 1;
        randomAnime(animeState);
        //resetAnime();
        Invoke("resetAnime", 0.2f);
    }

    public void ListenPose()
    {
       // ////Debug.Log("ListenPose: transform "+transform.eulerAngles);
        anim.SetBool(listningHash, true);
        //animeState = 1;
    }

    public void UnlistenPose()
    {
        anim.SetBool(listningHash, false);
        //animeState = 0;
    }
    public void boredPose()
    {
        ////Debug.Log("boredHash");
        anim.SetBool(boredHash, true);
        //animeState = 1;
    }

    public void UnboredPose()
    {
        anim.SetBool(boredHash, false);
        //animeState = 0;
    }
    public void Right45Pose()
    {
        anim.SetBool(right45Hash, true);
        //animeState = 2;
    }

    public void UnRight45Pose()
    {
        anim.SetBool(right45Hash, false);
     //   animeState = 0;
    }

    public void left45Pose()
    {
        anim.SetBool(left45Hash, true);
        //animeState = 3;
    }

    public void Unleft45Pose()
    {
        anim.SetBool(left45Hash, false);
        //animeState = 0;
    }

    public void yesPose()
    {
        anim.SetBool(yesHash, true);
        //animeState = 4;
    }

    public void UnyesPose()
    {
        anim.SetBool(yesHash, false);
        //animeState = 0;
    }

    public void noPose()
    {
        anim.SetBool(noHash, true);
        //animeState = 5;
    }

    public void UnnoPose()
    {
        anim.SetBool(noHash, false);
        //animeState = 0;
    }

    public void punchPose()
    {
        anim.SetBool(punchHash, true);
        //animeState = 6;
    }

    public void UnpunchHashPose()
    {
        anim.SetBool(punchHash, false);
        //animeState = 0;
    }

    public void fiestBumpPose()
    {
        anim.SetBool(fiestBumpHash, true);
        //animeState = 7;
    }

    public void UnfiestBumpPose()
    {
        anim.SetBool(fiestBumpHash, false);
        //animeState = 0;
    }

    public void byePose()
    {
        anim.SetBool(byeHash, true);
        //animeState = 8;
    }

    public void UnbyePose()
    {
        anim.SetBool(byeHash, false);
        //animeState = 0;
    }

    public void expressing1Pose()
    {
        anim.SetBool(expressing1Hash, true);
        //animeState = 5;
    }

    public void Unexpressing1Pose()
    {
        anim.SetBool(expressing1Hash, false);
        //animeState = 0;
    }

    public void expressing2Pose()
    {
        anim.SetBool(expression2Hash, true);
        //animeState = 5;
    }

    public void Unexpression2Pose()
    {
        anim.SetBool(expression2Hash, false);
        //animeState = 0;
    }

    public void expression3Pose()
    {
        anim.SetBool(expression3Hash, true);
        //animeState = 5;
    }

    public void Unexpression3Pose()
    {
        anim.SetBool(expression3Hash, false);
        //animeState = 0;
    }

    public void expression4Pose()
    {
        anim.SetBool(expression4Hash, true);
        //animeState = 5;
    }

    public void Unexpression4Pose()
    {
        anim.SetBool(expression4Hash, false);
        //animeState = 0;
    }

    public void angryPose()
    {
        anim.SetBool(angryHash, true);
        //animeState = 5;
    }

    public void UnangryPose()
    {
        anim.SetBool(angryHash, false);
        //animeState = 0;
    }

    public void angry2Pose()
    {
        anim.SetBool(angry2Hash, true);
        //animeState = 5;
    }

    public void Unangry2Pose()
    {
        anim.SetBool(angry2Hash, false);
        //animeState = 0;
    }

    public void arlaunghPose()
    {
        ////Debug.Log("arlaunghPose");
        if (anim == null)
            anim = GetComponent<Animator>();

        anim.SetBool(arlaunghHash, true);
        //UnarlaunghPose();
        //animeState = 5;
        Invoke("UnarlaunghPose", 0.2f);
    }

    public void UnarlaunghPose()
    {
        ////Debug.Log("UnarlaunghPose");
        anim.SetBool(arlaunghHash, false);
        enumerator = WaitAndPlay();
        m_MyCoroutineReference = StartCoroutine(enumerator);
        autoPlayAnimation = true;
        //animeState = 0;
    }

    public void boored0Pose()
    {
        anim.SetBool(boored0Hash, true);
        //animeState = 5;
    }

    public void Unboored0Pose()
    {
        anim.SetBool(boored0Hash, false);
        //animeState = 0;
    }
    
    public void fistbumpPose()
    {
        anim.SetBool(fistbumpHash, true);
        //animeState = 5;
    }

    public void UnfistbumpPose()
    {
        anim.SetBool(fistbumpHash, false);
        //animeState = 0;
    }

    public void mixamoGreetingPose()
    {
        anim.SetBool(mixamoGreetingHash, true);
    }

    public void ResetmixamoGreetingPose()
    {
        anim.SetBool(mixamoGreetingHash, false);
    }

    public void mixamoRunningTiredPose()
    {
        anim.SetBool(mixamoRunningTiredHash, true);
    }

    public void ResetmixamoRunningTiredPose()
    {
        anim.SetBool(mixamoRunningTiredHash, false);
    }

    public void mixamoRunningJumpPose()
    {
        anim.SetBool(mixamoRunningJumpHash, true);
    }

    public void ResetmixamoRunningJumpPose()
    {
        anim.SetBool(mixamoRunningJumpHash, false);
    }

    public void mixamoRunningFlipPose()
    {
        anim.SetBool(mixamoRunningFlipHash, true);
    }

    public void ResetmixamoRunningFlipPose()
    {
        anim.SetBool(mixamoRunningFlipHash, false);
    }

    public void mixamoRunningBackwardPose()
    {
        anim.SetBool(mixamoRunningBackwardHash, true);
    }

    public void ResetmixamoRunningBackwardPose()
    {
        anim.SetBool(mixamoRunningBackwardHash, false);
    }

    public void mixamoJumpingUpPose()
    {
        anim.SetBool(mixamoJumpingUpHash, true);
    }

    public void ResetmixamoJumpingUpPose()
    {
        anim.SetBool(mixamoJumpingUpHash, false);
    }

    public void mixamoJumpingJacksPose()
    {
        anim.SetBool(mixamoJumpingJacksHash, true);
    }

    public void ResetmixamoJumpingJacksPose()
    {
        anim.SetBool(mixamoJumpingJacksHash, false);
    }


    public void agreeingPose()
    {
        anim.SetBool(agreeingHash, true);
    }

    public void unagreeingPose()
    {
        anim.SetBool(agreeingHash, false);
    }

    public void resetAnime()
    {
        switch (animeState)
        {
            case 1:
                UnlistenPose();
                break;
            case 2:
                UnRight45Pose();
                break;
            case 3:
                Unleft45Pose();
                break;
            case 4:
                UnyesPose();
                break;
            case 5:
                UnnoPose();
                break;
            case 6:
                UnbyePose();
               // UnpunchHashPose();
                break;
            case 7:
                UnfiestBumpPose();
                break;
            case 8:
                UnboredPose();
                //UnbyePose();
                break;
            case 9:
                Unexpressing1Pose();
                break;
            case 10:
                Unexpression2Pose();
                break;
            case 11:
                Unexpression3Pose();
                break;
            case 12:
                Unexpression4Pose();
                break;
            case 13:
                UnangryPose();
                break;
            case 14:
                Unangry2Pose();
                break;
            case 15:
                unagreeingPose();
                break;
            case 16:
                Unboored0Pose();
                //punchPose();
                break;
            case 17:
                UnfistbumpPose();
                break;
            case 18:
                ResetmixamoGreetingPose();
                break;
            case 19:
                //ResetmixamoJumpingUpPose();
                ResetmixamoRunningTiredPose();
                break;
            case 20:
                ResetmixamoRunningJumpPose();
                break;
            case 21:
                ResetmixamoRunningFlipPose();
                break;
            case 22:
                ResetmixamoRunningBackwardPose();
                break;
            case 23:
                ResetmixamoJumpingUpPose();
                break;
            case 24:
                ResetmixamoJumpingJacksPose();
                break;
        }
    }
    int servverAnimeState = 0;
    public void resetServerAnime()
    {
        resetAnime(servverAnimeState);
    }

    public void resetAnime(int state)
    {
        switch (state)
        {
            case 1:
                UnlistenPose();
                break;
            case 2:
                UnRight45Pose();
                break;
            case 3:
                Unleft45Pose();
                break;
            case 4:
                UnyesPose();
                break;
            case 5:
                UnnoPose();
                break;
            case 6:
                UnbyePose();
                // UnpunchHashPose();
                break;
            case 7:
                UnfiestBumpPose();
                break;
            case 8:
                UnboredPose();
                //UnbyePose();
                break;
            case 9:
                Unexpressing1Pose();
                break;
            case 10:
                Unexpression2Pose();
                break;
            case 11:
                Unexpression3Pose();
                break;
            case 12:
                Unexpression4Pose();
                break;
            case 13:
                UnangryPose();
                break;
            case 14:
                Unangry2Pose();
                break;
            case 15:
                unagreeingPose();
                break;
            case 16:
                Unboored0Pose();
                //punchPose();
                break;
            case 17:
                UnfistbumpPose();
                break;
            case 18:
                ResetmixamoGreetingPose();
                break;
            case 19:
                //ResetmixamoJumpingUpPose();
                ResetmixamoRunningTiredPose();
                break;
            case 20:
                ResetmixamoRunningJumpPose();
                break;
            case 21:
                ResetmixamoRunningFlipPose();
                break;
            case 22:
                ResetmixamoRunningBackwardPose();
                break;
            case 23:
                ResetmixamoJumpingUpPose();
                break;
            case 24:
                ResetmixamoJumpingJacksPose();
                break;
        }
    }

    public void randomAnime(int state)
    {
        ////Debug.Log("randomAnime");
        switch (state)
        {
            case 1:
                ListenPose();
                break;
            case 2:
                Right45Pose();
                break;
            case 3:
                left45Pose();
                break;
            case 4:
                yesPose();
                break;
            case 5:
                noPose();
                break;
            case 6:
                byePose();
                //punchPose();
                break;
            case 7:
                fiestBumpPose();
                break;
            case 8:
                boredPose();
                //byePose();
                break;
            case 9:
                expressing1Pose();
                break;
            case 10:
                expressing2Pose();
                break;
            case 11:
                expression3Pose();
                break;
            case 12:
                expression4Pose();
                break;
            case 13:
                angryPose();
                break;
            case 14:
                angry2Pose();
                break;
            case 15:
                agreeingPose();
                break;
            case 16:
                boored0Pose();
                //punchPose();
                break;
            case 17:
                fistbumpPose();
                break;
            case 18:
                mixamoGreetingPose();
                break;
            case 19:
               // mixamoJumpingUpPose();
                mixamoRunningTiredPose();
                break;
            case 20:
                mixamoRunningJumpPose();
                break;
            case 21:
                mixamoRunningFlipPose();
                break;
            case 22:
                mixamoRunningBackwardPose();
                break;
            case 23:
                mixamoJumpingUpPose();
                break;
            case 24:
                mixamoJumpingJacksPose();
                break;
        }
    }
}
