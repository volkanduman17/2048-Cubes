using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private float pushForce;
    [SerializeField] private float cubeMaxPosX;
    [Space]
    [SerializeField] private TouchSlider touchSlider;
    public Cube mainCube;

    private bool isPointerDown;
    private Vector3 cubePos;

    private void Start()
    {
        SpawnCube();
        //
        touchSlider.OnPointerDownEvent += OnPointerDown;
        touchSlider.OnPointerDragEvent += OnPointerDrag;
        touchSlider.OnPointerUpEvent += OnPointerUp;
    }

    private void Update()
    {
        if (isPointerDown)
        {
            mainCube.transform.position = Vector3.Lerp(mainCube.transform.position,
                                                       cubePos,
                                                       moveSpeed * Time.deltaTime);
        }
        
    }

    private void OnPointerDown(){
        isPointerDown = true;
    }
    private void OnPointerDrag(float xMovement){
        if (isPointerDown)
        {
            cubePos = mainCube.transform.position;
            cubePos.x = xMovement * cubeMaxPosX;
        }
    }
    private void OnPointerUp(){
        if (isPointerDown)
        {
            isPointerDown = false;
            //küpü itme
            mainCube.CubeRigidbody.AddForce(Vector3.forward*pushForce, ForceMode.Impulse);

            //0.3 saniye sonra yeni küp spawn et
            Invoke("SpawnNewCube", 0.3f);

        }
    }

    private void SpawnNewCube()
    {
        mainCube.IsMainCube = false;
        SpawnCube();
    }

    private void SpawnCube()
    {
        mainCube = CubeSpawner.Instance.SpawnRandom();
        mainCube.IsMainCube = true;
        //cubepos deðerini resetle
        cubePos = mainCube.transform.position;
    }

    private void OnDestroy()
    { //listener yok etme
        touchSlider.OnPointerDownEvent -= OnPointerDown;
        touchSlider.OnPointerDragEvent -= OnPointerDrag;
        touchSlider.OnPointerUpEvent -= OnPointerUp;

    }


}
