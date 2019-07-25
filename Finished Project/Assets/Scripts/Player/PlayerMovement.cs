using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
#region Variables

    CameraBobbing bobber = new CameraBobbing();


    [SerializeField]
    private float speed, jumpForce, sensitivity = 1.2f,yVelocity, gravityFactor = -5f;

    private float camX, camY, bobbingX, bobbingY;

    public Vector3 bobbingOffset;

    public float waveLengthX, waveLengthY, amplitudeX,amplitudeY;

    bool cursorLocked = false;

    CharacterController m_CharacterController;
    Camera m_Camera;


    #region Properties

        public float Speed{get;set;}
      
        public float JumpForce{get => jumpForce;set => jumpForce = value;}
        

    #endregion
    
#endregion


    // Start is called before the first frame update
    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_Camera = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        
        
        Move(speed);

        UpdateCursor();
    }


    private void Move(float speed)
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        float tempOffset = 0;

        if(x != 0 | y != 0)
        {
            bobbingX = bobber.UpdateXPos(waveLengthX, amplitudeX);

            bobbingY = bobber.UpdateYPos(waveLengthY + 0.03f, amplitudeY * 3);
            tempOffset = 1.5f;

        }

        else
        {
            // bobbingX = 0;
            tempOffset = 1;
            bobbingY = bobber.UpdateYPos(waveLengthY, amplitudeY);
        }

        camX  = Input.GetAxisRaw("Mouse X") * sensitivity;
        camY -= Input.GetAxisRaw("Mouse Y") * sensitivity;

        camY = Mathf.Clamp(camY, -75, 75);


        #region JumpSection


        if(m_CharacterController.isGrounded)
        {
            yVelocity = gravityFactor * Time.fixedDeltaTime;
            if(Input.GetKeyDown(KeyCode.Space) && yVelocity <= 0)
            {
                yVelocity = jumpForce * Time.fixedDeltaTime;
                
            }
        }
        else
        {
            yVelocity += gravityFactor * Time.fixedDeltaTime;
        }
        

        #endregion

        if(Input.GetKey(KeyCode.LeftControl) && speed != 0)
            Speed = speed - 8;
        else if(Input.GetKey(KeyCode.LeftShift) && speed != 0)
            Speed = speed + 8;
        else
            if(speed != 0)
                Speed = speed;

        Vector3 Movement = new Vector3(x,0,y).normalized * Speed * Time.deltaTime;

        float jump = yVelocity * Time.deltaTime * Speed;
        Movement = new Vector3(Movement.x, jump, Movement.z);
        transform.Rotate(0,camX,0);
        m_Camera.transform.localRotation = Quaternion.Euler(camY,0,0);

        Movement = transform.rotation * Movement;
        m_Camera.transform.localPosition =  new Vector3(bobbingX,bobbingY, 0) + (bobbingOffset * tempOffset);
        m_CharacterController.Move(Movement);

        
    }


    void UpdateCursor()
    {
        
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            cursorLocked = false;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            cursorLocked = true;
        }

        if(cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if(!cursorLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


    }

}
