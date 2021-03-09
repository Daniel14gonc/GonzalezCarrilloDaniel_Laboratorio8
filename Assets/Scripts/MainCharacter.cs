using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    public GameObject light;
    public GameObject intermittentLight;
    public GameObject ghost;
    public AudioClip ghostScream;
    public AudioClip lightSwitch;
    public GameObject mjolnir;
    public GameObject shield;
    public GameObject helmet;
    Animator lightControl;

    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        light.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (light)
        {
            if (Input.GetMouseButtonDown(1))
            {
                light.SetActive(!light.activeSelf);
                audio.PlayOneShot(lightSwitch);
            }
        }

        if (intermittentLight)
            lightControl = intermittentLight.GetComponent<Animator>();

        Vector3 pos = Input.mousePosition;
        Ray myRay = Camera.main.ScreenPointToRay(pos);
        RaycastHit rayInfo;
        if (Physics.Raycast(myRay, out rayInfo, 3.0f))
        {
            if (rayInfo.collider.gameObject.CompareTag("Mjolnir"))
            {
                mjolnir.SetActive(true);
            }
            else if (rayInfo.collider.gameObject.CompareTag("Shield"))
                shield.SetActive(true);
            else if (rayInfo.collider.gameObject.CompareTag("Helmet"))
                helmet.SetActive(true);
            else
            {
                mjolnir.SetActive(false);
                shield.SetActive(false);
                helmet.SetActive(false);
            }
        }

        Light lightInt = intermittentLight.GetComponent<Light>();
        float algo = Mathf.Sin(Time.deltaTime * 100) * 2.0f;
        print(algo);
        lightInt.intensity = algo;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Portal"))
        {
            lightControl.SetBool("isOn", true);
        }
        else if(other.gameObject.CompareTag("Ghost"))
            ghost.SetActive(false);
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Portal"))
        {
            lightControl.SetBool("isOn", false);
            ghost.SetActive(true);
            audio.PlayOneShot(ghostScream);
        }


    }
}
