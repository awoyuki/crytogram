using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G1_UILetterLock : MonoBehaviour
{
    [SerializeField] Image locked;
    [SerializeField] GameObject DoubleLocked;
    [SerializeField] Image lock_left;
    [SerializeField] Image lock_right;
    [SerializeField] Color def_color;
    [SerializeField] Color lock_color;

    public bool unlockComplete { get; private set; }

    public void InitLock(G1_LetterStatus letterStatus)
    {
        switch (letterStatus)
        {
            case G1_LetterStatus.Lock:
                locked.gameObject.SetActive(true);
                DoubleLocked.gameObject.SetActive(false);
                break;
            case G1_LetterStatus.DoubleLock:
                locked.gameObject.SetActive(false);
                DoubleLocked.gameObject.SetActive(true);
                break;
            case G1_LetterStatus.LockRight:
                locked.gameObject.SetActive(false);
                DoubleLocked.gameObject.SetActive(true);
                lock_left.color = def_color;
                lock_right.color = lock_color;
                break;
            case G1_LetterStatus.LockLeft:
                locked.gameObject.SetActive(false);
                DoubleLocked.gameObject.SetActive(true);
                lock_left.color = lock_color;
                lock_right.color = def_color;
                break;
        }
    }

    public void UnLock()
    {
        locked.color = lock_color;
        lock_left.color = lock_color;
        lock_right.color = lock_color;
        unlockComplete = true;
    }

    public void UnDoubleLock(bool left)
    {
        if (left)
            lock_left.color = lock_color;
        else
            lock_right.color = lock_color;
    }
    
}
