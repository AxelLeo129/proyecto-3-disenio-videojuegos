using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SecondPlayer : MonoBehaviour
{

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private float input_horizontal;
    private float input_vertical;
    private float angulo_direccion;
    private float fuerza_breack_actual;
    private bool is_braking;
    private new AudioSource audio;

    [SerializeField] private float fuerza_motor;
    [SerializeField] private float fuerza_break;
    [SerializeField] private float angulo_direccion_maximo;
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;
 


    // Start is called before the first frame update
    void Start()
    {
        //GameObject.FindObjectOfType<AudioManager>().startCar();
        Time.timeScale = 1.0f;
        this.audio = GetComponent<AudioSource>();
        if (this.audio)
            this.audio.Stop();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }

    public void nextScene(int siguiente)
    {
        SceneManager.LoadScene(siguiente);
    }
    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        // Inicialización de los inputs horizontal y vertical a 0
        input_horizontal = 0;
        input_vertical = 0;

        // Verificar las teclas de flecha para el movimiento horizontal
        if (Input.GetKey(KeyCode.RightArrow))
        {
            input_horizontal = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            input_horizontal = -1;
        }

        // Verificar las teclas de flecha para el movimiento vertical
        if (Input.GetKey(KeyCode.UpArrow))
        {
            input_vertical = 1;
            if (!this.audio.isPlaying)
                this.audio.Play();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            input_vertical = -1;
            if (!this.audio.isPlaying)
                this.audio.Play();
        }
        else
        {
            // Si ninguna tecla de movimiento vertical está presionada, detener el audio
            this.audio.Stop();
        }

        // Determinar si el vehículo debe frenar
        // Nota: Asumiendo que quieras frenar cuando no se presiona la tecla de adelante (UpArrow)
        // Podrías ajustar la lógica de frenado aquí según tus necesidades
        this.is_braking = (input_vertical == 0);
    }

    private void HandleMotor()
    {
        this.frontLeftWheelCollider.motorTorque = this.input_vertical * this.fuerza_motor;
        this.frontRightWheelCollider.motorTorque = this.input_vertical * this.fuerza_motor;
        this.fuerza_breack_actual = this.is_braking ? this.fuerza_break : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        this.frontRightWheelCollider.brakeTorque = this.fuerza_breack_actual;
        this.frontLeftWheelCollider.brakeTorque = this.fuerza_breack_actual;
        this.rearLeftWheelCollider.brakeTorque = this.fuerza_breack_actual;
        this.rearRightWheelCollider.brakeTorque = this.fuerza_breack_actual;
    }

    private void HandleSteering()
    {
        this.angulo_direccion = this.angulo_direccion_maximo * this.input_horizontal;
        this.frontLeftWheelCollider.steerAngle = this.angulo_direccion;
        this.frontRightWheelCollider.steerAngle = this.angulo_direccion;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(this.frontLeftWheelCollider, this.frontLeftWheelTransform);
        UpdateSingleWheel(this.frontRightWheelCollider, this.frontRightWheelTransform);
        UpdateSingleWheel(this.rearLeftWheelCollider, this.rearLeftWheelTransform);
        UpdateSingleWheel(this.rearRightWheelCollider, this.rearRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 posicion;
        Quaternion rotacion;
        wheelCollider.GetWorldPose(out posicion, out rotacion);
        wheelTransform.rotation = rotacion;
        wheelTransform.position = posicion;
    }

}
