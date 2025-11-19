using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//public class EnterNextScene : IInteractable
//{
    //private bool enterAllowed;
    //private string sceneToLoad;

    //private void OnTriggerEnter2D(Collider collision)
    //{
    //    if (collision.GetComponent<Hatch>())
    //    {
    //        sceneToLoad = "world";
    //        enterAllowed = true;
    //    }
    //    else if (collision.GetComponent<TreeHouse>())
    //    {
    //        sceneToLoad = "attic";
    //        enterAllowed = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider collision)
    //{
    //    if (collision.GetComponent<Hatch>() || collision.GetComponent<TreeHouse>())
    //    {
    //        enterAllowed = false;
    //    }
    //}

    //protected override void Update()
    //{
    //    base.Update();
    //}

    //public override void OnInteracted()
    //{
    //    Debug.Log("EnterNextScene");
    //    if (enterAllowed && Input.GetKey(KeyCode.Return))
    //    {
    //        SceneManager.LoadScene(sceneToLoad);
    //    }
    //}

//}