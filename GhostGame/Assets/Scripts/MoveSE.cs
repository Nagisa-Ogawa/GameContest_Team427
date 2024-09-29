using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveSE : MonoBehaviour
{

    [SerializeField] AudioClip m_Sound;
    AudioSource audioSource;

    GameObject SelectObjCurrent;
    GameObject SelectObjPrev;


    void Start()
    {
        //Component‚ðŽæ“¾
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        SelectObjCurrent = EventSystem.current.currentSelectedGameObject;

        if (SelectObjPrev != SelectObjCurrent)
        {
            if (SelectObjPrev != null)
            {
                audioSource.PlayOneShot(m_Sound);
            }

            Debug.Log(SelectObjPrev);
            Debug.Log(SelectObjCurrent);

        }

        SelectObjPrev = SelectObjCurrent;

    }
}