using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class psychologyslidescontrollerscript : MonoBehaviour
{
    public GameObject[] slides;
    public int currentImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentImage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentImage--;
            slides[currentImage + 1].gameObject.SetActive(false);
            slides[currentImage].gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentImage++;
            slides[currentImage - 1].gameObject.SetActive(false);
            slides[currentImage].gameObject.SetActive(true);
        }
        if (currentImage >= slides.Length || currentImage < 0)
        {
            currentImage = 0;
            slides[currentImage].gameObject.SetActive(true);
        }
    }
}
