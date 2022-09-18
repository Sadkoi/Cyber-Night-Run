using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBehaviour : MonoBehaviour
{
    public GameObject swivel;
    //public GameObject truck;
    public GameObject title;
    public GameObject camera;

    public float cameraOffset;

    private float lastSpawnL;
    private float lastSpawnR;

    public GameObject[] vehicleList;

    public Light brakes;
    public Light turnSigR;
    public Light turnSigL;

    public GameObject songTitle;
    public Transform songOffScreen;
    public Transform songOnScreen;
    public Text speedo;
    private float time;
    public float downTime, upTime, pressTime = 0;
    public float countDown = 3.0f;
    public bool ready = false;

    public string[] SongList = {"Bad Love by Niwel","Blue Sky by Ikson","Landscape by Jarico","Island by Jarico","Alive by Ikson"}; 
    public AudioClip[] mp3Files;
    public AudioSource asource;

    //public string[] tempSongList;
    //public AudioClip[] tempmp3Files;

    // Start is called before the first frame update
    void Start()
    {
        title.SetActive(true);
        speedo.text = "";
        turnSigL.intensity = 0;
        turnSigR.intensity = 0;
        songTitle.GetComponent<Text>().text = "";
        cameraOffset = camera.transform.position.z - transform.position.z;
        swivel = GameObject.Find("swivel");
        vehicleList = GameObject.FindGameObjectsWithTag("NPC");
        Debug.Log(GameObject.FindGameObjectsWithTag("NPC").Length);
        asource = gameObject.GetComponent<AudioSource>();
    }

    void Update(){
        if(!ready){ //idle screen
            checkStart();
        }else{ //while game is running
            title.SetActive(false);
            UpdateLandTilePos();
            spawnVehicles();
            UIHandler();
            blinkers();
            audioHandler();
        }
    }

    void checkStart(){
        if (Input.anyKey && ready == false && !(Input.GetKey(KeyCode.Mouse0)) && !(Input.GetKey(KeyCode.Mouse1))){
            ready = true;
        }
    }

    void UpdateLandTilePos(){
        camera.transform.position = new Vector3(camera.transform.position.x,camera.transform.position.y,cameraOffset + transform.position.z);
        //swivel.transform.position = new Vector3(swivel.transform.position.x,swivel.transform.position.y,transform.position.z);
        if(Input.GetAxis("Vertical") < 0){
            brakes.intensity = 8f;
        }else{
            brakes.intensity = 4f;
        }
    }

    void spawnVehicles(){
        if(transform.position.z + 45 > lastSpawnL + (60 + gameObject.GetComponent<Rigidbody>().velocity.magnitude * 2)){
            lastSpawnL = transform.position.z + 45;
            lastSpawnR = transform.position.z + 45;
            Instantiate(vehicleList[Random.Range(0,(vehicleList.Length - 1))], new Vector3(-4.3f,1.46f,transform.position.z + 45), Quaternion.identity);
            Instantiate(vehicleList[Random.Range(0,(vehicleList.Length - 1))], new Vector3(4.3f,1.46f,transform.position.z + 45), Quaternion.identity);
        }
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.back, out hit, 10f)){
            print("watching meee");
        }
    }

    void UIHandler(){
        speedo.text = (int)(gameObject.GetComponent<Rigidbody>().velocity.magnitude * 4) + "";
    }

    void blinkers(){
        if(Input.GetAxis("Horizontal") > 0){
            turnSigL.intensity = 0;
            if((int)(Time.time * 2.5) % 2 == 0){
                turnSigR.intensity = 0;
            }else{
                turnSigR.intensity = 8;
            }
        }else{
            if(Input.GetAxis("Horizontal") < 0){
                turnSigR.intensity = 0;
                if((int)(Time.time * 2.5) % 2 == 0){
                    turnSigL.intensity = 0;
                }else{
                    turnSigL.intensity = 8;
                }
            }else{
                turnSigL.intensity = 0;
                turnSigR.intensity = 0;
            }
        }
    }

    void audioHandler(){
        if(!asource.isPlaying){
            int listIndex = Random.Range(0,4);
            asource.clip = mp3Files[listIndex];
            asource.Play();
            StartCoroutine(dropSongDown(SongList[listIndex]));
        }
    }

    IEnumerator dropSongDown(string Songname){
        songTitle.GetComponent<Text>().text = "Now Playing:   " + Songname;
        for(float i = songOffScreen.position.y; i > songOnScreen.position.y; i-= Time.deltaTime * 80){
            songTitle.transform.position = new Vector3(songTitle.transform.position.x,i,0f);
            yield return null; 
        }
        yield return new WaitForSeconds(2);

        for(float i = songOnScreen.position.y; i < songOffScreen.position.y; i+= Time.deltaTime * 80){
            songTitle.transform.position = new Vector3(songTitle.transform.position.x,i,0f);
            yield return null; 
        }
    }

}
